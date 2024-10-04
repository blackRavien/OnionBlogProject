using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using System.Text;

namespace OnionProject.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly string uri = "https://localhost:7296"; // API URL'si
        public AuthorController(IAuthorService authorService)
        {
            this._authorService = authorService;
        }

        
        public async Task<IActionResult> Index()
        {
            //var authorList = await _authorService.GetAuthors();
            //return View(authorList); // Burada AuthorVm kullanılıyor.
            List<AuthorVm> authors = new List<AuthorVm>();
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/AuthorApi/Index"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    authors = JsonConvert.DeserializeObject<List<AuthorVm>>(apiCevap);
                }
            }

            return View(authors);
        }

        [HttpGet]
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

            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(author.FirstName), "FirstName");
                form.Add(new StringContent(author.LastName), "LastName");

                // Dosya varsa form-data'ya ekle
                if (author.Image != null) // UploadPath yerine Image kullanılacak
                {
                    var fileStreamContent = new StreamContent(author.Image.OpenReadStream());
                    form.Add(fileStreamContent, "Image", author.Image.FileName); // UploadPath yerine Image kullanılıyor
                }

                var response = await httpClient.PostAsync($"{uri}/api/AuthorApi/Create", form);
                string errorContent = await response.Content.ReadAsStringAsync();  // API'den dönen hata mesajını alıyoruz
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = $"{author.FirstName} {author.LastName} kişisinin kaydı başarıyla oluşturuldu!";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["Error"] = "Bu isimde kayıtlı bir yazar zaten var!";
                }
                else
                {
                    TempData["Error"] = "Yazar kaydı oluşturulurken bir hata meydana geldi.";
                }
            }

            return Redirect("https://localhost:7225/Admin/Author/Index");
        }


        public async Task<IActionResult> Details(int id)
        {
            AuthorDetailVm authorDetail = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AuthorApi/Details/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiCevap = await response.Content.ReadAsStringAsync();
                    authorDetail = JsonConvert.DeserializeObject<AuthorDetailVm>(apiCevap);
                }
            }

            if (authorDetail == null)
            {
                return NotFound();
            }

            return View(authorDetail);
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var author = await _authorService.GetById(id); // ID ile yazarı getir
            if (author == null)
            {
                return NotFound();
            }
            return View(author); // UpdateAuthorDTO modelini View'e gönder
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAuthorDTO author)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Hata mesajlarını konsola yazdır.
                }
                return BadRequest(ModelState); // Model hatalarını döner.
            }


            // Güncelleme işlemi için API çağrısı yapalım
            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(author.Id.ToString()), "Id");
                form.Add(new StringContent(author.FirstName), "FirstName");
                form.Add(new StringContent(author.LastName), "LastName");

                // Eğer bir resim yüklenmişse, dosyayı da ekle
                if (author.Image != null)
                {
                    var fileStreamContent = new StreamContent(author.Image.OpenReadStream());
                    form.Add(fileStreamContent, "Image", author.Image.FileName);
                }
                
                var response = await httpClient.PostAsync($"{uri}/api/AuthorApi/Update", form);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = $"{author.FirstName} {author.LastName} kişisinin kaydı başarıyla güncellendi!";
                }
                else
                {
                    TempData["Error"] = "Yazar kaydı güncellenirken bir hata meydana geldi.";
                    return View(author);
                }
            }

            return Redirect("https://localhost:7225/Admin/Author/Index"); // Güncelleme başarılıysa Index sayfasına yönlendir.
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{uri}/api/AuthorApi/Delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Yazar başarıyla silindi!";
                }
                else
                {
                    TempData["Error"] = "Yazar silinirken bir hata meydana geldi.";
                }
            }

            return Redirect("https://localhost:7225/Admin/Author/Index"); // İşlem tamamlandığında liste sayfasına yönlendir
        }





    }
}
