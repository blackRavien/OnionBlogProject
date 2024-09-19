using Microsoft.AspNetCore.Mvc;

namespace OnionProject.MVC.Areas.Admin.Controllers
{
    public class AuthorDetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
