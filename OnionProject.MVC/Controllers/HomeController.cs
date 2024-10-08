using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Domain.Entities;

namespace OnionProject.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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



        //[HttpPost]
        //public async Task<IActionResult> Login(LoginDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);

        //        if (user != null)
        //        {
        //            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

        //            if (result.Succeeded)
        //            {
        //                if (await _userManager.IsInRoleAsync(user, "Member"))
        //                {
        //                    return RedirectToAction("Index", "UserPost", new { area = "User" }); // Member ise UserPost sayfas�na y�nlendir
        //                }

        //                return RedirectToAction("Index", "AdminPost", new { area = "Admin" }); // Admin ise admin sayfas�na y�nlendir
        //            }

        //        }

        //        ModelState.AddModelError("", "Ge�ersiz giri� denemesi.");
        //    }


        //    return View(model);
        //}



        // Logout (��k��)



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
