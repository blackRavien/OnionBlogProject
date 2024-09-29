using Microsoft.AspNetCore.Mvc;

namespace OnionProject.MVC.Areas.User.Controllers
{
    public class UserPostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
