using Microsoft.AspNetCore.Http;
using OnionProject.Application.Extensions;
using OnionProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    public class CreateAuthorDTO
    {
        [Required(ErrorMessage = "İsmin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Adınızın en az 3 harfli olması gereklidir.")]
        [Display(Name = "İsim")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyismin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Soyadınızın en az 3 harfli olması gereklidir.")]
        [Display(Name = "Soyisim")]
        public string LastName { get; set; }

        // PictureFileExtension Attribute, dosya formatını kontrol eder.
        [PictureFileExtension(ErrorMessage = "Lütfen geçerli bir resim formatı yükleyin.")]
        public IFormFile Image { get; set; } // Resim dosyasını tutan alan

        public string? ImagePath { get; set; } // Resmin kaydedildiği dosya yolu

        // Eğer CreatedDate ve Status backend'de atanıyorsa DTO'dan kaldırabilirsiniz.
        // Aksi takdirde bu alanları bırakabilirsiniz.

        public DateTime CreatedDate => DateTime.Now; // Oluşturulma tarihi
        public Status Status => Status.Active; // Varsayılan olarak aktif

        // Yeni eklemeler
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [MaxLength(1000)]
        public string Biography { get; set; }
    }

}
