using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.Entities;
using System.Net.Http;
using System.Security.Claims;

namespace OnionProject.MVC.Controllers
{
    [Area("User")] // Area Kullanıyorsak
    [Route("UserPostApi")]
    public class UserPostController : Controller
    {
        private readonly string uri = "https://localhost:7296"; // API URL
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IAppUserService _appUserService;
        private readonly IAuthorService _authorService;
        private readonly IGenreService _genreService;

        public UserPostController(IMapper mapper, IPostService postService, ICommentService commentService, IAuthorService authorService, IGenreService genreService)
        {
            _mapper = mapper;
            _postService = postService;
            _commentService = commentService;
            _authorService = authorService;
            _genreService = genreService;
        }

        // GET: UserPost
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name; // Kullanıcı adını al
            ViewBag.Username = username; // ViewBag ile gönder
            ViewBag.Genres = await _genreService.GetAllGenres();
            ViewBag.Authors = await _authorService.GetAuthors();

            List<GetPostsVm> posts = new List<GetPostsVm>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://localhost:7296/api/UserPostApi/Index"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    posts = JsonConvert.DeserializeObject<List<GetPostsVm>>(apiResponse);
                }
            }

            // Her post için ImagePath'i kontrol et ve gerekirse düzelt
            foreach (var post in posts)
            {
                if (!string.IsNullOrEmpty(post.ImagePath) && !post.ImagePath.StartsWith("http"))
                {
                    // Eğer ImagePath zaten tam bir URL değilse, tam URL'yi ekleyin
                    post.ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}";
                }
            }

            return View(posts);
        }

        

        // GET: UserPost/Details/5
        [Authorize]
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            PostDetailsVm postDetailsVm = new PostDetailsVm();
            using (var httpClient = new HttpClient())
            {
                // API isteği oluştur ve yanıtı al
                using (var response = await httpClient.GetAsync($"https://localhost:7296/api/AdminPostApi/Details/{id}"))
                {
                    if (response.IsSuccessStatusCode) // İstek başarılı olduysa
                    {
                        // API'den gelen JSON'u string olarak oku
                        string apiResponse = await response.Content.ReadAsStringAsync();

                        // JSON'u PostDetailsVm nesnesine deserialize et
                        postDetailsVm = JsonConvert.DeserializeObject<PostDetailsVm>(apiResponse);
                    }
                    else
                    {
                        return NotFound(); // Eğer istek başarısızsa NotFound döndür
                    }
                }
            }

            // Yorumları al
            var comments = await _commentService.GetCommentsByPostIdAsync(id);

            // View'e göndermek için bir model oluştur
            var model = new PostDetailsWithCommentVm
            {
                PostDetails = postDetailsVm, // Post detayları (API'den gelen tam resim URL'si ile)
                Comments = comments, // Yorumlar
                NewComment = new CreateCommentDTO() // Yeni yorum için boş bir DTO
            };

            return View(model); // Modeli View'e gönder
        }


        // CreateComment metodu için özel route ekliyoruz
        [Authorize]
        [HttpPost("CreateComment")]
        public async Task<IActionResult> CreateComment(PostDetailsWithCommentVm model)
        {

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// Kullanıcının ID'sini alın.


                var createCommentDto = new CreateCommentDTO
                {
                    Content = model.NewComment.Content,
                    PostId = model.PostDetails.Id,
                    UserId = userId,
                    
                };

                // API üzerinden yorum oluştur
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(createCommentDto);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{uri}/api/UserPostApi/CreateComment", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("", "Yorum eklenirken bir hata oluştu.");
                        return View("Details", model);
                    }
                }

                return RedirectToAction("Details", new { id = model.PostDetails.Id });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            // Hata mesajlarını bir log dosyasına yazdırabilirsiniz veya UI'da gösterin.
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }

            return View("Details", model);
        }



        [HttpPost("DeleteComment")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int commentId, int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // API'den yorum silme isteği gönder
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{uri}/api/UserPostApi/DeleteComment/DeleteComment/{commentId}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", new { id = postId });
                }
            }

            return RedirectToAction("Details", new { id = postId }); // Hata olsa bile detaya geri dön
        }



        // GET: UserPost/Profile
        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> Profile()
        {
            ProfileVm profileVm;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'sini alın

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/UserPostApi/GetProfile/GetProfile/{userId}"))

                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        profileVm = JsonConvert.DeserializeObject<ProfileVm>(apiResponse);
                    }
                    else
                    {
                        return NotFound(); // Profil bulunamadıysa NotFound döndür
                    }
                }
            }

            return View(profileVm);
        }


        // GET: UserPost/EditProfile
        [Authorize]
        [HttpGet("EditProfile")]
        public async Task<IActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'sini alın
            ProfileUpdateDTO updateProfileDto;

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/UserPostApi/GetProfile/GetProfile/{userId}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var currentUser = JsonConvert.DeserializeObject<AppUser>(apiResponse);

                        // ProfileUpdateDTO'yu doldurun
                        updateProfileDto = new ProfileUpdateDTO
                        {
                            FirstName = currentUser.FirstName,
                            LastName = currentUser.LastName,
                            UserName = currentUser.UserName,
                            Email = currentUser.Email,
                            PhoneNumber = currentUser.PhoneNumber,
                            
                            // Gerekirse diğer alanları da doldurun
                        };
                    }
                    else
                    {
                        return NotFound(); // Profil bulunamadıysa NotFound döndür
                    }
                }
            }

            return View("EditProfile", updateProfileDto); // EditProfile view'ına yönlendirilir
        }




        


        // POST: UserPost/EditProfile
        [Authorize]
        [HttpPost("EditProfile")]
        public async Task<IActionResult> EditProfile(ProfileUpdateDTO updateProfileDto)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcı ID'sini al
               

                // Mevcut kullanıcı bilgilerini al
                AppUser currentUser;
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"{uri}/api/UserPostApi/GetProfile/GetProfile/{userId}");
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        currentUser = JsonConvert.DeserializeObject<AppUser>(apiResponse);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı bilgileri alınamadı.");
                        return View(updateProfileDto); // Hata varsa DTO'yu geri gönder
                    }
                }

                // Güncelleme işlemleri
                currentUser.FirstName = string.IsNullOrEmpty(updateProfileDto.FirstName) ? currentUser.FirstName : updateProfileDto.FirstName;
                currentUser.LastName = string.IsNullOrEmpty(updateProfileDto.LastName) ? currentUser.LastName : updateProfileDto.LastName;
                currentUser.UserName = string.IsNullOrEmpty(updateProfileDto.UserName) ? currentUser.UserName : updateProfileDto.UserName;
                currentUser.Email = string.IsNullOrEmpty(updateProfileDto.Email) ? currentUser.Email : updateProfileDto.Email;
                currentUser.PhoneNumber = string.IsNullOrEmpty(updateProfileDto.PhoneNumber) ? currentUser.PhoneNumber : updateProfileDto.PhoneNumber;


                // Şifre güncelleme kontrolü
                if (!string.IsNullOrEmpty(updateProfileDto.Password))
                {
                    // Yeni bir PasswordHash oluştur
                    var passwordHasher = new PasswordHasher<AppUser>();
                    currentUser.PasswordHash = passwordHasher.HashPassword(currentUser, updateProfileDto.Password);
                }


                // Kullanıcıyı güncelle
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(currentUser);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{uri}/api/UserPostApi/UpdateProfile/UpdateProfile/{userId}", content);


                    if (response.IsSuccessStatusCode)
                    {
                        TempData["ProfileUpdateMessage"] = "Profiliniz başarıyla güncellendi!";
                        return RedirectToAction("Profile"); // Başarılıysa Profile view'ına yönlendir
                    }
                    else
                    {
                        TempData["ProfileUpdateMessage"] = "Profil güncelleme işlemi başarısız oldu.";
                        return RedirectToAction("Profile"); // Eğer istek başarısızsa NotFound döndür
                    }
                }


            }
            return View(updateProfileDto); // Hata durumu veya ModelState geçerli değilse DTO'yu geri döndür
        }

       
    }
}
