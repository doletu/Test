using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class BlogComment :CommentBase
    {
        

        [Display(Name = "Tên Người Dùng")]
        public string UserName { get; set; }


        [Display(Name = "Thời gian")]
        public DateTime CommentTime { get; set; }

        [Display(Name = "Bình Luận Gốc")]
        public int ParentId { get; set; }
    

    }
}
