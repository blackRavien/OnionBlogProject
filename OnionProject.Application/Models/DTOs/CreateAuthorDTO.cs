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
        [Required(ErrorMessage ="İsmin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage ="Adınızın en az 3 harfli olması gereklidir.")]
        [Display(Name ="İsim")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyismin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Soyadınızın en az 3 harfli olması gereklidir.")]
        [Display(Name = "Soyisim")]
        public string LastName { get; set; }

        [PictureFileExtension] //Kendi yazdığımız Extension sınıfını kullanarak doğru formatta resim almayı kontrol edebiliriz.
        public IFormFile UploadPath { get; set; }
        public string? ImagePath { get; set; }
        public DateTime CreatedDate => DateTime.Now;
        public Status Status => Status.Active;



    }
}
