﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Views.Identity.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string RoleClaimIndex => "RoleClaim";

        public static string UserClaimIndex => "Role";

        public static string Category => "Category";

        public static string Post => "Post";

        public static string PostCreate => "Create Post";
        
        public static string RoleIndex(ViewContext viewContext) => PageNavClass(viewContext, RoleClaimIndex);

        public static string RoleUserIndex(ViewContext viewContext) => PageNavClass(viewContext, UserClaimIndex);

        public static string CategoryIndex(ViewContext viewContext) => PageNavClass(viewContext, Category);

        public static string PostIndex(ViewContext viewContext) => PageNavClass(viewContext, Post);
        
        public static string CreatePost(ViewContext viewContext) => PageNavClass(viewContext, PostCreate);


        //public static string DownloadPersonalData => "DownloadPersonalData";

        //public static string DeletePersonalData => "DeletePersonalData";

        //public static string ExternalLogins => "ExternalLogins";

        //public static string PersonalData => "PersonalData";

        //public static string TwoFactorAuthentication => "TwoFactorAuthentication";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);



        //public static string DownloadPersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DownloadPersonalData);

        //public static string DeletePersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, DeletePersonalData);

        //public static string ExternalLoginsNavClass(ViewContext viewContext) => PageNavClass(viewContext, ExternalLogins);

        //public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        //public static string TwoFactorAuthenticationNavClass(ViewContext viewContext) => PageNavClass(viewContext, TwoFactorAuthentication);

        // Trả về CSS class: bằng active nếu viewContext.ViewData["ActivePage"] bằng với page
        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            if (activePage == null)
            {
                activePage = System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            }
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}