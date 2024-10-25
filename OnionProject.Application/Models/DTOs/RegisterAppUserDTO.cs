using OnionProject.Domain.Enum; // Domain enum'ları için gerekli namespace
using System;
using System.ComponentModel.DataAnnotations; // Veri doğrulama için gerekli namespace

namespace OnionProject.Application.Models.DTOs
{
    // Kullanıcı kayıt işlemleri için DTO sınıfı
    public class RegisterAppUserDTO
    {
        // Kullanıcı bilgilerini tutan alanlar
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")] // Kullanıcı adı zorunlu alan
        [MinLength(3, ErrorMessage = "Kullanıcı adı en az 3 karakterden oluşmalıdır.")] // Minimum uzunluk kontrolü
        [Display(Name = "Kullanıcı Adı")] // Görsel düzenleme için etiket
        public string UserName { get; set; } // Kullanıcının sistemdeki adı

        [Required(ErrorMessage = "Email adresi zorunludur.")] // Email zorunlu alan
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")] // Geçerli email formatı kontrolü
        [Display(Name = "Email")] // Görsel düzenleme için etiket
        public string Email { get; set; } // Kullanıcının email adresi

        [Required(ErrorMessage = "Şifre zorunludur.")] // Şifre zorunlu alan
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakterden oluşmalıdır.")] // Minimum uzunluk kontrolü
        [DataType(DataType.Password)] // Şifrenin gizli tutulmasını sağlar
        [Display(Name = "Şifre")] // Görsel düzenleme için etiket
        public string Password { get; set; } // Kullanıcı şifresi

        [Required(ErrorMessage = "İsim zorunludur.")] // İsim zorunlu alan
        [Display(Name = "İsim")] // Görsel düzenleme için etiket
        public string FirstName { get; set; } // Kullanıcının adı

        [Required(ErrorMessage = "Soyisim zorunludur.")] // Soyisim zorunlu alan
        [Display(Name = "Soyisim")] // Görsel düzenleme için etiket
        public string LastName { get; set; } // Kullanıcının soyadı

        [Display(Name = "Kullanıcı Rolü")] // Görsel düzenleme için etiket
        public string Role { get; set; } = "Member"; // Kullanıcının rolü, varsayılan olarak "Member"

        public DateTime CreatedDate => DateTime.Now; // Kullanıcının oluşturulma tarihi otomatik olarak atanır

        public Status Status => Status.Active; // Kullanıcı durumu varsayılan olarak aktif olur
    }
}

/*
    Genel Özet:
RegisterAppUserDTO sınıfı, kullanıcı kayıt işlemleri için kullanılan bir DTO'dur. Aşağıda sınıfın içerdiği alanlar ve açıklamaları yer almaktadır:

UserName: Kullanıcının sistemdeki adı. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
Email: Kullanıcının e-posta adresi. Zorunlu bir alandır ve geçerli bir e-posta formatında olmalıdır.
Password: Kullanıcının şifresidir. Zorunlu bir alandır ve en az 6 karakter uzunluğunda olmalıdır.
FirstName: Kullanıcının adıdır. Zorunlu bir alandır.
LastName: Kullanıcının soyadıdır. Zorunlu bir alandır.
Role: Kullanıcının rolünü belirtir. Varsayılan değeri "Member" olarak atanmıştır.
CreatedDate: Kullanıcının oluşturulma tarihidir. Otomatik olarak güncel tarih atanır.
Status: Kullanıcının durumunu belirtir. Varsayılan olarak aktif (Status.Active) olarak atanmıştır.
Bu DTO, kullanıcı kayıt işlemi sırasında gerekli bilgileri toplamak ve doğrulamak amacıyla tasarlanmıştır.
 */