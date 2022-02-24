using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers.NewFolder
{
    public class EditController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(PostBase post)
        {
            
            return View();
        }

        public IActionResult Create()
        {

            return View();
        }


    }
}
