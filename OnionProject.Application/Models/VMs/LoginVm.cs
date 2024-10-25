using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.VMs
{
    // Kullanıcı giriş bilgilerini tutan ViewModel sınıfı
    public class LoginVm
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string Username { get; set; } // Kullanıcı adı

        [Required(ErrorMessage = "Şifre gereklidir.")]
        public string Password { get; set; } // Şifre
    }
}
/*
    Genel Özet:
LoginVm sınıfı, kullanıcı giriş bilgilerini tutmak için kullanılan bir ViewModel'dir. Bu sınıf, kullanıcıdan alınan kullanıcı adı ve şifre bilgilerini saklar.

Alanlar:
Username: Kullanıcının giriş yaparken kullandığı kullanıcı adı. Required niteliği ile bu alanın doldurulması zorunlu kılınmıştır. Kullanıcı adı girilmediğinde hata mesajı olarak "Kullanıcı adı gereklidir." gösterilecektir.

Password: Kullanıcının giriş yapmak için girdiği şifre. Bu alan da Required niteliğine sahiptir ve "Şifre gereklidir." hata mesajı ile kullanıcıyı bilgilendirir.

Kullanım:
Bu ViewModel, giriş sayfasında kullanıcıdan bilgi almak için kullanılır. Giriş işlemi sırasında, bu sınıfın örneği oluşturularak kullanıcı adı ve şifre bilgileri alınır ve doğrulama işlemleri gerçekleştirilir.
 */