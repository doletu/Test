using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC.Data.Constrants;
using MVC.Data.Identity;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MVC.Controllers.Post
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        private readonly UserManager<AppUser> _usermanager;

        private readonly ILogger<PostController> _logger;

        public PostController(AppDbContext context,
            UserManager<AppUser> usermanager,
            ILogger<PostController> logger)
        {
            _context = context;
            _usermanager = usermanager;
            _logger = logger;
        }
        public const int ITEMS_PER_PAGE = 4;
        // GET: Admin/Post
        public async Task<IActionResult> Index([Bind(Prefix = "page")] int pageNumber)
        {

            if (pageNumber == 0)
                pageNumber = 1;

            var listPosts = _context.Posts
                .Include(p => p.Author)
                .Include("PostCategories.Category")
                .OrderByDescending(p => p.DateCreated);

            _logger.LogInformation(pageNumber.ToString());

            // Lấy tổng số dòng dữ liệu
            var totalItems = listPosts.Count();
            // Tính số trang hiện thị (mỗi trang hiện thị ITEMS_PER_PAGE mục)
            int totalPages = (int)Math.Ceiling((double)totalItems / ITEMS_PER_PAGE);

            //if (pageNumber > totalPages)
            //    return RedirectToAction(nameof(PostController.Index), new { page = totalPages });

            var posts =  listPosts
                .Skip(ITEMS_PER_PAGE * (pageNumber - 1))
                .Take(ITEMS_PER_PAGE)
                .ToList();

            // return View (await listPosts.ToListAsync());
            ViewData["pageNumber"] = pageNumber;
            ViewData["totalPages"] = totalPages;

            return View(posts);
        }
        // GET: Admin/Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [BindProperty]
        public int[] selectedCategories { set; get; }

        // GET: Admin/Post/Create
        public async Task<IActionResult> Create()
        {

            // Thông tin về User tạo Post
            var user = await _usermanager.GetUserAsync(User);
            ViewData["userpost"] =$"{user.UserName} {user.FullName}";

            // Danh mục chọn để đăng bài Post, tạo MultiSelectList
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title");
            return View();

        }

        // POST: Admin/Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Title,Description,Slug,Content")] PostBase post)
        {

            var user = await _usermanager.GetUserAsync(User);
            ViewData["userpost"] = $"{user.UserName} {user.FullName}";

            // Phát sinh Slug theo Title
            if (ModelState["Slug"].ValidationState == ModelValidationState.Invalid)
            {
                post.Slug = Utils.GenerateSlug(post.Title);
                ModelState.SetModelValue("Slug", new ValueProviderResult(post.Slug));
                // Thiết lập và kiểm tra lại Model
                ModelState.Clear();
                TryValidateModel(post);
            }

            if (selectedCategories.Length == 0)
            {
                ModelState.AddModelError(String.Empty, "Phải ít nhất một chuyên mục");
            }

            bool SlugExisted = await _context.Posts.Where(p => p.Slug == post.Slug).AnyAsync();
            if (SlugExisted)
            {
                ModelState.AddModelError(nameof(post.Slug), "Slug đã có trong Database");
            }

            if (ModelState.IsValid)
            {
                var textContent = GetHtmlImageUrlList(post.Content);
 //               for (int j = 0; j < GetHtmlImageUrlList(str).Length; j++)
 //                   3                         {
 //                   4                              //Get image path string
 //5                              string img = GetHtmlImageUrlList(str)[i];
 //                   6                              //Delete pictures on the server
 //7                              string filepath = Server.MapPath(img);
 //                   8                              FileInfo file = new FileInfo(filepath);
 //                   9                              if (file.Exists)
 //                       10                              {
 //                       11                                  file.Delete();
 //                       12                              }
 //                   13                         }

                //Tạo Post
                var newpost = new Models.Post()
                {
                    AuthorId = user.Id,
                    Title = post.Title,
                    Slug = post.Slug,
                    Content = post.Content,
                    Description = post.Description,
                    Status = 0,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    
                };
                _context.Add(newpost);
                await _context.SaveChangesAsync();

                // Chèn thông tin về PostCategory của bài Post
                foreach (var selectedCategory in selectedCategories)
                {
                    _context.Add(new PostCategory() { PostID = newpost.PostId, CategoryID = selectedCategory });
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title", selectedCategories);
            return View(post);
        }


        public static string[] GetHtmlImageUrlList(string htmlText)
        {
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);
            //Create a new MatchCollection object of matches Save the number of matched objects (img tag)
            MatchCollection matches = regImg.Matches(htmlText);
            int i = 0;
            string[] sUrlList = new string[matches.Count];
            //Traverse all img tag objects
            foreach (Match match in matches)
            {
                //Get the path src of all Imgs and save them to the array
                sUrlList[i++] = match.Groups["imgUrl"].Value;
            }
            return sUrlList;
        }


        // GET: Admin/Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var post = await _context.Posts.FindAsync (id);
            var post = await _context.Posts.Where(p => p.PostId == id)
                .Include(p => p.Author)
                .Include("PostCategories.Category").FirstOrDefaultAsync();
            if (post == null)
            {
                return NotFound();
            }

            ViewData["userpost"] = $"{post.Author.UserName} {post.Author.FullName}";
            ViewData["datecreate"] = post.DateCreated.ToShortDateString();

            // Danh mục chọn
            var selectedCates = post.PostCategories.Select(c => c.CategoryID).ToArray();
            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title", selectedCates);

            return View(post);
        }

        // POST: Admin/Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Description,Slug,Content")] PostBase post)
        {

            if (id != post.PostId)
            {
                return NotFound();
            }



            // Phát sinh Slug theo Title
            if (ModelState["Slug"].ValidationState == ModelValidationState.Invalid)
            {
                post.Slug = Utils.GenerateSlug(post.Title);
                ModelState.SetModelValue("Slug", new ValueProviderResult(post.Slug));
                // Thiết lập và kiểm tra lại Model
                ModelState.Clear();
                TryValidateModel(post);
            }

            if (selectedCategories.Length == 0)
            {
                ModelState.AddModelError(String.Empty, "Phải ít nhất một chuyên mục");
            }

            bool SlugExisted = await _context.Posts.Where(p => p.Slug == post.Slug && p.PostId != post.PostId).AnyAsync();
            if (SlugExisted)
            {
                ModelState.AddModelError(nameof(post.Slug), "Slug đã có trong Database");
            }

            if (ModelState.IsValid)
            {

                // Lấy nội dung từ DB
                var postUpdate = await _context.Posts.Where(p => p.PostId == id)
                    .Include("PostCategories.Category").FirstOrDefaultAsync();
                if (postUpdate == null)
                {
                    return NotFound();
                }

                // Cập nhật nội dung mới
                postUpdate.Title = post.Title;
                postUpdate.Description = post.Description;
                postUpdate.Content = post.Content;
                postUpdate.Slug = post.Slug;
                postUpdate.DateUpdated = DateTime.Now;

                // Các danh mục không có trong selectedCategories
                var listcateremove = postUpdate.PostCategories
                                               .Where(p => !selectedCategories.Contains(p.CategoryID))
                                               .ToList();
                listcateremove.ForEach(c => postUpdate.PostCategories.Remove(c));

                // Các ID category chưa có trong postUpdate.PostCategories
                var listCateAdd = selectedCategories
                                    .Where(
                                        id => !postUpdate.PostCategories.Where(c => c.CategoryID == id).Any()
                                    ).ToList();

                listCateAdd.ForEach(id => {
                    postUpdate.PostCategories.Add(new PostCategory()
                    {
                        PostID = postUpdate.PostId,
                        CategoryID = id
                    });
                });

                try
                {

                    _context.Update(postUpdate);

                    await _context.SaveChangesAsync();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var categories = await _context.Categories.ToListAsync();
            ViewData["categories"] = new MultiSelectList(categories, "Id", "Title", selectedCategories);
            return View(post);
        }

        // GET: Admin/Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Admin/Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }







    }
}
