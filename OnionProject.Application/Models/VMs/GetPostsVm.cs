using System;

namespace OnionProject.Application.Models.VMs
{
    public class GetPostsVm
    {
        public int Id { get; set; } // Gönderinin ID'si
        public string Title { get; set; } // Gönderinin başlığı
        //public string ImagePath { get; set; } // Gönderinin resmi
        public string AuthorFullName { get; set; } // Yazarın tam adı
        public string GenreName { get; set; } // Türün adı
        public DateTime CreatedDate { get; set; } // Gönderinin oluşturulma tarihi

        public string ImagePath { get; set; }

        //// ImagePath property'sini güncelleyin
        //public string ImagePath
        //{
        //    get
        //    {
        //        // Tam URL'yi oluşturun
        //        return $"https://localhost:7296{_imagePath}";
        //    }
        //}

        // Örnek: _imagePath, API'den gelen içeriği temsil eder. 
        // Bu değeri API'den almak için başka bir property tanımlamanız gerekebilir.
        //private string _imagePath; // API'den gelen içeriği temsil eder

        //public void SetImagePath(string imagePath)
        //{
        //    _imagePath = imagePath;
        //}
    }
}
