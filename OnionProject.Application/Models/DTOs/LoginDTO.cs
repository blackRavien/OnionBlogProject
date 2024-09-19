using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [Display(Name = "Email Adresi")]
        public string Email { get; set; } // Kullanıcının giriş yaparken kullanacağı email adresi

        [Required(ErrorMessage = "Şifre girilmesi zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)] // Şifrenin gizli tutulmasını sağlıyor.
        [Display(Name = "Şifre")]
        public string Password { get; set; } // Kullanıcının giriş yaparken kullanacağı şifre
    }
}
