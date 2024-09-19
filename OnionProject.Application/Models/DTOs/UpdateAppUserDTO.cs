using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.DTOs
{
    public class UpdateAppUserDTO
    {
        // Kullanıcının mevcut id'sini almak için gerekli. Bu yüzden zorunlu olarak işaretledik.
        [Required]
        public string Id { get; set; }

        // Kullanıcının ismini güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "İsmin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Adınızın en az 3 harfli olması gereklidir.")]
        [Display(Name = "İsim")]
        public string FirstName { get; set; }

        // Kullanıcının soyadını güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "Soyismin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Soyadınızın en az 3 harfli olması gereklidir.")]
        [Display(Name = "Soyisim")]
        public string LastName { get; set; }

        // Kullanıcı profil fotoğrafını güncellemek için dosya yükleme özelliği.
        public IFormFile UploadPath { get; set; }

        // Kullanıcının mevcut profil fotoğrafı (opsiyonel).
        public string? ImagePath { get; set; }

        // Kullanıcının güncelleme tarihini tutan alan. Güncelleme işleminde tarih otomatik olarak atanır.
        public DateTime UpdatedDate => DateTime.Now;
    }
}
