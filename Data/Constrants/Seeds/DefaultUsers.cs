using Microsoft.AspNetCore.Identity;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVC.Data.Constrants.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new AppUser
            {
                UserName = "basicuser@gmail.com",
                Email = "basicuser@gmail.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
        }
        public static async Task SeedSuperAdminAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new AppUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123456");
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
                await roleManager.SeedClaimsForAdmin();
                await roleManager.SeedClaimsForManager();
                await roleManager.SeedClaimsForUser();

            }
        }
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.SuperAdmin.ToString());

            await roleManager.AddPermissionClaim(adminRole, Items.Posts.ToString());
            await roleManager.AddPermissionClaim(adminRole, Items.Category.ToString());
            await roleManager.AddPermissionClaim(adminRole, Items.Claims.ToString());
            await roleManager.AddPermissionClaim(adminRole, Items.Users.ToString());
            await roleManager.AddPermissionClaim(adminRole, Items.Roles.ToString());
        }
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            
            var allPermissions = Permissions.GeneratePermissionsForSuperAdminModule(module);
            
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }


        public static async Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.Admin.ToString());

            await roleManager.AddAdminClaim(adminRole, Items.Posts.ToString());
            await roleManager.AddAdminClaim(adminRole, Items.Category.ToString());
            await roleManager.AddAdminClaim(adminRole, Items.Claims.ToString());
            await roleManager.AddAdminClaim(adminRole, Items.Users.ToString()); 
            await roleManager.AddAdminClaim(adminRole, Items.Roles.ToString());
        }
        public static async Task AddAdminClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);

            var allPermissions = Permissions.GeneratePermissionsForAdminModule(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
        public static async Task SeedClaimsForManager(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.Manager.ToString());

            await roleManager.AddManagerClaim(adminRole, Items.Posts.ToString());
            await roleManager.AddManagerClaim(adminRole, Items.Category.ToString());
            await roleManager.AddManagerClaim(adminRole, Items.Users.ToString());
        }
        public static async Task AddManagerClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);

            var allPermissions = Permissions.GeneratePermissionsForManagerModule(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
        public static async Task SeedClaimsForUser(this RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.Basic.ToString());

            await roleManager.AddUserClaim(adminRole, Items.Posts.ToString());
            await roleManager.AddUserClaim(adminRole, Items.Category.ToString());
        }
        public static async Task AddUserClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);

            var allPermissions = Permissions.GeneratePermissionsForUserModule(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
