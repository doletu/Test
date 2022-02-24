using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            List<BlogComment> comments = new List<BlogComment>();
            var id = 1;
            BlogComment comment = new BlogComment()
            {
                Id = id,
                CommentText = "AAAAA",
                UserName = "AAAAA",
                CommentTime = DateTime.Now,
                ParentId=0
            };
            comments.Add(comment);
            return View(comments);
        }



        public JsonResult addNewComment(BlogComment comment2)
        {
            List<BlogComment> comments = new List<BlogComment>();
            var id = 1;
            BlogComment comment = new BlogComment()
            {
                Id = id,
                CommentText = "AAAAA",
                UserName = "AAAAA",
                CommentTime = DateTime.Now,
                ParentId = 0
            };
            comments.Add(comment);
            id = 2;
            comment = new BlogComment()
            {
                Id = id,
                CommentText = "AAAAA",
                UserName = "AAAAA",
                CommentTime = DateTime.Now,
                ParentId = 1
            };
            comments.Add(comment);
            try
            {
                comments.Add(comment2);

                return Json(new { error = false, result = comments });

            }
            catch (Exception)
            {
                //Handle Error here..
            }

            return Json(new { error = true });
        }



   
    }
}
