using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;

namespace OnionProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminPostApiController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IAuthorService _authorService;
        private readonly ICommentRepo _commentRepo;

        public AdminPostApiController(IPostService postService, IAuthorService authorService, ICommentRepo commentRepo)
        {
            _postService = postService;
            _authorService = authorService;
            _commentRepo = commentRepo;
        }

        // Tüm postları listelemek için
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var postList = await _postService.GetPosts();
            return Ok(postList); // PostVm listesi döner
        }

        // Detayları görmek için
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var postDetail = await _postService.GetDetail(id);
            if (postDetail == null)
            {
                return NotFound();
            }
            return Ok(postDetail); // PostDetailsVm döner
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorService.GetAuthors(); // Yazarları al
            return Ok(authors);
        }


        // Yeni bir post oluşturmak için
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePostDTO post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Post oluşturma
            await _postService.Create(post);
            return Ok();
        }

        


        // Postu güncellemek için
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePostDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _postService.Update(model);
                return Ok(); // Başarıyla güncellendi
            }
            catch (Exception ex)
            {
                // Hata detaylarını logla
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ID ile belirli bir postu almak için
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _postService.GetById(id);
            if (post == null)
            {
                return NotFound(new { message = "Post bulunamadı" });
            }

            return Ok(post); // UpdatePostDTO veya PostVm dönebilirsiniz
        }



        // Postu silmek için
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetById(id);
            if (post == null)
            {
                return NotFound(new { message = "Post bulunamadı" });
            }

            await _postService.Delete(id);
            return NoContent(); // 204 No Content, silme başarılı
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDTO createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = new Comment
            {
                Content = createCommentDto.Content,
                PostId = createCommentDto.PostId,
                AuthorId = createCommentDto.AuthorId,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepo.AddAsync(comment);

            return Ok();
        }


    }
}
