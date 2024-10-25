using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs; // DTO sınıflarını ekler
using OnionProject.Application.Services.AbstractServices; // Hizmet arayüzlerini ekler

namespace OnionProject.API.Controllers
{
    [Route("api/[controller]/[action]")] // API yolunu belirler
    [ApiController] // API denetleyicisi olduğunu belirtir
    public class AuthorApiController : ControllerBase
    {
        private readonly IAuthorService _authorService; // Yazar hizmeti için bir arayüz
        private readonly IPostService _postService; // Post hizmeti için bir arayüz (kullanılmıyor, bu kodda gereksiz)

        // Yapıcı metot: Yazar hizmetini enjekte eder
        public AuthorApiController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // Tüm yazarları almak için GET isteği
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var authorList = await _authorService.GetAuthors(); // Yazar listesini al

            // Her yazar için tam URL'yi oluştur
            var authorVms = authorList.Select(author => new
            {
                author.Id,
                author.FirstName,
                author.LastName,
                author.CreatedDate,
                author.UpdatedDate,
                author.DeletedDate,
                author.Status,
                ImagePath = $"https://localhost:7296/{author.ImagePath.TrimStart('/')}", // Tam URL'yi oluştur
            }).ToList();

            return Ok(authorVms); // 200 OK ile yazarları döner
        }

        // Belirli bir yazarın detaylarını almak için GET isteği
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var authorDetail = await _authorService.GetDetail(id); // Yazar detayını al
            if (authorDetail == null)
            {
                return NotFound(); // Yazar bulunamazsa 404 Not Found döner
            }

            // Detayları döndürmeden önce ImagePath'ı tam URL olarak ayarlayın
            authorDetail.ImagePath = $"https://localhost:7296/{authorDetail.ImagePath.TrimStart('/')}"; // Tam URL'yi oluştur

            return Ok(authorDetail); // 200 OK ile yazar detaylarını döner
        }

        // Yeni bir yazar oluşturmak için POST isteği
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAuthorDTO author)
        {
            if (!ModelState.IsValid) // Model doğrulama
            {
                return BadRequest(ModelState); // Geçersizse 400 Bad Request döner
            }

            bool exists = await _authorService.IsAuthorExists(author.FirstName, author.LastName); // Yazarın var olup olmadığını kontrol et
            if (exists)
            {
                return Conflict("Bu isimde bir yazar zaten mevcut."); // Çakışma varsa 409 Conflict döner
            }

            // Resim dosyasının varlığını kontrol et
            if (author.Image != null)
            {
                // Dosya adını oluştur
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(author.Image.FileName);

                // Dosya kaydetme yolu
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                // Dosyayı kaydet
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await author.Image.CopyToAsync(stream); // Dosyayı kaydeder
                }

                // Veritabanına kaydedilecek resim yolu
                author.ImagePath = "/images/" + fileName;
            }

            await _authorService.Create(author); // Yeni yazarı oluştur
            return Ok(); // 200 OK döner
        }

        // Mevcut bir yazarı güncellemek için PUT isteği
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateAuthorDTO model)
        {
            try
            {
                if (!ModelState.IsValid) // Model doğrulama
                {
                    return BadRequest(ModelState); // Geçersizse 400 Bad Request döner
                }

                // Mevcut yazarı al
                var author = await _authorService.GetById(model.Id);
                if (author == null)
                {
                    return NotFound(new { message = "Yazar bulunamadı" }); // Yazar bulunamazsa 404 Not Found döner
                }

                // Diğer alanları güncelle, ancak CreatedDate'i değiştirme
                author.FirstName = model.FirstName;
                author.LastName = model.LastName;
                author.Image = model.Image; // Bu kısımda güncelleme yapıyorsanız dikkatli olmalısınız
                author.Email = model.Email;
                author.PhoneNumber = model.PhoneNumber;
                author.Biography = model.Biography;

                await _authorService.Update(author); // Yazar güncelle
                return Ok(); // 200 OK döner
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Hata durumunda 500 Internal Server Error döner
            }
        }

        // Belirli bir yazarı silmek için DELETE isteği
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Yazarın var olup olmadığını kontrol et
            var author = await _authorService.GetById(id);
            if (author == null)
            {
                return NotFound(new { message = "Yazar bulunamadı" }); // Yazar bulunamazsa 404 Not Found döner
            }

            // Yazar silme işlemini gerçekleştir
            await _authorService.Delete(id); // Yazar sil

            // Başarılı olduğunda 204 No Content dön
            return NoContent(); // Silme başarılı, 204 No Content döner
        }
    }
}


/*
    Özet:
Bağımlılıklar: Microsoft.AspNetCore.Mvc, OnionProject.Application.Models.DTOs, ve OnionProject.Application.Services.AbstractServices gibi bağımlılıkları içerir.
Yazar Yönetimi: Yazarlarla ilgili CRUD (Oluşturma, Okuma, Güncelleme, Silme) işlemlerini gerçekleştiren bir API denetleyicisidir.
Yazar Listesi: Index metodu, tüm yazarları getirir ve her bir yazar için tam resim URL'sini oluşturur.
Yazar Detayları: Details metodu, belirli bir yazarın detaylarını getirir ve resim yolunu tam URL olarak ayarlar.
Yeni Yazar Oluşturma: Create metodu, yeni bir yazar oluşturur ve resim dosyasını kaydeder. Eğer yazar mevcutsa çakışma döner.
Yazar Güncelleme: Update metodu, mevcut bir yazarın bilgilerini günceller. Eğer yazar bulunamazsa hata döner.
Yazar Silme: Delete metodu, belirli bir yazarı siler. Eğer yazar bulunamazsa hata döner.
Kod, ASP.NET Core'da RESTful bir API tasarımında yazar yönetimini nasıl uygulayabileceğinizi gösterir. Her metodun, istekleri nasıl yönettiği ve yanıtları nasıl döndürdüğü hakkında açıklamalar içerir.
 */