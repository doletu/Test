using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class FeeckBackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
