using Microsoft.AspNetCore.Mvc; // ASP.NET Core MVC için temel sınıfları sağlar
using Microsoft.EntityFrameworkCore; // Entity Framework Core kullanımı için
using OnionProject.Application.Models.DTOs; // Data Transfer Object (DTO) sınıflarını içerir
using OnionProject.Application.Models.VMs; // ViewModel sınıflarını içerir
using OnionProject.Application.Services.AbstractServices; // Servislerin soyut sınıflarını içerir
using OnionProject.Domain.AbstractRepositories; // Repository'lerin soyut sınıflarını içerir
using OnionProject.Domain.Entities; // Uygulama içinde kullanılan varlıkları içerir
using System.Security.Claims; // Kullanıcı kimlik bilgilerini yönetmek için

using SixLabors.ImageSharp; // Görüntü işleme için ImageSharp kütüphanesi
using SixLabors.ImageSharp.Processing; // Görüntü işleme yöntemlerini içerir
using SixLabors.ImageSharp.Formats; // Görüntü formatlarını içerir
using OnionProject.Infrastructure.Context; // Uygulama veri bağlamı için
using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için

namespace OnionProject.API.Controllers
{
    // API denetleyicisi için yol ve yönlendirme ayarları
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminPostApiController : ControllerBase
    {
        // Servisleri depolamak için alanlar
        private readonly IPostService _postService;
        private readonly IAuthorService _authorService;
        private readonly ICommentService _commentService;
        private readonly ICommentRepo _commentRepo;
        private readonly AppDbContext _appDbContext;

        // Yapıcı metot, bağımlılıkları enjekte eder
        public AdminPostApiController(IPostService postService, IAuthorService authorService, ICommentRepo commentRepo, ICommentService commentService, AppDbContext appDbContext)
        {
            _postService = postService;
            _authorService = authorService;
            _commentRepo = commentRepo;
            _commentService = commentService;
            _appDbContext = appDbContext;
        }

        // Tüm postları listelemek için HTTP GET metodu
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var postList = await _postService.GetPosts(); // Postları al

            // Her post için tam URL'yi oluştur
            var postVms = postList.Select(post => new
            {
                post.Id,
                post.Title,
                post.Content,
                post.GenreName,
                post.AuthorFirstName,
                post.AuthorLastName,
                post.CreatedDate,
                ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}" // Tam URL'yi oluştur
            }).ToList();

            return Ok(postVms); // PostVm listesi döner
        }

        // Belirli bir postun detaylarını almak için
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var postDetail = await _postService.GetDetail(id); // Postun detayını al
            if (postDetail == null)
            {
                return NotFound(); // Post bulunamadıysa 404 döner
            }

            // Post detayları için ViewModel oluşturma
            var postDetailsVm = new PostDetailsVm
            {
                Id = postDetail.Id,
                Title = postDetail.Title,
                Content = postDetail.Content,
                GenreName = postDetail.GenreName,
                AuthorFirstName = postDetail.AuthorFirstName,
                AuthorLastName = postDetail.AuthorLastName,
                CreatedDate = postDetail.CreatedDate,
                Comments = postDetail.Comments,
                AuthorDetailVm = postDetail.AuthorDetailVm,
                ImagePath = $"{postDetail.ImagePath.TrimStart('/')}" // Resim yolunu ayarla
            };

            return Ok(postDetailsVm); // PostDetailsVm döner
        }

        // Yazarları almak için
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorService.GetAuthors(); // Yazarları al
            return Ok(authors); // Yazarları döner
        }

        // Yeni bir post oluşturmak için HTTP POST metodu
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePostDTO post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Model geçersizse hata döner
            }

            // Post oluşturma
            await _postService.Create(post);
            return Ok(); // Başarıyla oluşturuldu
        }

        // Postu güncellemek için HTTP PUT metodu
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePostDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Model geçersizse hata döner
                }

                var post = await _postService.GetById(model.Id); // Postu al
                if (post == null)
                {
                    return NotFound(new { message = "Post bulunamadı" }); // Post bulunamazsa hata döner
                }

                // Post alanlarını güncelle
                post.Title = model.Title;
                post.Content = model.Content;
                post.AuthorId = model.AuthorId;
                post.GenreId = model.GenreId;
                post.UpdatedDate = DateTime.Now;

                // Yeni bir resim yüklenmemişse mevcut resmi koru
                if (model.UploadPath != null && model.UploadPath.Length > 0)
                {
                    using var image = Image.Load(model.UploadPath.OpenReadStream()); // Resmi yükle
                    image.Mutate(x => x.Resize(600, 500)); // Resmi boyutlandır
                    Guid guid = Guid.NewGuid(); // Yeni bir GUID oluştur
                    image.Save($"wwwroot/images/{guid}.jpg"); // Resmi kaydet
                    post.ImagePath = $"/images/{guid}.jpg"; // Yeni resmi güncelle
                }

                await _postService.Update(post); // Postu güncelle
                return Ok(); // Başarıyla güncellendi
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Sunucu hatası döner
            }
        }

        // Belirli bir postu almak için
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetById(id); // Postu al
            if (post == null)
            {
                return NotFound(new { message = "Post bulunamadı" }); // Post bulunamazsa hata döner
            }

            return Ok(post); // Postu döner
        }

        // Postu silmek için HTTP DELETE metodu
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetById(id); // Postu al
            if (post == null)
            {
                return NotFound(new { message = "Post bulunamadı" }); // Post bulunamazsa hata döner
            }

            await _postService.Delete(id); // Postu sil
            return NoContent(); // 204 No Content, silme başarılı
        }

        // Yeni bir yorum oluşturmak için HTTP POST metodu
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Model geçersizse hata döner
            }

            await _commentService.AddCommentAsync(createCommentDto); // Yorum ekle
            return Ok("Yorum başarıyla eklendi!"); // Başarı mesajı döner
        }

        // Belirli bir yorumu silmek için HTTP DELETE metodu
        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _commentService.GetCommentsByPostIdAsync(commentId); // Yorumu bul

            if (comment == null)
            {
                return NotFound(new { message = "Yorum bulunamadı." }); // Yorum bulunamazsa hata döner
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Mevcut kullanıcının ID'sini al

            await _commentService.DeleteCommentAsync(commentId); // Yorumu sil

            return Ok(new { message = "Yorum başarıyla silindi." }); // Başarı mesajı döner
        }

        // İletişim mesajlarını almak için
        [HttpGet("api/contact")]
        public async Task<IActionResult> GetContactMessages()
        {
            var contactMessages = await _appDbContext.ContactMessages.ToListAsync(); // İletişim mesajlarını al
            return Ok(contactMessages); // Mesajları döner
        }

        // Belirli bir iletişim mesajını silmek için HTTP DELETE metodu
        [HttpDelete("api/contact/{id}")]
        public async Task<IActionResult> DeleteContactMessage(int id)
        {
            var message = await _appDbContext.ContactMessages.FindAsync(id); // Mesajı bul
            if (message == null)
            {
                return NotFound(); // Mesaj bulunamazsa hata döner
            }

            _appDbContext.ContactMessages.Remove(message); // Mesajı sil
            await _appDbContext.SaveChangesAsync(); // Değişiklikleri kaydet

            return NoContent(); // 204 No Content, silme başarılı
        }
    }
}

/*
    Özet:
Bağımlılıklar: Kod, ASP.NET Core MVC ve Entity Framework Core gibi bağımlılıkları kullanır. Ayrıca, görüntü işleme için ImageSharp kütüphanesini kullanır.
Post Yönetimi: Postlar için CRUD (Oluşturma, Okuma, Güncelleme, Silme) işlemlerini gerçekleştirir. Index metodu tüm postları listelerken, Details metodu belirli bir postun detaylarını getirir.
Yorum Yönetimi: Yorum ekleme ve silme işlemlerini gerçekleştirir. CreateComment metodu yeni bir yorum eklerken, DeleteComment belirli bir yorumu siler.
İletişim Mesajları Yönetimi: İletişim mesajlarını almak ve silmek için gerekli yöntemleri sağlar.
Yetkilendirme: Kullanıcının kimlik bilgilerini kullanarak yorum silme işlemlerini yapar.
Kod, ASP.NET Core'da RESTful bir API tasarlamak için yaygın kullanılan yöntemleri gösterir. Her metodun, istekleri nasıl yönettiği ve yanıtları nasıl döndürdüğü hakkında açıklamalar içerir.
 */