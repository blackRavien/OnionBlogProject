using Microsoft.AspNetCore.Http; // Dosya yükleme işlemleri için gerekli namespace
using System;
using System.ComponentModel.DataAnnotations; // Veri doğrulama için gerekli namespace

namespace OnionProject.Application.Models.DTOs
{
    // Kullanıcı güncelleme işlemleri için DTO sınıfı
    public class UpdateAppUserDTO
    {
        // Kullanıcının mevcut id'sini almak için gerekli. Bu yüzden zorunlu olarak işaretledik.
        [Required(ErrorMessage = "Kullanıcı ID'si gereklidir.")]
        public string Id { get; set; } // Kullanıcının sistemdeki ID'si

        // Kullanıcının ismini güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "İsmin girilmesi zorunludur.")] // Zorunlu alan
        [MinLength(3, ErrorMessage = "Adınızın en az 3 harfli olması gereklidir.")] // Minimum uzunluk kontrolü
        [Display(Name = "İsim")] // Görsel düzenleme için etiket
        public string FirstName { get; set; } // Kullanıcının adı

        // Kullanıcının soyadını güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "Soyismin girilmesi zorunludur.")] // Zorunlu alan
        [MinLength(3, ErrorMessage = "Soyadınızın en az 3 harfli olması gereklidir.")] // Minimum uzunluk kontrolü
        [Display(Name = "Soyisim")] // Görsel düzenleme için etiket
        public string LastName { get; set; } // Kullanıcının soyadı

        // Kullanıcı profil fotoğrafını güncellemek için dosya yükleme özelliği.
        public IFormFile UploadPath { get; set; } // Kullanıcının yeni profil fotoğrafı

        // Kullanıcının mevcut profil fotoğrafı (opsiyonel).
        public string? ImagePath { get; set; } // Kullanıcının mevcut profil fotoğrafı yolu

        // Kullanıcının güncelleme tarihini tutan alan. Güncelleme işleminde tarih otomatik olarak atanır.
        public DateTime UpdatedDate => DateTime.Now; // Güncelleme tarihi
    }
}
/*
    Genel Özet:
UpdateAppUserDTO sınıfı, kullanıcı güncelleme işlemleri için kullanılan bir DTO'dur. Aşağıda sınıfın içerdiği alanlar ve açıklamaları yer almaktadır:

Id: Kullanıcının sistemdeki kimliğini belirtir. Bu alan zorunludur.
FirstName: Kullanıcının adını güncellemek için kullanılır. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
LastName: Kullanıcının soyadını güncellemek için kullanılır. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
UploadPath: Kullanıcının yeni profil fotoğrafını yüklemek için kullanılır. IFormFile türündedir.
ImagePath: Kullanıcının mevcut profil fotoğrafının yolunu tutar. Bu alan isteğe bağlıdır (opsiyoneldir).
UpdatedDate: Kullanıcının güncelleme tarihini belirtir. Güncelleme işlemi sırasında otomatik olarak güncel tarih atanır.
Bu DTO, kullanıcı bilgilerini güncellerken gerekli verileri toplamak ve doğrulamak amacıyla tasarlanmıştır.
 */