using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    public class CreateCommentDTO
    {
        [Required(ErrorMessage = "Yorum içeriği zorunludur.")]
        public string Content { get; set; }
        public int PostId { get; set; }
        public int? AuthorId { get; set; } //yorum ekleyen admin yazar id'si (sadece admin yorum yazarsa kullanılacak)
        public string? UserId { get; set; } //yorum ekleyen standart user id'si (sadece user yorum yazarsa kullanılacak)
        //public string UserName { get; set; }
    }

}
