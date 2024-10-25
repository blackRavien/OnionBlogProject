using System;

namespace OnionProject.Application.Models.VMs
{
    // Gönderi bilgilerini içeren ViewModel sınıfı
    public class GetPostsVm
    {
        public int Id { get; set; } // Gönderinin benzersiz kimliği
        public string Title { get; set; } // Gönderinin başlığı
        public string AuthorFirstName { get; set; } // Yazarın adı
        public string AuthorLastName { get; set; } // Yazarın soyadı
        public string AuthorFullName => $"{AuthorFirstName} {AuthorLastName}"; // Yazarın tam adı
        public string GenreName { get; set; } // Gönderinin ait olduğu türün adı
        public DateTime CreatedDate { get; set; } // Gönderinin oluşturulma tarihi
        public string ImagePath { get; set; } // Gönderinin resim yolu
    }
}
/*
    Genel Özet:
GetPostsVm sınıfı, gönderi (post) bilgilerini görüntülemek için kullanılan bir ViewModel'dir. Sınıf, bir gönderinin detaylarını ve yazar bilgilerini içermektedir.

Alanlar:
Id: Gönderinin benzersiz kimliğini belirtir.
Title: Gönderinin başlığını tutar. Bu alan, gönderinin konusunu ifade eder.
AuthorFirstName: Yazarın adını tutar.
AuthorLastName: Yazarın soyadını tutar.
AuthorFullName: Yazarın tam adını birleştirir. Bu özellik, ad ve soyadın bir arada gösterilmesi için kullanılır.
GenreName: Gönderinin ait olduğu türün adını tutar. Bu, gönderinin hangi kategoriye ait olduğunu belirtir.
CreatedDate: Gönderinin oluşturulma tarihini tutar. Bu tarih, gönderinin ne zaman paylaşıldığını gösterir.
ImagePath: Gönderinin görüntülemek için kullanılan resmin yolu.
 */