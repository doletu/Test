using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers.Identity
{
    public class ListRoleModel
    {
        public List<IdentityRole> roles { set; get; }

        [TempData] // Sử dụng Session lưu thông báo
        public string StatusMessage { get; set; } = "Nhập thông tin để tạo role mới";
    }
}
