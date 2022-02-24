using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class CommentBase
    {

        //[Key]
        public int Id { set; get; }

        [Required(ErrorMessage = "Phải có nội dung")]
        [Display(Name = "Nội Dung")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        public string CommentText { set; get; }


        [Display(Name = "Người Đăng")]
        public string UserId { set; get; }

        [Display(Name = "Nguồn")]
        public int BlogId { set; get; }

    }
}
