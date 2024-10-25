using Microsoft.AspNetCore.Http; // Dosya yükleme işlemleri için gerekli olan namespace
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Data annotation özelliklerini kullanmak için eklenen namespace
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Extensions // Bu sınıfın, OnionProject.Application katmanı içinde yer aldığını belirten namespace
{
    // Custom validation attribute sınıfı, kullanıcı tarafından yüklenen resim dosyalarının uzantılarını kontrol etmek için kullanılır
    public class PictureFileExtensionAttribute : ValidationAttribute // ValidationAttribute sınıfından türeyen custom validation sınıfı
    {
        // Validation işlemini gerçekleştiren metod
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Yüklenen dosyanın IFormFile türünde olup olmadığını kontrol ediyoruz
            IFormFile file = value as IFormFile;

            // Eğer dosya null ise ve validasyona tabi tutulmuyorsa buradan direkt Success dönebiliriz (dosya null durumunu ekleyebilirsin)
            if (file == null)
            {
                return ValidationResult.Success; // Dosya yüklenmemişse validasyondan geçer
            }

            // Dosyanın uzantısını alıp küçük harfe çeviriyoruz (jpg, png, jpeg gibi)
            var extension = Path.GetExtension(file.FileName).ToLower();

            // Geçerli dosya uzantıları listesi
            string[] extensions = { "jpg", "png", "jpeg" };

            // Dosyanın uzantısının geçerli olup olmadığını kontrol ediyoruz
            bool result = extensions.Any(x => extension.EndsWith(x)); // Dosyanın uzantısı geçerli mi diye kontrol ediyor

            // Eğer geçerli bir uzantı değilse, hata mesajı döndürüyoruz
            if (!result)
            {
                return new ValidationResult("Geçerli formatta bir dosya yükleyin! (\"jpg\", \"png\", \"jpeg\")");
            }

            // Eğer geçerli uzantıya sahip bir dosya yüklendiyse validasyondan geçiyor
            return ValidationResult.Success;
        }
    }
}

/*
    Genel Özet:
PictureFileExtensionAttribute.cs dosyası, resim dosyalarının uzantılarını kontrol eden bir custom validation sınıfıdır. ASP.NET Core'da dosya yükleme işlemi sırasında belirli uzantılara (jpg, png, jpeg) sahip olup olmadığını kontrol eder. Dosya uzantısı geçerli değilse bir hata mesajı döndürülür, aksi takdirde validasyon başarılı olur. Bu sınıf, özellikle form işlemleri sırasında kullanıcıların yalnızca belirli türde dosyaları (resimleri) yüklemesini sağlamak için kullanılır.
 */