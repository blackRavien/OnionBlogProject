using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using System.Text;

namespace OnionProject.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminPostController : Controller
    {
        private readonly string uri = "https://localhost:7296"; // API URL

        public async Task<IActionResult> Index()
        {
            List<GetPostsVm> posts = new List<GetPostsVm>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/AdminPost/Index"))
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
                var response = await httpClient.GetAsync($"{uri}/api/AdminGenre");
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
                var response = await httpClient.GetAsync($"{uri}/api/Author/Index");
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

                var response = await httpClient.PostAsync($"{uri}/api/AdminPost/Create", form);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Post başarıyla oluşturuldu!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Post oluşturulurken hata meydana geldi!";
            }
            return View(post);
        }


        public async Task<IActionResult> Details(int id)
        {
            PostDetailsVm postDetail = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminPost/Details/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    postDetail = JsonConvert.DeserializeObject<PostDetailsVm>(apiResponse);
                }
            }
            return postDetail == null ? NotFound() : View(postDetail);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            //var post = await GetPostById(id);
            //if (post == null) return NotFound();
            //return View(post);

            List<GenreVm> genres = new List<GenreVm>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminGenre");
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
                var response = await httpClient.GetAsync($"{uri}/api/Author/Index");
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

                if (post.UploadPath != null)
                {
                    var fileStreamContent = new StreamContent(post.UploadPath.OpenReadStream());
                    form.Add(fileStreamContent, "UploadPath", post.UploadPath.FileName);
                }

                var response = await httpClient.PutAsync($"{uri}/api/AdminPost/Update", form);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Post başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["Error"] = "Post güncellenirken hata meydana geldi!";
            }
            return View(post);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{uri}/api/AdminPost/Delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Post başarıyla silindi!";
                }
                else
                {
                    TempData["Error"] = "Post silinirken hata meydana geldi!";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<UpdatePostDTO> GetPostById(int id)
        {
            UpdatePostDTO post = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminPost/Details/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    post = JsonConvert.DeserializeObject<UpdatePostDTO>(apiResponse);
                }
            }
            return post;
        }
    }
}
