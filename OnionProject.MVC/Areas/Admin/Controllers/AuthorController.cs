using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using System.Text;

namespace OnionProject.MVC.Areas.Admin.Controllers
{
    [Area("Admin")] // Bu controller'ın Admin alanında olduğunu belirtir.
    [Authorize(Roles = "Admin")] // Yalnızca Admin rolündeki kullanıcıların erişimine izin verir.
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService; // Yazar hizmetini kullanmak için servis
        private readonly string uri = "https://localhost:7296"; // API URL'si

        public AuthorController(IAuthorService authorService)
        {
            this._authorService = authorService; // Constructor'da authorService'i al
        }

        // Yazarların listesini görüntüleyen metod
        public async Task<IActionResult> Index()
        {
            List<AuthorVm> authors = new List<AuthorVm>(); // Yazarların saklanacağı liste
            using (var httpClient = new HttpClient())
            {
                using (var cevap = await httpClient.GetAsync($"{uri}/api/AuthorApi/Index"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync(); // API'den yanıtı al
                    authors = JsonConvert.DeserializeObject<List<AuthorVm>>(apiCevap); // JSON'u AuthorVm listesine dönüştür
                }
            }

            return View(authors); // Yazar listesini görünüme gönder
        }

        // Yazar oluşturma sayfasını görüntüleyen metod
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Boş bir oluşturma görünümü döndür
        }

        // Yazar oluşturma işlemi
        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorDTO author)
        {
            if (!ModelState.IsValid) // Model geçerli değilse
            {
                TempData["Error"] = "Girdiğiniz verileri kontrol ediniz!"; // Hata mesajı
                return View(author); // Hatalı veriyi geri döndür
            }

            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent(); // Form verileri için içerik
                form.Add(new StringContent(author.FirstName), "FirstName"); // Adı ekle
                form.Add(new StringContent(author.LastName), "LastName"); // Soyadı ekle
                form.Add(new StringContent(author.Email), "Email"); // E-postayı ekle
                form.Add(new StringContent(author.PhoneNumber ?? ""), "PhoneNumber"); // Telefon numarasını ekle (null kontrolü)
                form.Add(new StringContent(author.Biography ?? ""), "Biography"); // Biyografiyi ekle (null kontrolü)

                // Resim varsa form-data'ya ekle
                if (author.Image != null)
                {
                    var fileStreamContent = new StreamContent(author.Image.OpenReadStream());
                    form.Add(fileStreamContent, "Image", author.Image.FileName);
                }

                var response = await httpClient.PostAsync($"{uri}/api/AuthorApi/Create", form); // API'ye POST isteği gönder
                string errorContent = await response.Content.ReadAsStringAsync(); // API'den dönen hata mesajını alıyoruz
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = $"{author.FirstName} {author.LastName} kişisinin kaydı başarıyla oluşturuldu!"; // Başarılı kayıt mesajı
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["Error"] = "Bu isimde kayıtlı bir yazar zaten var!"; // Zaten kayıtlı ise hata mesajı
                }
                else
                {
                    TempData["Error"] = "Yazar kaydı oluşturulurken bir hata meydana geldi."; // Genel hata mesajı
                }
            }

            return Redirect("https://localhost:7225/Admin/Author/Index"); // Başarılı ise yazarlar listesini göster
        }

        // Yazar detaylarını görüntüleyen metod
        [AllowAnonymous] // Tüm kullanıcılar erişebilir
        public async Task<IActionResult> Details(int id)
        {
            AuthorDetailVm authorDetail = null; // Yazar detaylarını saklayacak değişken
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AuthorApi/Details/{id}"); // Yazar detaylarını al
                if (response.IsSuccessStatusCode)
                {
                    string apiCevap = await response.Content.ReadAsStringAsync(); // API yanıtını al
                    authorDetail = JsonConvert.DeserializeObject<AuthorDetailVm>(apiCevap); // JSON'u AuthorDetailVm'e dönüştür
                }
            }

            if (authorDetail == null) // Eğer yazar bulunamazsa
            {
                return NotFound(); // 404 Not Found döndür
            }

            return View(authorDetail); // Yazar detaylarını görünüme gönder
        }

        // Yazar güncelleme sayfasını görüntüleyen metod
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var author = await _authorService.GetById(id); // ID ile yazarı getir
            if (author == null) // Eğer yazar bulunamazsa
            {
                return NotFound(); // 404 Not Found döndür
            }
            return View(author); // Yazar bilgilerini güncelleme görünümüne gönder
        }

        // Yazar güncelleme işlemi
        [HttpPost]
        public async Task<IActionResult> Update(UpdateAuthorDTO author)
        {
            if (!ModelState.IsValid) // Model geçerli değilse
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors); // Hata mesajlarını al
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Hata mesajlarını konsola yazdır.
                }
                return BadRequest(ModelState); // Model hatalarını döner.
            }

            // Güncelleme işlemi için API çağrısı yapalım
            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent(); // Form verileri için içerik
                form.Add(new StringContent(author.Id.ToString()), "Id"); // ID'yi ekle
                form.Add(new StringContent(author.FirstName), "FirstName"); // Adı ekle
                form.Add(new StringContent(author.LastName), "LastName"); // Soyadı ekle
                form.Add(new StringContent(author.Email), "Email"); // E-postayı ekle
                form.Add(new StringContent(author.PhoneNumber), "PhoneNumber"); // Telefon numarasını ekle
                form.Add(new StringContent(author.Biography), "Biography"); // Biyografiyi ekle

                // Eğer bir resim yüklenmişse, dosyayı da ekle
                if (author.Image != null)
                {
                    var fileStreamContent = new StreamContent(author.Image.OpenReadStream());
                    form.Add(fileStreamContent, "Image", author.Image.FileName); // Resmi ekle
                }

                var response = await httpClient.PutAsync($"{uri}/api/AuthorApi/Update", form); // API'ye PUT isteği gönder
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = $"{author.FirstName} {author.LastName} kişisinin kaydı başarıyla güncellendi!"; // Başarılı güncelleme mesajı
                }
                else
                {
                    TempData["Error"] = "Yazar kaydı güncellenirken bir hata meydana geldi."; // Hata mesajı
                    return View(author); // Hatalı veriyi geri döndür
                }
            }

            return Redirect("https://localhost:7225/Admin/Author/Index"); // Güncelleme başarılıysa yazarlar listesini göster
        }

        // Yazar silme işlemi
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{uri}/api/AuthorApi/Delete/{id}"); // API'ye DELETE isteği gönder
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Yazar başarıyla silindi!"; // Başarılı silme mesajı
                }
                else
                {
                    TempData["Error"] = "Yazar silinirken bir hata meydana geldi."; // Hata mesajı
                }
            }

            return Redirect("https://localhost:7225/Admin/Author/Index"); // İşlem tamamlandığında yazarlar listesini göster
        }
    }
}
