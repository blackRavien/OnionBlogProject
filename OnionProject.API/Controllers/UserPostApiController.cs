using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnionProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserPostApiController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        public UserPostApiController(IPostService postService, ICommentService commentService)
        {
            _postService = postService;
            _commentService = commentService;
        }

        // Tüm gönderileri almak için
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetPosts();

            var postVms = posts.Select(post => new
            {
                post.Id,
                post.Title,
                post.Content,
                post.GenreName,
                post.AuthorFirstName,
                post.AuthorLastName,
                post.CreatedDate,
                ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}"

            }).ToList();


            return Ok(postVms);
        }

        // Belirli bir gönderinin detaylarını almak için
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var postDetail = await _postService.GetDetail(id);
            if (postDetail == null)
            {
                return NotFound();
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
                Comments = postDetail.Comments,
                AuthorDetailVm = postDetail.AuthorDetailVm,
                // ImagePath'i tam URL olarak ayarla
                ImagePath = $"{postDetail.ImagePath.TrimStart('/')}" // Tam URL'yi oluştur
            };

            return Ok(postDetailsVm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _commentService.AddCommentAsync(createCommentDto);
            return Ok("Yorum başarıyla eklendi!");
        }


        [HttpDelete("DeleteComment/{commentId}")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _commentService.GetCommentsByPostIdAsync(commentId); // Yorumu bul

            if (comment == null)
            {
                return NotFound(new { message = "Yorum bulunamadı." });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Mevcut kullanıcının ID'sini al

            // Yorumu sil
            await _commentService.DeleteCommentAsync(commentId);

            return Ok(new { message = "Yorum başarıyla silindi." });
        }


    }
}
