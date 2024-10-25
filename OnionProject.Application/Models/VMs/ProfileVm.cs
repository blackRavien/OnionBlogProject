using System;

namespace OnionProject.Application.Models.VMs
{
    // Kullanıcı profili bilgilerini tutan ViewModel
    public class ProfileVm
    {
        public string FirstName { get; set; } // Kullanıcının adı
        public string LastName { get; set; } // Kullanıcının soyadı
        public string UserName { get; set; } // Kullanıcı adı
        public string Email { get; set; } // Kullanıcının e-posta adresi
        public string Password { get; set; } // Kullanıcının şifresi
        public string? PhoneNumber { get; set; } // Kullanıcının telefon numarası (opsiyonel)
    }
}

/*
    Genel Özet:
ProfileVm sınıfı, bir kullanıcının profil bilgilerini tutmak için kullanılan bir ViewModel'dir. Kullanıcıların temel bilgilerini ve kimlik bilgilerini içeren alanlar barındırır.

Alanlar:
FirstName: Kullanıcının adını belirtir.

LastName: Kullanıcının soyadını belirtir.

UserName: Kullanıcının platformdaki kullanıcı adını belirtir.

Email: Kullanıcının e-posta adresini tutar. Bu, genellikle oturum açma veya bildirimler için kullanılır.

Password: Kullanıcının şifresini tutar. Bu alan, kullanıcı kaydı veya şifre güncelleme işlemleri için gereklidir.

PhoneNumber: Kullanıcının telefon numarasını tutar. Bu alan opsiyonel olup, kullanıcı tarafından doldurulabilir.

Kullanım:
Bu ViewModel, kullanıcı profili görüntüleme, düzenleme veya kayıt işlemleri için kullanılabilir. Örneğin, kullanıcı profili sayfasında ya da kayıt formlarında bu alanlar kullanılarak kullanıcıdan bilgi alınabilir veya mevcut bilgiler görüntülenebilir.
 */