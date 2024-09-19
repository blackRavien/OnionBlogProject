using System;

namespace OnionProject.Application.Models.VMs
{
    public class GetPostsVm
    {
        public int Id { get; set; } // Gönderinin ID'si
        public string Title { get; set; } // Gönderinin başlığı
        public string ImagePath { get; set; } // Gönderinin resmi
        public string AuthorFullName { get; set; } // Yazarın tam adı
        public string GenreName { get; set; } // Türün adı
        public DateTime CreatedDate { get; set; } // Gönderinin oluşturulma tarihi
    }
}
