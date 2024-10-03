using Microsoft.AspNetCore.Http;
using OnionProject.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.DTOs
{
    public class UpdatePostDTO
    {
        // Gönderinin mevcut id'sini almak için gerekli. Bu yüzden zorunlu olarak işaretledik.
        [Required]
        public int Id { get; set; }

        // Gönderinin başlığını güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "Başlık girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Başlığın en az 3 karakterli olması gereklidir.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        // Gönderinin içeriğini güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "İçeriğin girilmesi zorunludur.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; }

        // Gönderinin yeni resmini güncellemek için dosya yükleme özelliği.
        public IFormFile UploadPath { get; set; }

        // Gönderinin mevcut resim yolu (opsiyonel).
        public string? ImagePath { get; set; }

        // Gönderinin durumunu (aktif, pasif vs.) güncellemek için gerekli alan.
        public Status Status { get; set; }

        // Gönderinin güncelleme tarihini tutan alan. Güncelleme işleminde tarih otomatik olarak atanır.
        public DateTime UpdatedDate {  get; set; }

        // Gönderinin yazarını güncellemek için gerekli olan yazar id'si.
        [Required]
        public int AuthorId { get; set; }

        // Gönderinin türünü güncellemek için gerekli olan tür id'si.
        [Required]
        public int GenreId { get; set; }

        public DateTime CreatedDate {  get; set; }
    }
}
