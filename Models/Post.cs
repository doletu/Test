using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    [Table("Post")]
    public class Post : PostBase
    {

        [Required]
        [Display(Name = "Tác giả")]
        public string AuthorId { set; get; }
        
        [ForeignKey("AuthorId")]
        [Display(Name = "Tác giả")]
        public AppUser Author { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdated { set; get; }

        // Các Image con
        public ICollection<Image> ChildrenImage { get; set; }



    }
}
