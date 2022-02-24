using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Constrants
{
    public enum Roles
    {
        SuperAdmin,
        Admin,
        Manager,
        Basic
    }

    public enum Permission
    {
        Create,
        View,
        Edit,
        Delete,
        AuthorizeAll,
        AuthorizeSecondary,
        AuthorizeThird
    }


    public enum Items
    {
        Users,
        Roles,
        Posts,
        Claims,
        Category
    }

    public class PermissionModel
    {
        public bool isChecked { get; set; }
        public string ClaimValue { get; set; }
        public string ClaimType { get; set; }
    }





    public static class Permissions
    {
        public static List<string> GeneratePermissionsForSuperAdminModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.{Permission.Create}",
                $"Permissions.{module}.{Permission.View}",
                $"Permissions.{module}.{Permission.Edit}",
                $"Permissions.{module}.{Permission.Delete}",
                $"Permissions.{module}.{Permission.AuthorizeAll}",
            };
        }

        public static List<string> GeneratePermissionsForAdminModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.{Permission.Create}",
                $"Permissions.{module}.{Permission.View}",
                $"Permissions.{module}.{Permission.Edit}",
                $"Permissions.{module}.{Permission.Delete}",
                $"Permissions.{module}.{Permission.AuthorizeSecondary}",

            };
        }
        public static List<string> GeneratePermissionsForManagerModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.{Permission.Create}",
                $"Permissions.{module}.{Permission.View}",
                $"Permissions.{module}.{Permission.Edit}",
                $"Permissions.{module}.{Permission.Delete}",
                $"Permissions.{module}.{Permission.AuthorizeThird}",

            };
        }
        public static List<string> GeneratePermissionsForUserModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.{Permission.Create}",
                $"Permissions.{module}.{Permission.View}",
                $"Permissions.{module}.{Permission.Edit}",
                $"Permissions.{module}.{Permission.Delete}",
            };
        }



    }
}
