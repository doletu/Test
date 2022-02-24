using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models
{
    public class FeedBackComment : CommentBase
    {


        [Display(Name = "Mức đánh giá")]
        public float Rating { set; get; }



    }
}
