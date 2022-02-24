using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class ReportingComment : CommentBase
    {
        [Display(Name = "Tình Trạng")]
        public int Status { set; get; }
    }
}
