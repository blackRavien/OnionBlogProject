using Microsoft.AspNetCore.Authorization; // Yetkilendirme ile ilgili kütüphane
using Microsoft.AspNetCore.Http; // HTTP ile ilgili sınıflar
using Microsoft.AspNetCore.Identity; // Kimlik yönetimi ile ilgili kütüphane
using Microsoft.AspNetCore.Mvc; // MVC yapısı için gerekli sınıflar
using OnionProject.Application.Models.DTOs; // Veri Transfer Nesneleri (DTO) için kullanılan namespace
using OnionProject.Application.Models.VMs; // Görünüm Modelleri (VM) için kullanılan namespace
using OnionProject.Application.Services.AbstractServices; // Uygulama servisleri için kullanılan namespace
using OnionProject.Domain.AbstractRepositories; // Veri erişim katmanı için kullanılan repository arayüzleri
using OnionProject.Domain.Entities; // Domain katmanındaki varlıklar
using System.Collections.Generic; // Koleksiyon sınıfları
using System.Security.Claims; // Kullanıcı talepleri için gerekli sınıflar
using System.Threading.Tasks; // Asenkron programlama için gerekli sınıflar

namespace OnionProject.API.Controllers
{
    // Kullanıcı gönderileri ile ilgili API kontrolör sınıfı
    [Route("api/[controller]/[action]")] // API rota tanımı, kontrolörün adını ve aksiyonu belirler
    [ApiController] // API kontrolörü olduğunu belirtir
    public class UserPostApiController : ControllerBase
    {
        // Post ve yorum servisleri ile kullanıcı repo'sunun örnekleri
        private readonly IPostService _postService;
        private readonly IAppUserRepo _appUserRepo;
        private readonly ICommentService _commentService;
        private readonly IAppUserService _appUserService;

        // Logger servisi için bir değişken (kayıt tutma)
        private readonly ILogger _logger;

        // Constructor, gerekli servislerin bağımlılıklarını alır
        public UserPostApiController(IPostService postService, ICommentService commentService, IAppUserService appUserService, IAppUserRepo appUserRepo)
        {
            _postService = postService; // Post servisi
            _commentService = commentService; // Yorum servisi
            _appUserService = appUserService; // Kullanıcı servisi
            _appUserRepo = appUserRepo; // Kullanıcı repo'su
        }

        // Tüm gönderileri almak için
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Tüm gönderileri al
            var posts = await _postService.GetPosts();

            // Gönderileri görünüm modeline dönüştür
            var postVms = posts.Select(post => new
            {
                post.Id, // Gönderi ID'si
                post.Title, // Gönderi başlığı
                post.Content, // Gönderi içeriği
                post.GenreName, // Gönderi türü
                post.AuthorFirstName, // Yazarın adı
                post.AuthorLastName, // Yazarın soyadı
                post.CreatedDate, // Gönderim tarihi
                // Resim yolunu tam URL formatında ayarla
                ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}"
            }).ToList();

            // Başarılı yanıtla gönderileri döndür
            return Ok(postVms);
        }

        // Belirli bir gönderinin detaylarını almak için
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            // Gönderi detaylarını al
            var postDetail = await _postService.GetDetail(id);
            if (postDetail == null) // Eğer gönderi bulunamazsa
            {
                return NotFound(); // 404 Not Found döndür
            }

            // PostDetailsVm oluşturma
            var postDetailsVm = new PostDetailsVm
            {
                Id = postDetail.Id,
                Title = postDetail.Title,
                Content = postDetail.Content,
                GenreName = postDetail.GenreName,
                AuthorFirstName = postDetail.AuthorFirstName,
                AuthorLastName = postDetail.AuthorLastName,
                CreatedDate = postDetail.CreatedDate,
                Comments = postDetail.Comments, // İlgili yorumlar
                AuthorDetailVm = postDetail.AuthorDetailVm, // Yazar detayları
                // Resim yolunu tam URL formatında ayarla
                ImagePath = $"{postDetail.ImagePath.TrimStart('/')}" // Tam URL'yi oluştur
            };

            // Başarılı yanıtla gönderi detaylarını döndür
            return Ok(postDetailsVm);
        }

        // Yorum oluşturmak için
        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO createCommentDto)
        {
            // Model geçerliliğini kontrol et
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Geçersizse 400 Bad Request döndür
            }

            // Yorum ekle
            await _commentService.AddCommentAsync(createCommentDto);
            return Ok("Yorum başarıyla eklendi!"); // Başarılı yanıtla mesaj döndür
        }

        // Yorum silmek için
        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            // Yorumu bul
            var comment = await _commentService.GetCommentsByPostIdAsync(commentId);

            if (comment == null) // Eğer yorum bulunamazsa
            {
                return NotFound(new { message = "Yorum bulunamadı." }); // 404 Not Found döndür
            }

            // Mevcut kullanıcının ID'sini al
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Yorumu sil
            await _commentService.DeleteCommentAsync(commentId);

            // Başarılı yanıtla mesaj döndür
            return Ok(new { message = "Yorum başarıyla silindi." });
        }

        // Kullanıcı profilini almak için
        [HttpGet("GetProfile/{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            // Kullanıcı profilini al
            var userProfile = await _appUserService.GetUserById(userId);

            if (userProfile == null) // Eğer kullanıcı bulunamazsa
            {
                return NotFound(); // 404 Not Found döndür
            }

            // ProfileVm oluşturma
            var profileVm = new ProfileVm
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                UserName = userProfile.UserName,
                Email = userProfile.Email,
                PhoneNumber = userProfile.PhoneNumber,
            };

            // Başarılı yanıtla profil bilgilerini döndür
            return Ok(profileVm);
        }

        // Kullanıcı profilini güncellemek için
        [HttpPost("UpdateProfile/{userId}")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDTO updateProfileDto, string userId)
        {
            // Model geçerliliğini kontrol et
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Geçersizse 400 Bad Request döndür
            }

            // Kullanıcıyı veritabanından al
            var user = await _appUserRepo.GetById(userId);
            if (user == null) // Eğer kullanıcı bulunamazsa
            {
                return NotFound("Kullanıcı bulunamadı."); // 404 Not Found döndür
            }

            // Profil bilgilerini güncelle
            user.FirstName = updateProfileDto.FirstName ?? user.FirstName;
            user.LastName = updateProfileDto.LastName ?? user.LastName;
            user.UserName = updateProfileDto.UserName ?? user.UserName;
            user.Email = updateProfileDto.Email ?? user.Email;
            user.PhoneNumber = updateProfileDto.PhoneNumber ?? user.PhoneNumber;
            user.PasswordHash = updateProfileDto.PasswordHash ?? user.PasswordHash;
            

            //// Şifre güncelleme kontrolü
            //if (!string.IsNullOrEmpty(updateProfileDto.Password))
            //{
            //    var passwordHasher = new PasswordHasher<AppUser>(); // Şifre hashlemek için
            //    user.PasswordHash = passwordHasher.HashPassword(user, updateProfileDto.Password); // Yeni şifreyi hashle
            //}

            // Kullanıcıyı güncelle
            await _appUserRepo.Update(user);

            // Başarılı yanıtla mesaj döndür
            return Ok("Profil başarıyla güncellendi.");
        }
    }
}

/*
    Genel Sayfa Özeti
UserPostApiController, kullanıcı gönderileri ve yorumları ile ilgili işlemleri yöneten bir API kontrolörüdür. Kontrolör, kullanıcıların gönderileri listelemesine, belirli gönderilerin detaylarını görüntülemesine, yorum eklemesine ve silmesine, ayrıca kullanıcı profillerini görüntülemesine ve güncellemesine olanak tanır.

Gönderi İşlemleri: Index metodu, tüm gönderileri alır ve bir liste olarak döndürürken, Details metodu belirli bir gönderinin detaylarını alır.
Yorum İşlemleri: CreateComment metodu, yeni bir yorum eklerken, DeleteComment metodu mevcut bir yorumu siler.
Kullanıcı Profili İşlemleri: GetProfile metodu, belirli bir kullanıcının profil bilgilerini alırken, UpdateProfile metodu kullanıcının profil bilgilerini günceller.
Bu kontrolör, asenkron işlemlerle çalışır ve gerektiğinde HTTP yanıtlarını uygun şekilde döndürerek istemcilerin doğru bilgilere erişimini sağlar.
 */