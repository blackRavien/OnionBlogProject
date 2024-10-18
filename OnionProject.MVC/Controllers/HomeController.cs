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
            var username = User.Identity.Name; // Kullan�c� ad�n� al
            ViewBag.Username = username; // ViewBag ile g�nder
            ViewBag.Genres = await _genreService.GetAllGenres();
            ViewBag.Authors = await _authorService.GetAuthors();

            List<GetPostsVm> posts = new List<GetPostsVm>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:7296/api/UserPostApi/Index"))
                {
                    if (response.IsSuccessStatusCode) // Ba�ar�l� bir yan�t al�nd� m�?
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        posts = JsonConvert.DeserializeObject<List<GetPostsVm>>(apiResponse);
                    }
                    else
                    {
                        // Hata durumu, isterseniz burada hata mesaj�n� loglayabilirsiniz
                        ModelState.AddModelError(string.Empty, "API'den veri al�namad�.");
                        return View(new List<GetPostsVm>()); // Bo� bir liste ile geri d�n
                    }
                }
            }

            // Her post i�in ImagePath'i kontrol et ve gerekirse d�zelt
            foreach (var post in posts)
            {
                if (!string.IsNullOrEmpty(post.ImagePath) && !post.ImagePath.StartsWith("http"))
                {
                    // E�er ImagePath zaten tam bir URL de�ilse, tam URL'yi ekleyin
                    post.ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}";
                }
            }

            return View(posts);
        }




        //[HttpGet]
        //public async Task<IActionResult> Index()
        //{
        //    var username = User.Identity.Name; // Kullan�c� ad�n� al
        //    ViewBag.Username = username; // ViewBag ile g�nder

        //    List<GetPostsVm> posts = new List<GetPostsVm>();
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var response = await httpClient.GetAsync($"https://localhost:7296/api/UserPostApi/Index"))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            posts = JsonConvert.DeserializeObject<List<GetPostsVm>>(apiResponse);
        //        }
        //    }

        //    // Her post i�in ImagePath'i kontrol et ve gerekirse d�zelt
        //    foreach (var post in posts)
        //    {
        //        if (!string.IsNullOrEmpty(post.ImagePath) && !post.ImagePath.StartsWith("http"))
        //        {
        //            // E�er ImagePath zaten tam bir URL de�ilse, tam URL'yi ekleyin
        //            post.ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}";
        //        }
        //    }

        //    return View(posts);
        //}




        // Register (Kay�t Olma)




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
                    await _userManager.AddToRoleAsync(user, model.Role); // Kullan�c�ya rol atamas�
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
                // API iste�i olu�tur ve yan�t� al
                using (var response = await httpClient.GetAsync($"https://localhost:7296/api/AdminPostApi/Details/{id}"))
                {
                    if (response.IsSuccessStatusCode) // �stek ba�ar�l� olduysa
                    {
                        // API'den gelen JSON'u string olarak oku
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        // JSON'u PostDetailsVm nesnesine deserialize et
                        postDetailsVm = JsonConvert.DeserializeObject<PostDetailsVm>(apiResponse);
                    }
                    else
                    {
                        return NotFound(); // E�er istek ba�ar�s�zsa NotFound d�nd�r
                    }
                }
            }

            // Yorumlar� al
            var comments = await _commentService.GetCommentsByPostIdAsync(id);

            // View'e g�ndermek i�in bir model olu�tur
            var model = new PostDetailsWithCommentVm
            {
                PostDetails = postDetailsVm, // Post detaylar� (API'den gelen tam resim URL'si ile)
                Comments = comments, // Yorumlar
                NewComment = new CreateCommentDTO(), // Yeni yorum i�in bo� bir DTO
                AuthorDetail = new AuthorDetailVm()
            };

            return View(model); // Modeli View'e g�nder
        }

        public async Task<IActionResult> AboutUs()
        {
            ViewData["Layout"] = "~/Areas/User/Views/Shared/_UserLayout.cshtml";
            var authors = await _authorService.GetAuthors();


            // Her yazar�n ImagePath'ini tam URL'ye d�n��t�r
            foreach (var author in authors)
            {
                if (!string.IsNullOrEmpty(author.ImagePath) && !author.ImagePath.StartsWith("http"))
                {
                    author.ImagePath = $"https://localhost:7296{author.ImagePath}"; // Tam URL olu�tur
                }
            }

            return View(authors);
        }


        public async Task<IActionResult> ContactUs()
        {
            ViewData["Layout"] = "~/Areas/User/Views/Shared/_UserLayout.cshtml";
            return View();
        }


            // Login (Giri�)
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
                // Kullan�c�y� email ile bul
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // Giri� yap
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        // Kullan�c�n�n rol�n� kontrol et
                        if (await _userManager.IsInRoleAsync(user, "admin"))
                        {
                            // Adminse admin sayfas�na y�nlendir
                            return Redirect("/Admin/AdminMain/Index");
                        }

                        // Standart kullan�c�ysa UserPost sayfas�na y�nlendir
                        return RedirectToAction("Index", "UserPost", new { area = "User" });
                    }
                }

                // Giri� ba�ar�s�zsa hata mesaj� ekle
                ModelState.AddModelError("", "Ge�ersiz giri� denemesi.");
            }

            // Model hatal�ysa ya da kullan�c� bulunamazsa, giri� sayfas�n� tekrar g�ster
            return View(model);
        }


        [HttpPost]
        public IActionResult CreateComment(int postId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Yorum yapabilmek i�in giri� yapmal�s�n�z.";
                return RedirectToAction("Details", new { id = postId });
            }

            // E�er kullan�c� giri� yapm��sa yap�lacak i�lemler:
            // Yorum ekleme i�lemi burada ger�ekle�meli.

            return RedirectToAction("Details", new { id = postId }); // Yorum eklendikten sonra da ayn� sayfaya geri d�n�l�r.
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            
            // Taray�c� �nbelle�ini temizleme
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "-1";

            return RedirectToAction("Index", "Home");
        }
    }
}
