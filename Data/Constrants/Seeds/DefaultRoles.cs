using Microsoft.AspNetCore.Identity;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Constrants.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
        }
    }
}
