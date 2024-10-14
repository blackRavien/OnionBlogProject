using Microsoft.AspNetCore.Http;  // IFormFile sınıfını kullanmak için gerekli
using OnionProject.Domain.Enum;  // Status enum'ını kullanmak için gerekli
using System.ComponentModel.DataAnnotations.Schema;  // NotMapped attribute'ü için gerekli

namespace OnionProject.Domain.Entities
{
    // Author sınıfı bir yazarın bilgilerini temsil eder ve IBaseEntity arayüzünü implemente eder.
    public class Author : IBaseEntity
    {
        // Yazarın benzersiz kimlik numarası
        public int Id { get; set; }

        // Yazarın adı
        public string FirstName { get; set; }

        // Yazarın soyadı
        public string LastName { get; set; }

        // Yazarın profil resminin dosya yolu
        public string ImagePath { get; set; }

        // Yeni eklemeler
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Biography { get; set; }

        // Dinamik post sayısı
        public int NumberOfPosts => Posts?.Count ?? 0;

        // Veritabanına kaydedilmeyen, yazarın profil fotoğrafı yüklemek için kullanılan alan
        [NotMapped]
        public IFormFile UploadPath { get; set; }

        // Yazarın oluşturulma tarihi
        public DateTime CreatedDate { get; set; }

        // Yazarın güncellenme tarihi (opsiyonel, null olabilir)
        public DateTime? UpdatedDate { get; set; }

        // Yazarın silinme tarihi (opsiyonel, null olabilir)
        public DateTime? DeletedDate { get; set; }

        // Yazarın durumu (Aktif, Pasif, Silindi vb.) için Status enum'ı
        public Status Status { get; set; }

        // Navigasyon özelliği (Yazarın yazdığı gönderiler). Bir yazarın birden fazla gönderisi olabilir.
        public List<Post> Posts { get; set; } = new List<Post>();  // Varsayılan olarak boş bir liste
    }
}

/*
 Id: Yazarın benzersiz kimlik numarasıdır. Bu alan, yazarların veritabanında benzersiz bir şekilde tanımlanmasını sağlar.

FirstName ve LastName: Yazarın adı ve soyadı. Bu alanlar yazarın kişisel bilgilerini tutar.

ImagePath: Yazarın profil resminin dosya yolunu tutar. Bu yol, yazarın profil resmi sunucuda saklandığında nereye kaydedileceğini belirtir.

UploadPath: IFormFile tipinde olan bu özellik, yazarın profil resmi yüklemek için kullanılır. Ancak, [NotMapped] attribute'ü sayesinde bu özellik veritabanına kaydedilmez, sadece dosya yükleme işlemi için geçici olarak kullanılır.

IBaseEntity Özellikleri:
CreatedDate: Yazarın ne zaman oluşturulduğunu belirten tarih.

UpdatedDate: Yazarın güncellenme tarihi (opsiyoneldir, yani null olabilir).

DeletedDate: Yazarın silinme tarihi (opsiyoneldir, yani null olabilir).

Status: Yazarın durumunu belirler. Bu durumlar, Status enum'ı tarafından yönetilir. Örneğin, yazar Aktif, Pasif ya da Silindi olabilir.

Navigasyon Özelliği:
Posts: Yazar ile gönderiler arasındaki ilişkiyi ifade eden bir navigasyon özelliğidir. Bir yazarın birden fazla gönderisi olabilir, bu yüzden bu özellik bir List<Post> olarak tanımlanmıştır. Yani, her yazarın birden fazla yazı (Post) ile ilişkisi vardır.
 */