using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Model.Role
{
    public class RoleModel
    {
        public string ID { set; get; }


        [Required(ErrorMessage = "Phải nhập tên role")]
        [Display(Name = "Tên của Role")]
        [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { set; get; }


        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        [BindProperty]
        public bool IsUpdate { set; get; }
    }
}
