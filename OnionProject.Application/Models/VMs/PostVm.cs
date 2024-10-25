using System;

namespace OnionProject.Application.Models.VMs
{
    // Gönderi bilgilerini tutan ViewModel
    public class PostVm
    {
        public int Id { get; set; } // Gönderinin benzersiz kimliği
        public string Title { get; set; } // Gönderinin başlığı
        public string Content { get; set; } // Gönderinin içeriği
        public string ImagePath { get; set; } // Gönderinin görseli için yol
        public string GenreName { get; set; } // Gönderinin türü
        public string AuthorFirstName { get; set; } // Yazarın adı
        public string AuthorLastName { get; set; } // Yazarın soyadı

        // Yazarın tam adı
        public string AuthorFullName => $"{AuthorFirstName} {AuthorLastName}";

        // Gönderinin oluşturulma tarihi
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Gönderinin tür kimliği
        public int GenreId { get; set; }

        // Gönderinin yazar kimliği
        public int AuthorId { get; set; }
    }
}

/*
    Genel Özet:
PostVm sınıfı, bir gönderinin temel bilgilerini tutmak için kullanılan bir ViewModel'dir. Gönderi detaylarının görüntülenmesi için gereken alanları içerir.

Alanlar:
Id: Gönderinin benzersiz kimliğini temsil eder.

Title: Gönderinin başlığını tutar.

Content: Gönderinin içeriğini tutar.

ImagePath: Gönderinin görselinin dosya yolunu belirtir.

GenreName: Gönderinin ait olduğu türün adını tutar.

AuthorFirstName: Yazarın adını belirtir.

AuthorLastName: Yazarın soyadını belirtir.

AuthorFullName: Yazarın tam adını birleştirir. Bu alan, otomatik olarak FirstName ve LastName birleştirilerek oluşturulmuştur.

CreatedDate: Gönderinin oluşturulma tarihini tutar. Varsayılan olarak DateTime.Now ile atanır.

GenreId: Gönderinin ait olduğu türün kimliğini belirtir.

AuthorId: Gönderinin yazarının kimliğini belirtir.

Kullanım:
Bu ViewModel, gönderi listelerinde veya gönderi detay sayfalarında gönderi bilgilerini görüntülemek için kullanılır. Kullanıcılar gönderi başlıklarını, içeriklerini, türlerini ve yazar bilgilerini bu alanlar üzerinden görebilir.
 */