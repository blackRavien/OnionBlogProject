using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Application.Services.ConcreteManagers;
using OnionProject.Domain.Entities;
using System.Security.Claims;
using System;

namespace OnionProject.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthorService _authorService;
        private readonly ICommentService _commentService;
        private readonly IGenreService _genreService;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthorService authorService, ICommentService commentService, IGenreService genreService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authorService = authorService;
            _commentService = commentService;
            _genreService = genreService;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name; // Kullanýcý adýný al
            ViewBag.Username = username; // ViewBag ile gönder
            ViewBag.Genres = await _genreService.GetAllGenres();
            ViewBag.Authors = await _authorService.GetAuthors();

            List<GetPostsVm> posts = new List<GetPostsVm>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:7296/api/UserPostApi/Index"))
                {
                    if (response.IsSuccessStatusCode) // Baþarýlý bir yanýt alýndý mý?
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        posts = JsonConvert.DeserializeObject<List<GetPostsVm>>(apiResponse);
                    }
                    else
                    {
                        // Hata durumu, isterseniz burada hata mesajýný loglayabilirsiniz
                        ModelState.AddModelError(string.Empty, "API'den veri alýnamadý.");
                        return View(new List<GetPostsVm>()); // Boþ bir liste ile geri dön
                    }
                }
            }

            // Her post için ImagePath'i kontrol et ve gerekirse düzelt
            foreach (var post in posts)
            {
                if (!string.IsNullOrEmpty(post.ImagePath) && !post.ImagePath.StartsWith("http"))
                {
                    // Eðer ImagePath zaten tam bir URL deðilse, tam URL'yi ekleyin
                    post.ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}";
                }
            }

            return View(posts);
        }




        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    var username = User.Identity.Name; // Kullanýcý adýný al
        //    ViewBag.Username = username; // ViewBag ile gönder

        //    List<GetPostsVm> posts = new List<GetPostsVm>();
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var response = await httpClient.GetAsync($"https://localhost:7296/api/UserPostApi/Index"))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            posts = JsonConvert.DeserializeObject<List<GetPostsVm>>(apiResponse);
        //        }
        //    }

        //    // Her post için ImagePath'i kontrol et ve gerekirse düzelt
        //    foreach (var post in posts)
        //    {
        //        if (!string.IsNullOrEmpty(post.ImagePath) && !post.ImagePath.StartsWith("http"))
        //        {
        //            // Eðer ImagePath zaten tam bir URL deðilse, tam URL'yi ekleyin
        //            post.ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}";
        //        }
        //    }

        //    return View(posts);
        //}




        // Register (Kayýt Olma)




        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAppUserDTO model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreatedDate = model.CreatedDate,
                    Status = model.Status
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role); // Kullanýcýya rol atamasý
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // GET: UserPost/Details/5
        
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            PostDetailsVm postDetailsVm = new PostDetailsVm();
            using (var httpClient = new HttpClient())
            {
                // API isteði oluþtur ve yanýtý al
                using (var response = await httpClient.GetAsync($"https://localhost:7296/api/AdminPostApi/Details/{id}"))
                {
                    if (response.IsSuccessStatusCode) // Ýstek baþarýlý olduysa
                    {
                        // API'den gelen JSON'u string olarak oku
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        // JSON'u PostDetailsVm nesnesine deserialize et
                        postDetailsVm = JsonConvert.DeserializeObject<PostDetailsVm>(apiResponse);
                    }
                    else
                    {
                        return NotFound(); // Eðer istek baþarýsýzsa NotFound döndür
                    }
                }
            }

            // Yorumlarý al
            var comments = await _commentService.GetCommentsByPostIdAsync(id);

            // View'e göndermek için bir model oluþtur
            var model = new PostDetailsWithCommentVm
            {
                PostDetails = postDetailsVm, // Post detaylarý (API'den gelen tam resim URL'si ile)
                Comments = comments, // Yorumlar
                NewComment = new CreateCommentDTO(), // Yeni yorum için boþ bir DTO
                AuthorDetail = new AuthorDetailVm()
            };

            return View(model); // Modeli View'e gönder
        }

        public async Task<IActionResult> AboutUs()
        {
            ViewData["Layout"] = "~/Areas/User/Views/Shared/_UserLayout.cshtml";
            var authors = await _authorService.GetAuthors();


            // Her yazarýn ImagePath'ini tam URL'ye dönüþtür
            foreach (var author in authors)
            {
                if (!string.IsNullOrEmpty(author.ImagePath) && !author.ImagePath.StartsWith("http"))
                {
                    author.ImagePath = $"https://localhost:7296{author.ImagePath}"; // Tam URL oluþtur
                }
            }

            return View(authors);
        }


        public async Task<IActionResult> ContactUs()
        {
            ViewData["Layout"] = "~/Areas/User/Views/Shared/_UserLayout.cshtml";
            return View();
        }


            // Login (Giriþ)
            [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        //
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                // Kullanýcýyý email ile bul
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // Giriþ yap
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        // Kullanýcýnýn rolünü kontrol et
                        if (await _userManager.IsInRoleAsync(user, "admin"))
                        {
                            // Adminse admin sayfasýna yönlendir
                            return Redirect("/Admin/AdminMain/Index");
                        }

                        // Standart kullanýcýysa UserPost sayfasýna yönlendir
                        return RedirectToAction("Index", "UserPost", new { area = "User" });
                    }
                }

                // Giriþ baþarýsýzsa hata mesajý ekle
                ModelState.AddModelError("", "Geçersiz giriþ denemesi.");
            }

            // Model hatalýysa ya da kullanýcý bulunamazsa, giriþ sayfasýný tekrar göster
            return View(model);
        }


        [HttpPost]
        public IActionResult CreateComment(int postId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Yorum yapabilmek için giriþ yapmalýsýnýz.";
                return RedirectToAction("Details", new { id = postId });
            }

            // Eðer kullanýcý giriþ yapmýþsa yapýlacak iþlemler:
            // Yorum ekleme iþlemi burada gerçekleþmeli.

            return RedirectToAction("Details", new { id = postId }); // Yorum eklendikten sonra da ayný sayfaya geri dönülür.
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            
            // Tarayýcý önbelleðini temizleme
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "-1";

            return RedirectToAction("Index", "Home");
        }
    }
}
