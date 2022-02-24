using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Model
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Nhập chính xác địa chỉ email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Không để trống")]
        [Display(Name = "Nhập username hoặc email của bạn")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Nhập đúng thông tin")]
        public string UserNameOrEmail { set; get; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Nhớ thông tin đăng nhập?")]
        public bool RememberMe { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
    }
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Password phải giống nhau.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
    public class UserModel
    {
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; }

        [Phone]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        [Display(Name = "Họ tên đầy đủ")]
        public string FullName { set; get; }

        [MaxLength(255)]
        [Display(Name = "Địa chỉ")]
        public string Address { set; get; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày sinh d/m/y")]
        // [ModelBinder(BinderType=typeof(DayMonthYearBinder))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Birthday { set; get; }

        [Display(Name = "Ảnh Đại Diện")]
        public string ImageUrl { get; set; }


        [TempData]
        public string StatusMessage { get; set; }
    }
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không giống nhau")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} dài từ {2} đến {1} ký tự.", MinimumLength = 3)]
        [DataType(DataType.Text)]
        [Display(Name = "Tên tài khoản (viết liền - không dấu)")]
        public string UserName { set; get; }


        public string ReturnUrl { get; set; }

        // Xác thực từ dịch vụ ngoài (Googe, Facebook ... bài này chứa thiết lập)
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
    }

    public class RegisterConfirmModel
    {
        public string Email { get; set; }

        public string UrlContinue { set; get; }
    }


    public class EmailModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Đổi sang email mới")]
        public string NewEmail { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
    }
    public class ConfirmEmailModel
    {

        [TempData]
        public string StatusMessage { get; set; }
    }

    public class ChangePasswordModel
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password hiện tại")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại password mới")]
        [Compare("NewPassword", ErrorMessage = "Password phải giống nhau.")]
        public string ConfirmPassword { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
    }

}
