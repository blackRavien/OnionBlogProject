using Microsoft.AspNetCore.Http; // HTTP istekleri ve dosya yüklemeleri için kullanılan kütüphane
using OnionProject.Application.Extensions; // Projede yazılan özel validasyon (doğrulama) attribute'larının yer aldığı katman
using OnionProject.Domain.Enum; // Domain katmanında tanımlanan enumları (durumlar gibi) kullanır
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // DataAnnotations ile model doğrulama özellikleri
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    // Yeni bir yazar eklerken kullanılacak Data Transfer Object (DTO) sınıfı
    public class CreateAuthorDTO
    {
        // Yazarın adı zorunlu olarak girilmeli ve en az 3 karakterden oluşmalıdır
        [Required(ErrorMessage = "İsmin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Adınızın en az 3 harfli olması gereklidir.")]
        [Display(Name = "İsim")] // Görüntüleme için ismin karşılığı
        public string FirstName { get; set; }

        // Yazarın soyadı zorunlu olarak girilmeli ve en az 3 karakterden oluşmalıdır
        [Required(ErrorMessage = "Soyismin girilmesi zorunludur.")]
        [MinLength(3, ErrorMessage = "Soyadınızın en az 3 harfli olması gereklidir.")]
        [Display(Name = "Soyisim")] // Görüntüleme için soyismin karşılığı
        public string LastName { get; set; }

        // Resim dosyası için özel bir validasyon attribute kullanılıyor. Sadece "jpg", "png", "jpeg" uzantılarını kabul eder.
        [PictureFileExtension(ErrorMessage = "Lütfen geçerli bir resim formatı yükleyin.")]
        public IFormFile Image { get; set; } // Resim dosyasını yüklemek için kullanılan alan

        // Yazarın resmi yüklenirken, bu alanda resmin sunucu üzerindeki yolu tutulur
        public string? ImagePath { get; set; } // Resmin dosya sistemindeki yolu

        // CreatedDate alanı, DTO oluşturulduğunda varsayılan olarak şimdiki tarihi verir
        public DateTime CreatedDate => DateTime.Now; // Yazarın eklenme tarihi

        // Status alanı, yazarın varsayılan olarak aktif durumda olmasını sağlar
        public Status Status => Status.Active; // Yazarın durumu (aktif, pasif, vb.)

        // Yazarın e-posta adresi zorunlu ve geçerli bir formatta olmalıdır
        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; } // Yazarın e-posta adresi

        // Telefon numarası zorunlu değil, fakat format kontrolü yapılır
        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        public string PhoneNumber { get; set; } // Yazarın telefon numarası

        // Yazarın biyografisi için alan, maksimum 2000 karakterle sınırlandırılmıştır
        [MaxLength(2000, ErrorMessage = "Biyografi en fazla 2000 karakter olabilir.")]
        public string Biography { get; set; } // Yazarın biyografisi, özgeçmişi
    }
}

/*
    Genel Özet:
Bu CreateAuthorDTO sınıfı, yeni bir yazar oluştururken kullanılan DTO'dur. İçerisinde yazarın adı, soyadı, resmi, e-posta adresi, telefon numarası ve biyografisi gibi bilgiler bulunur. Ayrıca, resim formatını kontrol eden bir özel validasyon işlemi ve yazarın oluşturulma tarihi ile durumu gibi bilgiler de yer alır. DataAnnotations kullanılarak her alanın doğrulama kuralları belirtilmiştir, böylece istemci tarafından gönderilen veriler denetlenebilir. Bu DTO, yazar ekleme işlemlerinde API'ye veri taşımak için kullanılır.
 */