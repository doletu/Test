using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Model.Role
{
    public class UserInList: AppUser
    {
        // Liệt kê các Role của User ví dụ: "Admin,Editor" ...
        public string listroles { set; get; }

    }


    public class UserListModel
    {
        public List<UserInList> users { set; get; }

        public int totalPages { set; get; }


        [BindProperty(SupportsGet = true)]
        public int pageNumber { set; get; }
        [TempData] // Sử dụng Session lưu thông báo
        public string StatusMessage { get; set; } = "Nhập thông tin để tạo role mới";


    }
}
