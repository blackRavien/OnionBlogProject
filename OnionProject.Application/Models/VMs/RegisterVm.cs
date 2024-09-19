using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.VMs
{
    public class RegisterVm
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string Username { get; set; } // Kullanıcı adı

        [Required(ErrorMessage = "Şifre gereklidir.")]
        public string Password { get; set; } // Şifre

        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        public string Email { get; set; } // E-posta

        public string FirstName { get; set; } // Ad
        public string LastName { get; set; } // Soyad
    }
}
