using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    [Table("Image")]
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Display(Name ="Tên Hình Ảnh")]
        public string ImageName { get; set; }


        [Display(Name = "Đường Dẫn")]
        public string ImagePath { get; set; }


        [Display(Name ="Bài Viết Gốc")]
        public int ParentId { get; set; }


        [Display(Name = "Bài Viết Gốc")]
        [ForeignKey("ParentId")]
        public Post Parent { set; get; }

     
    }
}
