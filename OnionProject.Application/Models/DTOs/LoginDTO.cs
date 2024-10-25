using System.ComponentModel.DataAnnotations; // Veri doğrulama için gerekli namespace

namespace OnionProject.Application.Models.DTOs
{
    // Kullanıcı giriş bilgilerini temsil eden DTO sınıfı
    public class LoginDTO
    {
        // Giriş için gerekli olan email adresi
        [Required(ErrorMessage = "Email adresi gereklidir.")] // Email zorunlu alan
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")] // Geçerli email formatını kontrol eder
        [Display(Name = "Email Adresi")] // Görüntüleme adı
        public string Email { get; set; } // Kullanıcının giriş yaparken kullanacağı email adresi

        // Giriş için gerekli olan şifre
        [Required(ErrorMessage = "Şifre girilmesi zorunludur.")] // Şifre zorunlu alan
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır.")] // Şifre minimum uzunluğunu kontrol eder
        [DataType(DataType.Password)] // Şifrenin gizli tutulmasını sağlar
        [Display(Name = "Şifre")] // Görüntüleme adı
        public string Password { get; set; } // Kullanıcının giriş yaparken kullanacağı şifre
    }
}

/*
    Genel Özet:
LoginDTO sınıfı, kullanıcı giriş bilgilerini temsil etmek için kullanılan bir DTO'dur. Bu sınıf, kullanıcının giriş yaparken gerekli bilgileri içerir. Aşağıda sınıfın içerdiği alanlar ve açıklamaları yer almaktadır:

Email: Kullanıcının giriş yaparken kullanacağı e-posta adresidir. Bu alan zorunludur ve geçerli bir e-posta formatında olmalıdır.
Password: Kullanıcının giriş yaparken kullanacağı şifredir. Bu alan da zorunludur ve en az 6 karakter uzunluğunda olmalıdır. Ayrıca, şifrenin gizli tutulması için DataType.Password ile işaretlenmiştir.
Bu DTO, giriş işlemi sırasında kullanıcının e-posta ve şifre bilgilerini almak için tasarlanmıştır. Doğrulama özellikleri ile birlikte, kullanıcıdan alınan verilerin geçerliliğini sağlamak için çeşitli kontroller içerir.
 */