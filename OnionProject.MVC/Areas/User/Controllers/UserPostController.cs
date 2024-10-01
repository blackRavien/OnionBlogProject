using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using System.Net.Http;

namespace OnionProject.MVC.Controllers
{
    [Area("User")] // Eğer Area kullanıyorsanız
    [Route("UserPost")]
    public class UserPostController : Controller
    {
        private readonly string uri = "https://localhost:7296"; // API URL

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
            PostDetailsVm postDetail = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/UserPost/Details/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    postDetail = JsonConvert.DeserializeObject<PostDetailsVm>(apiResponse);
                }
            }

            if (postDetail == null)
            {
                return NotFound();
            }

            return View(postDetail);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDTO createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Geçersiz yorum!";
                return RedirectToAction(nameof(Details), new { id = createCommentDto.PostId });
            }

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync($"{uri}/api/UserComment/CreateComment", createCommentDto);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Yorum başarıyla eklendi!";
                }
                else
                {
                    TempData["Error"] = "Yorum eklenirken bir hata oluştu.";
                }
            }

            return RedirectToAction(nameof(Details), new { id = createCommentDto.PostId });
        }
    }
}
