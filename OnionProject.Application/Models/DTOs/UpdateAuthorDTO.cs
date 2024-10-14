using Microsoft.AspNetCore.Http;
using OnionProject.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.DTOs
{
    public class UpdateAuthorDTO
    {
        // Yazarın mevcut id'sini almak için gerekli. Bu yüzden zorunlu olarak işaretledik.
        [Required]
        public int Id { get; set; }

        // Yazarın ismini güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "İsmin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Adınızın en az 3 harfli olması gereklidir.")]
        [Display(Name = "İsim")]
        public string FirstName { get; set; }

        // Yazarın soyadını güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "Soyismin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Soyadınızın en az 3 harfli olması gereklidir.")]
        [Display(Name = "Soyisim")]
        public string LastName { get; set; }

        // Yazarın yeni profil fotoğrafını güncellemek için dosya yükleme özelliği.
        public IFormFile? Image { get; set; }

        // Yazarın mevcut profil fotoğrafı (opsiyonel).
        public string? ImagePath { get; set; }

        // Yazarın durumunu (aktif, pasif vs.) güncellemek için gerekli alan.
        public Status Status { get; set; }

        // Yazarın güncelleme tarihini tutan alan. Güncelleme işleminde tarih otomatik olarak atanır.
        public DateTime UpdatedDate => DateTime.Now;


        // Yeni alanlar
        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string PhoneNumber { get; set; }

        public string Biography { get; set; }
    }
}
