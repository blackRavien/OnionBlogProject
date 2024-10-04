using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;


namespace OnionProject.MVC.Areas.Admin.Controllers
{
    //todo: Resim yükleme işlerine de bak.
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminPostController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly ICommentRepo _commentRepo;
        private readonly string uri = "https://localhost:7296"; // API URL

        // DI ile repo'yu controller'a enjekte edin
        public AdminPostController(ICommentRepo commentRepo, IMapper mapper, ICommentService commentService, IPostService postService)
        {
            _commentRepo = commentRepo;
            _mapper = mapper;
            _commentService = commentService;
            _postService = postService;
            
        }

        public async Task<IActionResult> Index()
        {
            List<GetPostsVm> posts = new List<GetPostsVm>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/AdminPostApi/Index"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    posts = JsonConvert.DeserializeObject<List<GetPostsVm>>(apiResponse);
                }
            }
            return View(posts);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            List<GenreVm> genres = new List<GenreVm>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminGenreApi");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    genres = JsonConvert.DeserializeObject<List<GenreVm>>(apiResponse);
                }
            }
            ViewBag.Genres = genres;


            List<AuthorVm> authors = new List<AuthorVm>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AuthorApi/Index");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    authors = JsonConvert.DeserializeObject<List<AuthorVm>>(apiResponse);
                }
            }
            ViewBag.Authors = authors;


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDTO post)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Girdiğiniz verileri kontrol ediniz!";
                return View(post);
            }

            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(post.Title), "Title");
                form.Add(new StringContent(post.Content), "Content");
                form.Add(new StringContent(post.AuthorId.ToString()), "AuthorId");
                form.Add(new StringContent(post.GenreId.ToString()), "GenreId");

                if (post.UploadPath != null)
                {
                    var fileStreamContent = new StreamContent(post.UploadPath.OpenReadStream());
                    form.Add(fileStreamContent, "UploadPath", post.UploadPath.FileName);
                }

                var response = await httpClient.PostAsync($"{uri}/api/AdminPostApi/Create", form);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Post başarıyla oluşturuldu!";
                    return Redirect("https://localhost:7225/Admin/AdminPost/Index");
                }
                TempData["Error"] = "Post oluşturulurken hata meydana geldi!";
            }
            return Redirect("https://localhost:7225/Admin/AdminPost/Index");
        }


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


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            //Genre'leri alma
            List<GenreVm> genres = new List<GenreVm>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminGenreApi");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    genres = JsonConvert.DeserializeObject<List<GenreVm>>(apiResponse);
                }
            }
            ViewBag.Genres = genres;

            //Author'ları alma
            List<AuthorVm> authors = new List<AuthorVm>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AuthorApi/Index");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    authors = JsonConvert.DeserializeObject<List<AuthorVm>>(apiResponse);
                }
            }
            ViewBag.Authors = authors;

            //Post'u alma
            UpdatePostDTO post = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminPostApi/GetPost/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    post = JsonConvert.DeserializeObject<UpdatePostDTO>(apiResponse);
                }
            }

            //GÜNCELLEME bura eklendi
            if (post == null)
            {
                return NotFound(); // Eğer post bulunamazsa hata döner
            }


            return View(post); //GÜNCELLEME içine post yazdık

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdatePostDTO post)
        {
            if (!ModelState.IsValid) return View(post);

            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(post.Id.ToString()), "Id");
                form.Add(new StringContent(post.Title), "Title");
                form.Add(new StringContent(post.Content), "Content");
                form.Add(new StringContent(post.AuthorId.ToString()), "AuthorId");
                form.Add(new StringContent(post.GenreId.ToString()), "GenreId");
                form.Add(new StringContent(post.CreatedDate.ToString("o")), "CreatedDate"); // CreatedDate eklendi


                if (post.UploadPath != null)
                {
                    var fileStreamContent = new StreamContent(post.UploadPath.OpenReadStream());
                    form.Add(fileStreamContent, "UploadPath", post.UploadPath.FileName);
                }

                var response = await httpClient.PutAsync($"{uri}/api/AdminPostApi/Update", form);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Post başarıyla güncellendi!";
                    return Redirect("https://localhost:7225/Admin/AdminPost/Index");
                }
                TempData["Error"] = "Post güncellenirken hata meydana geldi!";
            }
            return Redirect("https://localhost:7225/Admin/AdminPost/Index");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{uri}/api/AdminPostApi/Delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Post başarıyla silindi!";
                }
                else
                {
                    TempData["Error"] = "Post silinirken hata meydana geldi!";
                }
            }
            return Redirect("https://localhost:7225/Admin/AdminPost/Index");
        }

        private async Task<UpdatePostDTO> GetPostById(int id)
        {
            UpdatePostDTO post = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminPostApi/Details/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    post = JsonConvert.DeserializeObject<UpdatePostDTO>(apiResponse);
                }
            }
            return post;
        }

        [HttpPost]
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

                return Redirect($"https://localhost:7225/Admin/AdminPost/Details/{createCommentDto.PostId}");
            }

            return Redirect($"https://localhost:7225/Admin/AdminPost/Details/{model.PostDetails.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId, int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // API'den yorum silme isteği gönder
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{uri}/api/UserPostApi/DeleteComment/DeleteComment/{commentId}");
                if (response.IsSuccessStatusCode)
                {
                    //return RedirectToAction("Details", new { id = postId });
                    return Redirect($"https://localhost:7225/Admin/AdminPost/Details/{postId}");
                }
            }

            //return RedirectToAction("Details", new { id = postId }); // Hata olsa bile detaya geri dön
            return Redirect($"https://localhost:7225/Admin/AdminPost/Details/{postId}"); // Hata olsa bile detaya geri dön
        }

    }
}
