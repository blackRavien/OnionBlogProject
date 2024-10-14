using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Application.Services.ConcreteManagers;
using OnionProject.Domain.Entities;

namespace OnionProject.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthorService _authorService;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAuthorService authorService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authorService = authorService;
        }

        public IActionResult Index()
        {
            return View();
        }


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
