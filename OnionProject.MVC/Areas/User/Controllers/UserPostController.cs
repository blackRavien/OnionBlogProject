using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using System.Net.Http;
using System.Security.Claims;

namespace OnionProject.MVC.Controllers
{
    [Area("User")] // Eğer Area kullanıyorsanız
    [Route("UserPost")]
    public class UserPostController : Controller
    {
        private readonly string uri = "https://localhost:7296"; // API URL
        private readonly ICommentService _commentService;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IAppUserService _appUserService;

        public UserPostController(IMapper mapper, IPostService postService, ICommentService commentService)
        {
            _mapper = mapper;
            _postService = postService;
            _commentService = commentService;
        }

        // GET: UserPost
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name; // Kullanıcı adını al
            ViewBag.Username = username; // ViewBag ile gönder

            List<GetPostsVm> posts = new List<GetPostsVm>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/UserPost/Index"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    posts = JsonConvert.DeserializeObject<List<GetPostsVm>>(apiResponse);
                }
            }
            return View(posts);
        }

        // GET: UserPost/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var postDetailsDTO = await _postService.GetById(id); // Bu kısım UpdatePostDTO olabilir
            var postDetailsVm = _mapper.Map<PostDetailsVm>(postDetailsDTO); // Mapping işlemi

            var comments = await _commentService.GetCommentsByPostIdAsync(id);

            var model = new PostDetailsWithCommentVm
            {
                PostDetails = postDetailsVm, // Dönüştürülmüş VM
                Comments = comments,
                NewComment = new CreateCommentDTO()
            };

            return View(model);
        }

        //bu yapıyı kaybetma
        // CreateComment metodu için özel route ekliyoruz
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
                    var response = await httpClient.PostAsync($"{uri}/api/UserPost/CreateComment", content);

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



        //[HttpPost("CreateComment")]
        //public async Task<IActionResult> CreateComment(PostDetailsWithCommentVm model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcının ID'sini alın.
        //        var username = User.FindFirstValue(ClaimTypes.Name); // Kullanıcının adını alın.

        //        var createCommentDto = new CreateCommentDTO
        //        {
        //            Content = model.NewComment.Content,
        //            PostId = model.PostDetails.Id,
        //            UserId = userId
        //        };

        //        // API üzerinden yorum oluştur
        //        using (var httpClient = new HttpClient())
        //        {
        //            var json = JsonConvert.SerializeObject(createCommentDto);
        //            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        //            var response = await httpClient.PostAsync($"{uri}/api/UserPost/CreateComment", content);

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                ModelState.AddModelError("", "Yorum eklenirken bir hata oluştu.");
        //                return View("Details", model);
        //            }
        //        }

        //        // Kullanıcı adı ve yorum içeriği ile birlikte kullanıcı arayüzüne yönlendirin
        //        // Burada, yönlendirme yerine direkt olarak yorumları içeren bir model oluşturabilirsiniz.
        //        // Detay sayfasına yönlendirirken kullanıcı adını modelde taşıyabilirsiniz.

        //        // Örnek: Kullanıcı adını modelinize ekleyin (eğer modeliniz buna izin veriyorsa)
        //        model.NewComment.UserName = username; // Kullanıcı adını DTO'ya ekleyin (CreateCommentDTO'ya UserName eklemeniz gerekebilir)

        //        return RedirectToAction("Details", new { id = model.PostDetails.Id });
        //    }

        //    return View("Details", model);
        //}


        // DeleteComment metodu
        //https://localhost:7296/api/UserPost/DeleteComment/DeleteComment/7
        [HttpPost("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int commentId, int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // API'den yorum silme isteği gönder
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{uri}/api/UserPost/DeleteComment/DeleteComment/{commentId}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Details", new { id = postId });
                }
            }

            return RedirectToAction("Details", new { id = postId }); // Hata olsa bile detaya geri dön
        }

        //// DeleteComment metodu için özel route ekliyoruz
        //[HttpPost("DeleteComment")]
        //public async Task<IActionResult> DeleteComment(int commentId, int postId)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //    var comment = (await _commentService.GetCommentsByPostIdAsync(postId))
        //        .FirstOrDefault(c => c.Id == commentId); // commentId ile eşleşen yorumu buluyoruz.

        //    if (comment != null && comment.UserId == userId)
        //    {
        //        await _commentService.DeleteCommentAsync(commentId);
        //    }


        //    return RedirectToAction("Details", new { id = postId });
        //}


    }
}

//public async Task<IActionResult> Details(int id)
        //{
        //    PostDetailsVm postDetail = null;
        //    using (var httpClient = new HttpClient())
        //    {
        //        var response = await httpClient.GetAsync($"{uri}/api/UserPost/Details/{id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            postDetail = JsonConvert.DeserializeObject<PostDetailsVm>(apiResponse);
        //        }
        //    }

        //    if (postDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(postDetail);
        //}