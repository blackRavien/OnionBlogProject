using OnionProject.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.DTOs
{
    public class RegisterAppUserDTO
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [MinLength(3, ErrorMessage = "Kullanıcı adı en az 3 karakterden oluşmalıdır.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; } // Kullanıcının sistemdeki adı

        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [Display(Name = "Email")]
        public string Email { get; set; } // Kullanıcının email adresi

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakterden oluşmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } // Kullanıcı şifresi

        [Required(ErrorMessage = "İsim zorunludur.")]
        [Display(Name = "İsim")]
        public string FirstName { get; set; } // Kullanıcının adı

        [Required(ErrorMessage = "Soyisim zorunludur.")]
        [Display(Name = "Soyisim")]
        public string LastName { get; set; } // Kullanıcının soyadı

        [Display(Name = "Kullanıcı Rolü")]
        public string Role { get; set; } = "Member"; // Kullanıcının rolü (varsayılan olarak "Member" atanır)

        public DateTime CreatedDate => DateTime.Now; // Kullanıcı oluşturulma tarihi otomatik atanır

        public Status Status => Status.Active; // Kullanıcı durumu varsayılan olarak aktif olur
    }
}
