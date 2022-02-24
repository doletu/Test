using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MVC.Models
{
    public class AppUser : IdentityUser
    {

        [MaxLength(100)]
        [Display(Name = "Họ Tên")]
        public string FullName { set; get; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày Sinh")]
        public DateTime? Birthday { set; get; }


        [MaxLength(255)]
        [Display(Name = "Địa Chỉ")]
        public string Address { set; get; }

        [Display(Name = "Tình Trạng Tài Khoản")]
        public bool Status { set; get; }

        [Display(Name = "Tên Hình Ảnh")]
        public string ImageData { get; set; }
    }
}
