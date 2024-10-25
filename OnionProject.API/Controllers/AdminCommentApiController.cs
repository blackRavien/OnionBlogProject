using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için gerekli
using Microsoft.AspNetCore.Mvc; // MVC yapılarına erişim sağlar
using OnionProject.Application.Models.DTOs; // DTO (Data Transfer Object) modellerini içerir
using OnionProject.Domain.AbstractRepositories; // Repository arayüzlerini içerir
using OnionProject.Domain.Entities; // Domain entity'lerini içerir
using OnionProject.Domain.Enum; // Enum tanımlarını içerir

namespace OnionProject.API.Controllers
{
    // Admin yorum API'si için rota belirleyici ve API kontrolcüsü 
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminCommentApiController : ControllerBase
    {
        private readonly ICommentRepo _commentRepo; // Yorum veritabanı işlemlerini yöneten repository

        // Repository'yi dependency injection ile alır
        public AdminCommentApiController(ICommentRepo commentRepo)
        {
            _commentRepo = commentRepo;
        }

        // Tüm yorumları listeleyen endpoint
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepo.GetAllCommentsAsync(); // Tüm yorumları asenkron şekilde getirir
            return Ok(comments); // 200 OK ile yorumları döner
        }

        // Yeni bir yorum eklemek için kullanılan endpoint
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDTO createCommentDto)
        {
            // Model doğruluğunu kontrol eder
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Geçersiz model durumunda 400 Bad Request döner
            }

            // DTO'dan Comment entity'sine dönüşüm
            var comment = new Comment
            {
                Content = createCommentDto.Content, // Yorum içeriğini DTO'dan alır
                PostId = createCommentDto.PostId, // Yorumun ilişkili olduğu post ID'sini DTO'dan alır
                UserId = createCommentDto.UserId, // Yorumu yapan kullanıcının ID'sini DTO'dan alır
                CreatedAt = DateTime.UtcNow, // Yorumun oluşturulma zamanını UTC olarak ayarlar
                CreatedDate = DateTime.UtcNow, // Veritabanı için oluşturulma tarihini ayarlar
                Status = Status.Active // Varsayılan olarak aktif durumunu ayarlar
            };

            // Yorum ekleme işlemi
            await _commentRepo.AddAsync(comment); // Veritabanına yeni yorumu asenkron olarak ekler
            return Ok(); // 200 OK yanıtı döner
        }

        // Yorum silme işlemi için kullanılan endpoint
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepo.GetCommentByIdAsync(id); // ID'ye göre yorumu getirir
            if (comment == null)
            {
                return NotFound(); // Yorum bulunamazsa 404 Not Found döner
            }

            await _commentRepo.DeleteAsync(id); // Yorum silme işlemi
            return Ok(); // 200 OK yanıtı döner
        }
    }
}

/*
    Genel Açıklama:
AdminCommentApiController, admin kullanıcılar için yorumları yönetmek amacıyla kullanılan bir API kontrolcüsüdür. ICommentRepo kullanılarak veritabanı işlemleri yapılır. Kontrolcü, yorumların listelenmesi, yeni bir yorum eklenmesi ve mevcut bir yorumun silinmesi gibi temel işlevleri sağlar. GetAllComments metodu tüm yorumları listeleyip dönerken, CreateComment yeni bir yorum eklemek için gerekli doğrulamaları yaparak veritabanına kaydeder. DeleteComment ise belirli bir yorumu ID ile bulup siler. Her bir metod, uygun HTTP yanıtları (200 OK, 404 Not Found, 400 Bad Request gibi) ile sonuç döndürür.
 */