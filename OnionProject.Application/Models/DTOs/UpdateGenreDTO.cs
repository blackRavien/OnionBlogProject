using OnionProject.Domain.Enum; // Status enum'u için gerekli namespace
using System;
using System.ComponentModel.DataAnnotations; // Veri doğrulama için gerekli namespace

namespace OnionProject.Application.Models.DTOs
{
    // Tür güncelleme işlemleri için DTO sınıfı
    public class UpdateGenreDTO
    {
        // Türün mevcut id'sini almak için gerekli. Bu yüzden zorunlu olarak işaretledik.
        [Required(ErrorMessage = "Tür ID'si gereklidir.")]
        public int Id { get; set; } // Türün sistemdeki kimliği

        // Türün adını güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "Tür ismi zorunludur.")] // Zorunlu alan
        [MinLength(3, ErrorMessage = "Tür ismi en az 3 karakterden oluşmalıdır.")] // Minimum uzunluk kontrolü
        [Display(Name = "Tür")] // Görsel düzenleme için etiket
        public string Name { get; set; } // Türün ismi

        // Türün durumunu (aktif, pasif vs.) güncellemek için gerekli alan.
        public Status Status { get; set; } // Türün durumu

        // Türün güncelleme tarihini tutan alan. Güncelleme işleminde tarih otomatik olarak atanır.
        public DateTime UpdatedDate => DateTime.Now; // Güncelleme tarihi
    }
}
/*
    Genel Özet:
UpdateGenreDTO sınıfı, tür güncelleme işlemleri için kullanılan bir DTO'dur. Aşağıda sınıfın içerdiği alanlar ve açıklamaları yer almaktadır:

Id: Türün sistemdeki kimliğini belirtir. Bu alan zorunludur.
Name: Türün adını güncellemek için kullanılır. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
Status: Türün durumunu (aktif, pasif vb.) belirtir. Status enum'undan gelir.
UpdatedDate: Türün güncelleme tarihini belirtir. Güncelleme işlemi sırasında otomatik olarak güncel tarih atanır.
Bu DTO, tür bilgilerini güncellerken gerekli verileri toplamak ve doğrulamak amacıyla tasarlanmıştır.
 */