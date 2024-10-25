using Microsoft.AspNetCore.Http; // Dosya yükleme işlemleri için gerekli namespace
using OnionProject.Domain.Enum; // Status enum'u için gerekli namespace
using System;
using System.ComponentModel.DataAnnotations; // Veri doğrulama için gerekli namespace

namespace OnionProject.Application.Models.DTOs
{
    // Yazar güncelleme işlemleri için DTO sınıfı
    public class UpdateAuthorDTO
    {
        // Yazarın mevcut id'sini almak için gerekli. Bu yüzden zorunlu olarak işaretledik.
        [Required(ErrorMessage = "Yazar ID'si gereklidir.")]
        public int Id { get; set; } // Yazarın sistemdeki kimliği

        // Yazarın ismini güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "İsmin girilmesi zorunludur.")] // Zorunlu alan
        [MinLength(3, ErrorMessage = "Adınızın en az 3 harfli olması gereklidir.")] // Minimum uzunluk kontrolü
        [Display(Name = "İsim")] // Görsel düzenleme için etiket
        public string FirstName { get; set; } // Yazarın adı

        // Yazarın soyadını güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "Soyismin girilmesi zorunludur.")] // Zorunlu alan
        [MinLength(3, ErrorMessage = "Soyadınızın en az 3 harfli olması gereklidir.")] // Minimum uzunluk kontrolü
        [Display(Name = "Soyisim")] // Görsel düzenleme için etiket
        public string LastName { get; set; } // Yazarın soyadı

        // Yazarın yeni profil fotoğrafını güncellemek için dosya yükleme özelliği.
        public IFormFile? Image { get; set; } // Yeni profil fotoğrafı

        // Yazarın mevcut profil fotoğrafı (opsiyonel).
        public string? ImagePath { get; set; } // Mevcut profil fotoğrafının yolu

        // Yazarın durumunu (aktif, pasif vs.) güncellemek için gerekli alan.
        public Status Status { get; set; } // Yazarın durumu

        // Yazarın güncelleme tarihini tutan alan. Güncelleme işleminde tarih otomatik olarak atanır.
        public DateTime UpdatedDate => DateTime.Now; // Güncelleme tarihi

        // Yeni alanlar
        [Required(ErrorMessage = "E-posta adresi zorunludur.")] // Zorunlu alan
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")] // E-posta format kontrolü
        public string Email { get; set; } // Yazarın e-posta adresi

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")] // Telefon numarası format kontrolü
        public string PhoneNumber { get; set; } // Yazarın telefon numarası

        public string Biography { get; set; } // Yazarın biyografisi
    }
}
/*
    Genel Özet:
UpdateAuthorDTO sınıfı, yazar güncelleme işlemleri için kullanılan bir DTO'dur. Aşağıda sınıfın içerdiği alanlar ve açıklamaları yer almaktadır:

Id: Yazarın sistemdeki kimliğini belirtir. Bu alan zorunludur.
FirstName: Yazarın adını güncellemek için kullanılır. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
LastName: Yazarın soyadını güncellemek için kullanılır. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
Image: Yazarın yeni profil fotoğrafını yüklemek için kullanılır. IFormFile? türündedir ve opsiyonel olarak tanımlanmıştır.
ImagePath: Yazarın mevcut profil fotoğrafının yolunu tutar. Bu alan isteğe bağlıdır (opsiyoneldir).
Status: Yazarın durumunu (aktif, pasif vb.) belirtir. Status enum'undan gelir.
UpdatedDate: Yazarın güncelleme tarihini belirtir. Güncelleme işlemi sırasında otomatik olarak güncel tarih atanır.
Email: Yazarın e-posta adresini günceller. Zorunlu bir alan olup geçerli bir e-posta formatı kontrol edilir.
PhoneNumber: Yazarın telefon numarasını günceller. Geçerli bir telefon numarası formatı kontrol edilir.
Biography: Yazarın biyografisini tutar.
Bu DTO, yazar bilgilerini güncellerken gerekli verileri toplamak ve doğrulamak amacıyla tasarlanmıştır.
 */