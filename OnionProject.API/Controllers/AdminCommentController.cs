using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using OnionProject.Domain.Enum;

namespace OnionProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminCommentController : ControllerBase
    {
        private readonly ICommentRepo _commentRepo;

        public AdminCommentController(ICommentRepo commentRepo)
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

        //// Yorum güncelleme
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateComment(int id, UpdateCommentDTO updateCommentDto)
        //{
        //    if (id != updateCommentDto.Id)
        //    {
        //        return BadRequest(); // ID'ler eşleşmiyorsa 400 döner
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState); // Model geçersizse 400 döner
        //    }

        //    // UpdateCommentDTO'yu Comment nesnesine dönüştür
        //    var comment = new Comment
        //    {
        //        Id = updateCommentDto.Id,
        //        Content = updateCommentDto.Content,
        //        UpdatedDate = DateTime.UtcNow // Güncellenme tarihi ekleyin
        //    };

        //    await _commentRepo.UpdateAsync(comment); // API'ye güncelleme isteği
        //    return Ok(); // Başarılı ise 200 döner
        //}



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
