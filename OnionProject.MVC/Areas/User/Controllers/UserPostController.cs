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

        public UserPostController(IMapper mapper, IPostService postService, ICommentService commentService, IAuthorService authorService)
        {
            _mapper = mapper;
            _postService = postService;
            _commentService = commentService;
            _authorService = authorService; 
        }

        // GET: UserPost
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name; // Kullanıcı adını al
            ViewBag.Username = username; // ViewBag ile gönder

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
                    UserId = userId
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

        


    }
}
