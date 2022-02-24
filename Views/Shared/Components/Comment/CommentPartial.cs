using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
namespace MVC.Views.Shared.Components.Comment
{
    [ViewComponent]
    public class CommentPartial : ViewComponent
    {
        public IViewComponentResult Invoke(List<Models.BlogComment> modelComment)
        {
            return View("_CommentPartial",modelComment);
        }

    }
}
