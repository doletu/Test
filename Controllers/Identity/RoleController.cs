using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC.Data.Constrants;
using MVC.Data.Identity;
using MVC.Data.Model.Role;
using MVC.Models;

namespace MVC.Controllers.Identity
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;

        const int USER_PER_PAGE = 1;
        public RoleController(RoleManager<IdentityRole> roleManager,
                              AppDbContext appDbContext,
                              UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _dbContext = appDbContext;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> IndexAsync()
        {
            ListRoleModel Roles = new ListRoleModel();
            Roles.roles = await _roleManager.Roles.ToListAsync();
            return View(Roles);
        }

       

        [HttpGet]
        public async Task<ActionResult> AddOrEditAsync(string roleid)
        {
            RoleModel model = new RoleModel();
            if (roleid == null)
            {
                model.IsUpdate = false;
                model.StatusMessage = "Hãy nhập thông tin để tạo role mới";
                ModelState.Clear();
            }
            else
            {
                model.IsUpdate = true;
                var result = await _roleManager.FindByIdAsync(roleid);
                if (result != null)
                {
                    model.Name = result.Name;
                    ViewData["Title"] = "Cập nhật role : " + model.Name;
                    ModelState.Clear();
                }
                else
                {
                    model.StatusMessage = "Error: Không có thông tin về Role ID = " + roleid;
                }
            }
            model.ID = roleid;
            return View(model);
 
        }
        [HttpPost]
        public async Task<IActionResult> AddOrEditAsync(RoleModel model)
        {

            if (!ModelState.IsValid)
            {
                model.StatusMessage = null;
                return View(model);
            }
            if (model.IsUpdate)
            {
                if (model.ID == null)
                {
                    ModelState.Clear();
                    model.StatusMessage = "Error: Không có thông tin về role";
                    return View(model);
                }
                var result = await _roleManager.FindByIdAsync(model.ID);
                if (result != null)
                {
                    result.Name = model.Name;
                    var roleUpdateRs = await _roleManager.UpdateAsync(result);
                    if (roleUpdateRs.Succeeded)
                    {
                        model.StatusMessage = "Đã cập nhật role thành công";
                    }
                    else
                    {
                        model.StatusMessage = "Error: ";
                        foreach (var er in roleUpdateRs.Errors)
                        {
                            model.StatusMessage += er.Description;
                        }
                    }
                }
                else
                {
                    model.StatusMessage = "Error: Không tìm thấy Role cập nhật";
                }
            }
            else
            {
                var newRole = new IdentityRole(model.Name);
                var rsNewRole = await _roleManager.CreateAsync(newRole);
                if (rsNewRole.Succeeded)
                {
                    model.StatusMessage = $"Đã tạo role mới thành công: {newRole.Name}";
                    return RedirectToAction("Index");
                }
                else
                {
                    model.StatusMessage = "Error: ";
                    foreach (var er in rsNewRole.Errors)
                    {
                        model.StatusMessage += er.Description;
                    }
                }
            }

            return View(model);
        }
        public async Task<IActionResult> DeleteRoleAsync(string roleid)
        {
            RoleModel model = new RoleModel();
            var result = await _roleManager.FindByIdAsync(roleid);
            if (result != null)
            {
                model.IsUpdate = true;
                model.Name = result.Name;
                ModelState.Clear();
            }
            else
            {
                model.StatusMessage = "Error: Không có thông tin về Role ID = " + roleid;
            }
            model.ID = roleid;
            return View(model);
        }
            

        [HttpPost]
        public async Task<IActionResult> DeleteRoleAsync(RoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return NotFound("Không xóa được");
            }

            var role = await _roleManager.FindByIdAsync(model.ID);
            if (role == null)
            {
                return NotFound("Không thấy role cần xóa");
            }

            ModelState.Clear();

            if (model.IsUpdate)
            {
                //Xóa
                await _roleManager.DeleteAsync(role);
                model.StatusMessage = "Đã xóa " + role.Name;

                return RedirectToAction("Index");
            }
            else
            {
                model.Name = role.Name;
                model.IsUpdate = true;

            }

            return View(model);
        }

        public async Task<IActionResult> IndexClaimAsync(string roleid)
        {
           
            if (string.IsNullOrEmpty(roleid))
                return NotFound("Không có role");

            RoleClaimViewModel model = new RoleClaimViewModel();
            model.roleid = roleid;
            var role = await _roleManager.FindByIdAsync(model.roleid);

            if (role == null)
                return NotFound("Không có role");

            List<PermissionModel> user_Permissions = new List<PermissionModel>();
            user_Permissions = await (from c in _dbContext.RoleClaims
                                  where c.RoleId == model.roleid
                                  select new PermissionModel()
                                  {
                                      ClaimValue=c.ClaimValue,
                                      ClaimType=c.ClaimType
                                  }).ToListAsync();
 
            
            
            model.claims = getClaimModel(user_Permissions);
            model.StatusMessage = "Thiết lập quyền hạn sử dụng của vai trò";
        
            return View(model);
        }

        public List<ClaimModel> getClaimModel(List<PermissionModel> user_Permissions)
        {
            var Items = Enum.GetNames(typeof(Items)).ToList();

            List<ClaimModel> claimMode = new List<ClaimModel>();
            foreach (var item in Items)
            {

                List<PermissionModel> modelClaim = new List<PermissionModel>();
                List<string> itemClaims = Permissions.GeneratePermissionsForUserModule(item);
                foreach (var itemclaim in itemClaims)
                {
                    var isClaim = user_Permissions.FirstOrDefault(x => x.ClaimValue == itemclaim);
                    if (isClaim != null)
                    {
                        isClaim.isChecked = true;
                        modelClaim.Add(isClaim);
                    }
                    else modelClaim.Add(new PermissionModel()
                    {
                        isChecked = false,
                        ClaimValue = itemclaim,
                        ClaimType = "Permission"
                    }); 
                }
                claimMode.Add(new ClaimModel()
                {
                    Item = item,
                    Permissions = modelClaim
                });
            }
            return claimMode;
        }

        [HttpPost]
        public async Task<IActionResult> IndexClaimAsync(RoleClaimViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            var role = await _roleManager.FindByIdAsync(model.roleid);

            if (role == null)
                return NotFound("Không có role");
            List<PermissionModel> user_Permissions = new List<PermissionModel>();
            user_Permissions = await (from c in _dbContext.RoleClaims
                                      where c.RoleId == model.roleid
                                      select new PermissionModel()
                                      {
                                          ClaimValue = c.ClaimValue,
                                          ClaimType = c.ClaimType
                                      }).ToListAsync();

            model.claims = getClaimModel(user_Permissions);
            model.StatusMessage = "Error";
            return View(model);

        }

            public async Task<IActionResult> UserClaim(int pageNumber)
        {
            UserListModel model = new UserListModel();
            model.StatusMessage = null;

            var cuser = await _userManager.GetUserAsync(User);
            if (pageNumber == 0)
                model.pageNumber = 1;
            else model.pageNumber = pageNumber;

            var lusers = (from u in _userManager.Users
                          orderby u.UserName
                          select new UserInList()
                          {
                              Id = u.Id,
                              UserName = u.UserName,
                          });


            int totalUsers = await lusers.CountAsync();
            model.totalPages = (int)Math.Ceiling((double)totalUsers / USER_PER_PAGE);

            model.users = await lusers.Skip(USER_PER_PAGE * (model.pageNumber - 1)).Take(USER_PER_PAGE).ToListAsync();
            foreach (var user in model.users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.listroles = string.Join(",", roles.ToList());
            }
            model.StatusMessage = "DANH SÁCH NGƯỜI DÙNG";
            return View(model);
        }

    }

}
   



