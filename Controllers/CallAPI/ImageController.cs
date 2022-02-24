

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MVC.Data.Identity;
using MVC.Data.Model;
using MVC.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVC.Controllers.CallAPI
{
    [Route("api/[controller]")]
    public class ImageController : ApiController
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly AppDbContext _context;

        public ImageController(AppDbContext context, IWebHostEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
            _context = context;
        }
        //[Route("api/ImageAPI/UploadFiles")]
        //[HttpPost]
        //public HttpResponseMessage UploadFiles(UserModel model)
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }
        //    var webRootPath = _hostingEnv.WebRootPath;

        //    //Fetch the File.
        //    var file

        //    //Fetch the File Name.
        //    string fileName = Path.GetFileName(postedFile.FileName);

        //    //Save the File.
        //    postedFile.SaveAs(path + fileName);

        //    //Send OK Response to Client.
        //    return Request.CreateResponse(HttpStatusCode.OK, fileName);
        //}

        //[HttpPost]
        //[Route("api/ImageAPI/GetFiles")]
        //public HttpResponseMessage GetFiles()
        //{
        //    string path = HttpContext.User.Server.MapPath("~/Uploads/");

        //    //Fetch the Image Files.
        //    List<string> images = new List<string>();

        //    //Extract only the File Names to save data.
        //    foreach (string file in Directory.GetFiles(path))
        //    {
        //        images.Add(Path.GetFileName(file));
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, images);
        //}
    }
}
