using Microsoft.AspNetCore.Http;
using OnionProject.Application.Extensions;
using OnionProject.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.DTOs
{
    public class CreatePostDTO
    {
        [Required(ErrorMessage = "Başlık zorunludur.")]
        [MinLength(5, ErrorMessage = "Başlık en az 5 karakter olmalıdır.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; }  // Post başlığı

        [Required(ErrorMessage = "İçerik zorunludur.")]
        [MinLength(20, ErrorMessage = "İçerik en az 20 karakter olmalıdır.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; }  // Post içeriği

        [PictureFileExtension(ErrorMessage = "Sadece resim formatı kabul edilir.")]
        [Display(Name = "Resim Yolu")]
        public IFormFile UploadPath { get; set; }  // Yüklenecek resim dosyası

        public string? ImagePath { get; set; }  // Resmin kaydedildiği yol (optional)

        [Display(Name = "Yazar")]
        public int AuthorId { get; set; }  // Post'un yazarının ID'si (foreign key)

        [Display(Name = "Kategori")]
        public int GenreId { get; set; }  // Post'un türü veya kategorisi (foreign key)

        public DateTime CreatedDate => DateTime.Now;  // Oluşturulma tarihi, varsayılan olarak şu anki zaman

        public Status Status => Status.Active;  // Varsayılan durum aktif
    }
}


/*
        Açıklamalar:
        Title ve Content alanları: Zorunlu alanlar olarak tanımlandı, belirli bir minimum karakter uzunluğu şartı ile.
        IFormFile UploadPath: Resim dosyası için kullanıcının yükleyeceği dosyayı alır. Aynı zamanda daha önce yazdığın PictureFileExtension ile resim formatı kontrol ediliyor.
        ImagePath: Resim dosyasının kaydedildiği yolu tutar. Bu opsiyonel bir alandır.
        AuthorId ve GenreId: İlişkili yazar ve tür (genre) için foreign key alanlarıdır.
        CreatedDate: Post’un oluşturulma tarihi otomatik olarak atanır.
        Status: Post'un durumu varsayılan olarak aktif (Status.Active) olarak ayarlanmıştır.
        Bu DTO Post ile ilgili tüm bilgileri alıp veri tabanına kaydetmeden önce kullanmak için tasarlandı.
 */