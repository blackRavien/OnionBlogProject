using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;

namespace OnionProject.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            this._authorService = authorService;
        }

        string uri = "http://localhost:5065";
        public async Task<IActionResult> Index()
        {
            //var authorList = await _authorService.GetAuthors();
            //return View(authorList); // Burada AuthorVm kullanılıyor.
            List<AuthorVm> authors = new List<AuthorVm>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/Author/Index"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    authors = JsonConvert.DeserializeObject<List<AuthorVm>>(apiCevap);
                }
            }

            return View(authors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorDTO author)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Girdiğiniz verileri kontrol ediniz!";
                return View(author);
            }

            bool result = await _authorService.IsAuthorExists(author.FirstName, author.LastName);

            if (!result)
            {
                await _authorService.Create(author);
                TempData["Success"] = $"{author.FirstName} {author.LastName} kişisinin kaydı başarıyla oluşturuldu!";
            }
            else
            {
                TempData["Error"] = "Bu isimde kayıtlı bir yazar zaten var!";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var authorDetail = await _authorService.GetDetail(id);  // AuthorDetailVm dönüyor
            if (authorDetail == null)
            {
                return NotFound();
            }
            return View(authorDetail);
        }

    }
}
