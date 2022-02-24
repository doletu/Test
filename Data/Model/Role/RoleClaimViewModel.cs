using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Data.Constrants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Model.Role
{
    public class RoleClaimViewModel
    {

        [BindProperty(SupportsGet = true)]
        public string roleid { set; get; }

        [TempData] // Sử dụng Session lưu thông báo
        public string StatusMessage { get; set; }

        public List<ClaimModel> claims { get; set; }
        

        //public SelectList<ClaimModel> claim { get; set; }
    }
    public class ClaimModel
    {
        public List<PermissionModel> Permissions { set; get; }
        public string Item { set; get; }
        
        
        //public bool isView { get; set; }
        //public bool isCreate { get; set; }
        //public bool isEdit { get; set; }
        //public bool isDelete { get; set; }

    }
}
