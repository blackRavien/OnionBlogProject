using Microsoft.AspNetCore.Mvc;

namespace OnionProject.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminMainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
