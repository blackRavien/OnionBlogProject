using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    public class CreateCommentDTO
    {
        public string Content { get; set; }
        public int PostId { get; set; }
        public int AuthorId { get; set; } //yorum ekleyen yazar id'si
        public string UserId { get; set; } //yorum ekleyen standart user id'si
    }

}
