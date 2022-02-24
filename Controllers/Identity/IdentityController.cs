using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MimeKit;
using MVC.Data.Component;
using MVC.Data.Constrants;
using MVC.Data.Model;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace MVC.Controllers.Identity
{
    public class IdentityController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        private IWebHostEnvironment _env;
        public IdentityController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<LoginModel> logger,
            IEmailSender sender,
            IWebHostEnvironment _environment,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = sender;
            _env = _environment;
            _roleManager = roleManager;
        }

        [HttpPost]
        public ActionResult UpLoadImage(List<IFormFile> files)
        {


            var filePath = "";
            foreach (IFormFile item in Request.Form.Files)
            {
                string serverMapPath = Path.Combine(_env.WebRootPath, item.FileName);
                using (var stream = new FileStream(serverMapPath, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
                filePath = "http://localhost:5001/" + item.FileName;
            }

            return Json(new { url = filePath });


        }

        public IActionResult AccessDenied(String returnUrl)
        {
            return View();
        }

        [Authorize(Roles ="SuperAdmin,Admin")]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Không tải được tài khoản ID = '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            //"http://localhost:5001/UserImage/default.png"
            var defaultImage = _env.WebRootPath + "/UserImage/default.png";
            UserModel model = new UserModel
            {
                PhoneNumber = phoneNumber,
                Birthday = user.Birthday,
                Address = user.Address,
                FullName = user.FullName,
                Username = userName,
                ImageUrl = (String.IsNullOrEmpty(user.ImageData)) ? "https://localhost:5001/UserImage/default.png" : user.ImageData
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> IndexAsync(UserModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Không có tài khoản ID: '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                var username = await _userManager.GetUserNameAsync(user);
                var phone = await _userManager.GetPhoneNumberAsync(user);

                model = new UserModel
                {
                    PhoneNumber = phone,
                    Birthday = user.Birthday,
                    Address = user.Address,
                    FullName = user.FullName,
                    Username = username
                };
                return View(model);
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            var userName = await _userManager.GetUserNameAsync(user);

            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    model.StatusMessage = "Lỗi cập nhật số điện thoại.";
                    return View(model);
                }
            }

            var file = Request.Form.Files.FirstOrDefault();
            


            // Cập nhật các trường bổ sung
            user.Address = model.Address;
            user.Birthday = model.Birthday;
            user.FullName = model.FullName;
            //user.ImageData = ImageData;
            model.Username = userName;
            
            await _userManager.UpdateAsync(user);

            // Đăng nhập lại để làm mới Cookie (không nhớ thông tin cũ)
            await _signInManager.RefreshSignInAsync(user);
            model.StatusMessage = "Hồ sơ của bạn đã cập nhật";
            return View(model);


        }



        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Email()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Khôg nạp được tài khoản ID '{_userManager.GetUserId(User)}'.");
            }
            EmailModel model = new EmailModel();

            var email = await _userManager.GetEmailAsync(user);
            
            model.Email = email;

            model.NewEmail = email;
            model.IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Email(EmailModel model)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Khôg nạp được tài khoản ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {

                model.Email = user.Email;
                model.NewEmail = user.Email;
                model.IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

                return View(model);
            }

            var email = await _userManager.GetEmailAsync(user);
            if (model.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmEmail",
                    "Identity",
                    values: new { userId = userId, email = model.NewEmail, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    model.NewEmail,
                    "Xác nhận",
                    $"Hãy xác nhận Email của bạn bằng cách <a href='{callbackUrl}'>bấm vào đây</a>.");

                model.StatusMessage = "Hãy mở email để xác nhận thay đổi";
                return View(model);
            }

            model.StatusMessage = "Bạn đã thay đổi email.";
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationEmail(EmailModel model)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Khôg nạp được tài khoản ID '{_userManager.GetUserId(User)}'.");
            }
            var email = await _userManager.GetEmailAsync(user);
            if (!ModelState.IsValid)
            {
                model.Email = email;

                model.NewEmail = user.Email;
                model.IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

                return View(model);
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //var callbackUrl = Url.Action("ConfirmEmail",
            //    "Identity",
            //    values: new {  userId = userId, code = code },
            //    protocol: Request.Scheme);
            //await _emailSender.SendEmailAsync(
            //    email,
            //    "Xác nhận Email",
            //    $"Xác nhận email <a href='{callbackUrl}'>bấm vào đây</a>.");

            var callbackUrl = Url.Action(nameof(ConfirmEmail), "Identity", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

            string Message = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
            // string body;  

            var webRoot = _env.WebRootPath; //get wwwroot Folder  

            //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html  
            var pathToFile = webRoot
                    + Path.DirectorySeparatorChar.ToString()
                    + "Template"
                    + Path.DirectorySeparatorChar.ToString()
                    + "Email.html";

            var subject = "Confirm Account Registration";

            var builder = new BodyBuilder();
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
            {
                builder.HtmlBody = SourceReader.ReadToEnd();
            }

            string messageBody = string.Format(builder.HtmlBody,
                subject,
                String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                model.NewEmail,
                Message,
                callbackUrl
                );

            await _emailSender.SendEmailAsync(model.NewEmail, subject, messageBody);


            //await _emailSender.SendEmailAsync(model.NewEmail, subject, Message);

            model.StatusMessage = "Hãy mở email để xác nhận";
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code, string returnUrl)
        {
            ConfirmEmailModel model = new ConfirmEmailModel();
            if (userId == null || code == null)
            {
                return RedirectToPage("~/Email");
            }


            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Không tồn tại User - '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            // Xác thực email
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {

                // Đăng nhập luôn nếu xác thực email thành công
                await _signInManager.SignInAsync(user, false);

                return ViewComponent(MessagePage.COMPONENTNAME,
                    new MessagePage.Message()
                    {
                        title = "Xác thực email",
                        htmlcontent = "Đã xác thực thành công, đang chuyển hướng",
                        urlredirect = (returnUrl != null) ? returnUrl : Url.Content("~/Identity/Index")
                    }
                );
            }
            else
            {
                model.StatusMessage = "Lỗi xác nhận email";
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string returnURL=null)
        {
            LoginModel model = new LoginModel();
            if (!string.IsNullOrEmpty(model.ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, model.ErrorMessage);
            }
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/Index");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            model.ReturnUrl = model.ReturnUrl;


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel model,String returnUrl=null)
        {
            returnUrl = returnUrl ?? Url.Content("~/Identity/Index");
            // Đã đăng nhập nên chuyển hướng về Index
            if (_signInManager.IsSignedIn(User)) return Redirect("~/Identity/Index");

            if (ModelState.IsValid)
            {

                IdentityUser user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
                if (user == null)
                    user = await _userManager.FindByNameAsync(model.UserNameOrEmail);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản không tồn tại.");
                    //model.ErrorMessage= "Tài khoản không tồn tại.";
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                        user.UserName,
                        model.Password,
                        model.RememberMe,
                        true
                    );


              

                if (result.RequiresTwoFactor)
                {
                    // Nếu cấu hình đăng nhập hai yếu tố thì chuyển hướng đến LoginWith2fa
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Tài khoản bí tạm khóa.");
                    // Chuyển hướng đến trang Lockout - hiện thị thông báo
                    return RedirectToPage("./Lockout");
                }
                if (result.Succeeded)
                {

                    _logger.LogInformation("User đã đăng nhập");
                    return ViewComponent(MessagePage.COMPONENTNAME, new MessagePage.Message()
                    {
                        title = "Đã đăng nhập",
                        htmlcontent = "Đăng nhập thành công",
                        urlredirect = returnUrl
                    });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Sai Mật Khẩu!!.");
                    return View(model);
                }
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(string returnUrl=null)
        {
            RegisterModel model = new RegisterModel();
            model.ReturnUrl = returnUrl;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model,string returnUrl = null)
        {
            model.ReturnUrl = returnUrl ?? Url.Content("~/Identity/Index");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // Tạo AppUser sau đó tạo User mới (cập nhật vào db)
                var user = new AppUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Vừa tạo mới tài khoản thành công.");

                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());

                    // phát sinh token theo thông tin user để xác nhận email
                    // mỗi user dựa vào thông tin sẽ có một mã riêng, mã này nhúng vào link
                    // trong email gửi đi để người dùng xác nhận
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    // callbackUrl = /Account/ConfirmEmail?userId=useridxx&code=codexxxx
                    // Link trong email người dùng bấm vào, nó sẽ gọi Page: /Acount/ConfirmEmail để xác nhận
                    var callbackUrl = Url.Action("ConfirmEmail",
                        "Identity",
                        values: new {  userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    // Gửi email    
                    await _emailSender.SendEmailAsync(model.Email, "Xác nhận địa chỉ email",
                        $"Hãy xác nhận địa chỉ email bằng cách <a href='{callbackUrl}'>Bấm vào đây</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        // Nếu cấu hình phải xác thực email mới được đăng nhập thì chuyển hướng đến trang
                        // RegisterConfirmation - chỉ để hiện thông báo cho biết người dùng cần mở email xác nhận
                        return RedirectToAction("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        // Không cần xác thực - đăng nhập luôn
                        await _signInManager.SignInAsync(user, isPersistent: false);

                    }

                    return Redirect(model.ReturnUrl); ;
                    
                }
                // Có lỗi, đưa các lỗi thêm user vào ModelState để hiện thị ở html heleper: asp-validation-summary
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> RegisterConfirmation(string email, string returnUrl = null)
        {
            RegisterConfirmModel model = new RegisterConfirmModel();

            if (email == null)
            {
                return RedirectToAction("/");
            }


            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Không có user với email: '{email}'.");
            }

            if (user.EmailConfirmed)
            {
                // Tài khoản đã xác thực email
                return ViewComponent(MessagePage.COMPONENTNAME,
                        new MessagePage.Message()
                        {
                            title = "Thông báo",
                            htmlcontent = "Tài khoản đã xác thực, chờ chuyển hướng",
                            urlredirect = (returnUrl != null) ? returnUrl : Url.Action("/Index")
                        }

                );
            }

            model.Email = email;

            if (returnUrl != null)
            {
                model.UrlContinue = Url.Action("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
            }
            else
                model.UrlContinue = Url.Page("RegisterConfirmation", new { email = model.Email });


            return View(model);
        }



        public async Task<IActionResult> ChangePasswordAsync()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Lỗi nạp User với ID '{_userManager.GetUserId(User)}'.");
            }
            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Lỗi nạp User với ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            model.StatusMessage = "Đã thay đổi password thành công.";

            return View(model);
        }


       [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // Phát sinh Token để reset password
                // Token sẽ được kèm vào link trong email,
                // link dẫn đến trang /Account/ResetPassword để kiểm tra và đặt lại mật khẩu
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Action(nameof(ConfirmEmail), "Identity", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                string Message = "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>";
                // string body;  

                var webRoot = _env.WebRootPath; //get wwwroot Folder  

                //Get TemplateFile located at wwwroot/Templates/EmailTemplate/Register_EmailTemplate.html  
                var pathToFile = webRoot
                        + Path.DirectorySeparatorChar.ToString()
                        + "Template"
                        + Path.DirectorySeparatorChar.ToString()
                        + "Email.html";

                var subject = "Confirm Account Registration";

                var builder = new BodyBuilder();
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToFile))
                {
                    builder.HtmlBody = SourceReader.ReadToEnd();
                }
                //{0} : Subject  
                //{1} : DateTime  
                //{2} : Email  
                //{3} : Username  
                //{4} : Password  
                //{5} : Message  
                //{6} : callbackURL  

                string messageBody = string.Format(builder.HtmlBody,
                    subject,
                    String.Format("{0:dddd, d MMMM yyyy}", DateTime.Now),
                    model.Email,
                    model.Email,
                    Message,
                    callbackUrl
                    );


                await _emailSender.SendEmailAsync(model.Email, subject, messageBody);

           

                // Gửi email
                //await _emailSender.SendEmailAsync(
                //    model.Email,
                //    "Đặt lại mật khẩu",
                //    $"Để đặt lại mật khẩu hãy <a href='{callbackUrl}'>bấm vào đây</a>.");

                // Chuyển đến trang thông báo đã gửi mail để reset password
                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();

        }

        [AllowAnonymous]
        public IActionResult ResetPasswordAsync(string code = null)
        {
            if (code == null)
            {
                return BadRequest("Mã token không có.");
            }
            else
            {
                ResetPasswordModel input = new ResetPasswordModel
                { 
                    // Giải mã lại code từ code trong url (do mã này khi gửi mail 
                    // đã thực hiện Encode bằng WebEncoders.Base64UrlEncode)
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel Input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Tìm User theo email
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Không thấy user
                return RedirectToPage("./ResetPasswordConfirmation");
            }
            // Đặt lại passowrd chu user - có kiểm tra mã token khi đổi
            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

            if (result.Succeeded)
            {
                // Chuyển đến trang thông báo đã reset thành công
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public async Task<IActionResult> LogOut(string returnUrl = null)
         {
            if (!_signInManager.IsSignedIn(User)) return RedirectToAction("Login");

            await _signInManager.SignOutAsync();
            _logger.LogInformation("Người dùng đăng xuất");


            return ViewComponent(MessagePage.COMPONENTNAME,
                new MessagePage.Message()
                {
                    title = "Đã đăng xuất",
                    htmlcontent = "Đăng xuất thành công",
                    urlredirect = (returnUrl != null) ? returnUrl : Url.Action("Login")
                }
            );
        }

        

    }

}
