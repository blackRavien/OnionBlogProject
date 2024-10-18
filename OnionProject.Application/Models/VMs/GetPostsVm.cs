using System;

namespace OnionProject.Application.Models.VMs
{
    public class GetPostsVm
    {
        public int Id { get; set; } // Gönderinin ID'si
        public string Title { get; set; } // Gönderinin başlığı
        public string AuthorFirstName { get; set; } // Yazarın adı
        public string AuthorLastName { get; set; }  // Yazarın soyadı
        public string AuthorFullName => $"{AuthorFirstName} {AuthorLastName}"; // Ad ve soyadı birleştir
        public string GenreName { get; set; } // Türün adı
        public DateTime CreatedDate { get; set; } // Gönderinin oluşturulma tarihi
        public string ImagePath { get; set; }

    }
}
