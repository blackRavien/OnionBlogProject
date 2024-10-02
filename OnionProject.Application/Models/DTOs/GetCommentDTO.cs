using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    public class GetCommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public string? UserId { get; set; } //yorum ekleyen standart user id'si (sadece user yorum yazarsa kullanılacak)
        public int? AuthorId { get; set; }   //yorum ekleyen admin yazar id'si (sadece admin yorum yazarsa kullanılacak)
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } // Kullanıcı adını ekliyoruz.

    }

}
