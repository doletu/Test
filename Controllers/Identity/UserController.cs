using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Data.Model.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("changeStatus")]
        public ActionResult changeStatus(String id)
        {

            return Ok("");
        }
        [HttpPost("changeClaim")]
        public ActionResult changeClaim(RoleClaimViewModel model)
        {

            return Ok("");
        }
    }
}
