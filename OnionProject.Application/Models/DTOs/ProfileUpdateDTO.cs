using Microsoft.AspNetCore.Http; // Gerekli namespace
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Veri doğrulama için gerekli namespace
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    // Kullanıcı profil güncellemeleri için DTO sınıfı
    public class ProfileUpdateDTO
    {
        // Kullanıcı adı ve soyadı gibi temel bilgileri tutan alanlar
        [Required(ErrorMessage = "İsim girilmesi zorunludur.")] // İsim zorunlu alan
        [MinLength(3, ErrorMessage = "İsminiz en az 3 karakter olmalıdır.")] // Minimum uzunluk kontrolü
        public string? FirstName { get; set; } // Kullanıcının adı

        [Required(ErrorMessage = "Soyisim girilmesi zorunludur.")] // Soyisim zorunlu alan
        [MinLength(3, ErrorMessage = "Soyisminiz en az 3 karakter olmalıdır.")] // Minimum uzunluk kontrolü
        public string? LastName { get; set; } // Kullanıcının soyadı

        [Required(ErrorMessage = "Username girilmesi zorunludur.")] // Kullanıcı adı zorunlu alan
        [MinLength(3, ErrorMessage = "İsminiz en az 3 karakter olmalıdır.")] // Minimum uzunluk kontrolü
        public string? UserName { get; set; } // Kullanıcının kullanıcı adı

        [Required(ErrorMessage = "Email girilmesi zorunludur.")] // Email zorunlu alan
        public string? Email { get; set; } // Kullanıcının email adresi

        public string? Password { get; set; } // Şifre güncelleme için (opsiyonel)
        public string? PhoneNumber { get; set; } // Kullanıcının telefon numarası (opsiyonel)
        public string? PasswordHash { get; set; } //parola hash varsa bunun üzerinden taşınır
    }
}
/*
    Genel Özet:
ProfileUpdateDTO sınıfı, kullanıcı profil bilgilerini güncellemek için kullanılan bir DTO'dur. Aşağıda sınıfın içerdiği alanlar ve açıklamaları yer almaktadır:

FirstName: Kullanıcının adıdır. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
LastName: Kullanıcının soyadıdır. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
UserName: Kullanıcının kullanıcı adıdır. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
Email: Kullanıcının e-posta adresidir. Zorunlu bir alandır.
Password: Kullanıcının şifresidir. Şifre güncellemesi yapılacaksa bu alan kullanılabilir, ancak zorunlu değildir.
PhoneNumber: Kullanıcının telefon numarasıdır. Bu alan opsiyonel olarak tanımlanmıştır.
Bu DTO, kullanıcı profil bilgilerini güncelleyebilmek için gerekli verileri tutmak amacıyla tasarlanmıştır ve her bir alan için doğrulama kuralları içerir.
 */