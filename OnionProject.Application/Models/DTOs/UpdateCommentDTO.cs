using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    public class UpdateCommentDTO
    {
        [Required] // Yorum ID'si zorunlu
        public int Id { get; set; }

        [Required(ErrorMessage = "Yorum içeriği boş olamaz.")]
        [StringLength(500, ErrorMessage = "Yorum içeriği en fazla 500 karakter olmalıdır.")]
        public string Content { get; set; }

        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow; // Güncelleme tarihi
    }
}