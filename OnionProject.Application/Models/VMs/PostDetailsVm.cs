using System;

namespace OnionProject.Application.Models.VMs
{
    public class PostDetailsVm
    {
        public int Id { get; set; } // Gönderinin ID'si
        public string? Title { get; set; } // Gönderinin başlığı
        public string? Content { get; set; } // Gönderinin içeriği


        public string? ImagePath { get; set; } // Gönderinin resmi


        public string? GenreName { get; set; } // Türün adı
        public string? AuthorFirstName { get; set; } // Yazarın adı
        public string? AuthorLastName { get; set; } // Yazarın soyadı
        public string AuthorFullName => $"{AuthorFirstName} {AuthorLastName}"; // Yazarın tam adı

        public List<CommentVm>? Comments { get; set; } // Yorumlar

        public DateTime CreatedDate { get; set; } // Gönderinin oluşturulma tarihi
        
        public AuthorDetailVm AuthorDetailVm { get; set; }

    }
}
