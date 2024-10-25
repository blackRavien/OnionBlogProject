using Microsoft.AspNetCore.Http; // HTTP ile ilgili temel bileşenler için gerekli namespace
using OnionProject.Application.Extensions; // Projeye özel uzantılar
using OnionProject.Domain.Enum; // Projede kullanılan enum yapılarının bulunduğu namespace
using System; // Temel C# sınıf kütüphanesi
using System.ComponentModel.DataAnnotations; // Doğrulama ve display attribute'lerini kullanmak için gerekli namespace

namespace OnionProject.Application.Models.DTOs
{
    // Yeni bir gönderi (post) oluşturmak için kullanılan DTO sınıfı
    public class CreatePostDTO
    {
        // Post başlığı zorunlu bir alan olup, en az 5 karakter olmalıdır.
        [Required(ErrorMessage = "Başlık zorunludur.")] // Zorunlu alan hatası mesajı
        [MinLength(5, ErrorMessage = "Başlık en az 5 karakter olmalıdır.")] // Minimum karakter sayısı uyarısı
        [Display(Name = "Başlık")] // Formda bu alanın nasıl görüntüleneceğini belirleyen Display attribute
        public string Title { get; set; }  // Post başlığı

        // Post içeriği zorunlu bir alan olup, en az 20 karakter olmalıdır.
        [Required(ErrorMessage = "İçerik zorunludur.")] // Zorunlu alan hatası mesajı
        [MinLength(20, ErrorMessage = "İçerik en az 20 karakter olmalıdır.")] // Minimum karakter sayısı uyarısı
        [Display(Name = "İçerik")] // Formda bu alanın nasıl görüntüleneceğini belirleyen Display attribute
        public string Content { get; set; }  // Post içeriği

        // Resim yüklemek zorunlu bir alan, ayrıca PictureFileExtension attribute'u ile format kontrolü yapılır.
        [Required(ErrorMessage = "Resim yüklemek zorunludur!")] // Zorunlu alan hatası mesajı
        [PictureFileExtension(ErrorMessage = "Sadece resim formatı kabul edilir.")] // Resim formatı kontrolü
        [Display(Name = "Resim Yolu")] // Formda bu alanın nasıl görüntüleneceğini belirleyen Display attribute
        public IFormFile UploadPath { get; set; }  // Yüklenecek resim dosyası

        // Resmin kaydedildiği yol, opsiyonel bir alandır.
        public string? ImagePath { get; set; }  // Resmin kaydedildiği yol (optional)

        // Post'un yazarının ID'si (foreign key).
        [Display(Name = "Yazar")] // Formda bu alanın nasıl görüntüleneceğini belirleyen Display attribute
        public int AuthorId { get; set; }  // Post'un yazarının ID'si (foreign key)

        // Post'un türü veya kategorisi (foreign key).
        [Display(Name = "Kategori")]
        [Required(ErrorMessage = "Kategori Türü girmek zorunludur.")] // Zorunlu alan hatası mesajı
        public int GenreId { get; set; }  // Post'un türü veya kategorisi (foreign key)

        // Oluşturulma tarihi otomatik olarak güncel tarih olacak.
        public DateTime CreatedDate => DateTime.Now;  // Oluşturulma tarihi, varsayılan olarak şu anki zaman

        // Varsayılan durum aktif.
        public Status Status => Status.Active;  // Varsayılan durum aktif
    }
}

/*
    Genel Özet:
    CreatePostDTO sınıfı, yeni bir gönderi (post) oluşturmak için kullanılan bir DTO'dur. Kullanıcıdan alınacak bilgiler arasında başlık, içerik, ve resim dosyası gibi zorunlu alanlar bulunur. Ayrıca, gönderinin oluşturulma tarihi ve durumu da otomatik olarak atanır. DTO, gönderi oluşturma işlemi sırasında sunucuya gönderilecek verilerin taşıyıcısı olarak işlev görür ve gerekli doğrulamaları içerir.
    Title ve Content alanları: Zorunlu alanlar olarak tanımlandı, belirli bir minimum karakter uzunluğu şartı ile.
    IFormFile UploadPath: Resim dosyası için kullanıcının yükleyeceği dosyayı alır. Aynı zamanda daha önce yazdığın PictureFileExtension ile resim formatı kontrol ediliyor.
    ImagePath: Resim dosyasının kaydedildiği yolu tutar. Bu opsiyonel bir alandır.
    AuthorId ve GenreId: İlişkili yazar ve tür (genre) için foreign key alanlarıdır.
    CreatedDate: Post’un oluşturulma tarihi otomatik olarak atanır.
    Status: Post'un durumu varsayılan olarak aktif (Status.Active) olarak ayarlanmıştır.
    Bu DTO, Post ile ilgili tüm bilgileri alıp veri tabanına kaydetmeden önce kullanmak için tasarlandı.
 */
