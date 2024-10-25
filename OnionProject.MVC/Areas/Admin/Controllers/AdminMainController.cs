using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OnionProject.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminMainController : Controller
    {
        //Genel Sayfa
        public IActionResult Index()
        {
            return View();
        }

        

    }
}
