using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using System.Security.Claims;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using OnionProject.Infrastructure.Context;

namespace OnionProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminPostApiController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IAuthorService _authorService;
        private readonly ICommentService _commentService;
        private readonly ICommentRepo _commentRepo;
        private readonly AppDbContext _appDbContext;

        public AdminPostApiController(IPostService postService, IAuthorService authorService, ICommentRepo commentRepo, ICommentService commentService, AppDbContext appDbContext)
        {
            _postService = postService;
            _authorService = authorService;
            _commentRepo = commentRepo;
            _commentService = commentService;
            _appDbContext = appDbContext;
        }


        // Tüm postları listelemek için
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var postList = await _postService.GetPosts();

            // Her post için tam URL'yi oluştur
            var postVms = postList.Select(post => new
            {
                post.Id,
                post.Title,
                post.Content,
                post.GenreName,
                post.AuthorFirstName,
                post.AuthorLastName,
                post.CreatedDate,
                ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}" // Tam URL'yi oluştur
            }).ToList();

            return Ok(postVms); // PostVm listesi döner
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
                // ImagePath'i tam URL olarak ayarla
                ImagePath = $"https://localhost:7296/{postDetail.ImagePath.TrimStart('/')}" // Tam URL'yi oluştur
            };

            return Ok(postDetailsVm); // PostDetailsVm döner
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







        //Postu güncellemek için
        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdatePostDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var post = await _postService.GetById(model.Id);
                if (post == null)
                {
                    return NotFound(new { message = "Post bulunamadı" });
                }

                // Diğer alanları güncelle
                post.Title = model.Title;
                post.Content = model.Content;
                post.AuthorId = model.AuthorId;
                post.GenreId = model.GenreId;
                post.UpdatedDate = DateTime.Now;

                // Yeni bir resim yüklenmemişse mevcut resmi koru
                if (model.UploadPath != null && model.UploadPath.Length > 0)
                {
                    using var image = Image.Load(model.UploadPath.OpenReadStream());
                    image.Mutate(x => x.Resize(600, 500));
                    Guid guid = Guid.NewGuid();
                    image.Save($"wwwroot/images/{guid}.jpg");
                    post.ImagePath = $"/images/{guid}.jpg"; // Yeni resmi güncelle
                }

                await _postService.Update(post);
                return Ok(); // Başarıyla güncellendi
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





        //[HttpPut]
        // public async Task<IActionResult> Update([FromForm] UpdatePostDTO model)
        // {
        //     try
        //     {
        //         if (!ModelState.IsValid)
        //         {
        //             return BadRequest(ModelState);
        //         }



        //         await _postService.Update(model);
        //         return Ok(); // Başarıyla güncellendi
        //     }
        //     catch (Exception ex)
        //     {
        //         // Hata detaylarını logla
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }


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


        [HttpGet("api/contact")]
        public async Task<IActionResult> GetContactMessages()
        {
            var contactMessages = await _appDbContext.ContactMessages.ToListAsync();
            return Ok(contactMessages);
        }

        [HttpDelete("api/contact/{id}")]
        public async Task<IActionResult> DeleteContactMessage(int id)
        {
            var message = await _appDbContext.ContactMessages.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _appDbContext.ContactMessages.Remove(message);
            await _appDbContext.SaveChangesAsync();
            return NoContent(); // 204 No Content
        }


    }
}
