using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.VMs
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string Username { get; set; } // Kullanıcı adı

        [Required(ErrorMessage = "Şifre gereklidir.")]
        public string Password { get; set; } // Şifre
    }
}
