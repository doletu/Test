using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers.Identity
{
    public class AddUserRoleModel
    {
        [Required]
        public string ID { set; get; }
        public string Name { set; get; }

        public string[] RoleNames { set; get; }
        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

    }
}
