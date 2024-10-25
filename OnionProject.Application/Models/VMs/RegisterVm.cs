using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.VMs
{
    // Kullanıcı kayıt bilgilerini tutan ViewModel
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

/*
    Genel Özet:
RegisterVm sınıfı, kullanıcı kayıt işlemleri için gerekli bilgileri tutan bir ViewModel'dir. Kullanıcının kimlik bilgilerini toplamak amacıyla oluşturulmuştur.

Alanlar:
Username: Kullanıcının platformdaki kullanıcı adını belirtir. Bu alan, gerekli olduğu için Required niteliği ile işaretlenmiştir.

Password: Kullanıcının şifresini tutar. Bu alan da gereklidir ve kullanıcı kaydı sırasında boş bırakılamaz.

Email: Kullanıcının e-posta adresini belirtir. Bu alan, kayıt işlemleri için gereklidir ve geçerli bir e-posta adresi olmalıdır.

FirstName: Kullanıcının adını belirtir. Bu alan isteğe bağlıdır.

LastName: Kullanıcının soyadını belirtir. Bu alan da isteğe bağlıdır.

Kullanım:
Bu ViewModel, kullanıcı kaydı sayfasında form verilerini toplamak için kullanılabilir. Örneğin, kullanıcıların kayıt olma süreçlerinde bu alanlardan bilgi alabilir ve geçerli verilerle kullanıcının kaydını gerçekleştirebilirsiniz.
 */