using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using OnionProject.Domain.Enum;

namespace OnionProject.API.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminCommentApiController : ControllerBase
    {
        private readonly ICommentRepo _commentRepo;

        public AdminCommentApiController(ICommentRepo commentRepo)
        {
            _commentRepo = commentRepo;
        }

        // Tüm yorumları listeleme
        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepo.GetAllCommentsAsync();
            return Ok(comments);
        }

        // Yorum ekleme
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateCommentDTO createCommentDto)
        {
            // Modelin geçerliliğini kontrol et
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // DTO'dan Comment nesnesine dönüştürme
            var comment = new Comment
            {
                Content = createCommentDto.Content,
                PostId = createCommentDto.PostId,
                UserId = createCommentDto.UserId,
                CreatedAt = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                Status = Status.Active // Varsayılan durum
            };

            // Yorum ekleme işlemi
            await _commentRepo.AddAsync(comment);
            return Ok();
        }


        // Yorum silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _commentRepo.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound(); // Yorum bulunamazsa 404 döner
            }

            await _commentRepo.DeleteAsync(id);
            return Ok();
        }
    }
}
