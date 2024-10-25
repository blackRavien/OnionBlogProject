using OnionProject.Domain.Enum; // Projede kullanılan enum yapılarının bulunduğu namespace
using System; // Temel C# sınıf kütüphanesi
using System.ComponentModel.DataAnnotations; // Doğrulama ve display attribute'lerini kullanmak için gerekli namespace

namespace OnionProject.Application.Models.DTOs
{
    // Yeni bir tür oluşturmak için kullanılan DTO sınıfı
    public class CreateGenreDTO
    {
        // Tür ismi zorunlu bir alan olup, en az 2 karakter içermelidir. 
        // [Required] ve [MinLength] ile doğrulama kuralları belirtilmiştir.
        [Required(ErrorMessage = "Tür ismi gereklidir.")] // Zorunlu alan hatası mesajı
        [MinLength(2, ErrorMessage = "Tür ismi en az 2 karakterden oluşmalıdır.")] // Minimum karakter sayısı uyarısı
        [Display(Name = "Tür İsmi")] // Formda bu alanın nasıl görüntüleneceğini belirleyen Display attribute
        public string Name { get; set; } // Tür ismini tutacak alan

        // Türün oluşturulma tarihini otomatik olarak güncel tarih olarak ayarlayan property.
        public DateTime CreatedDate => DateTime.Now; // Oluşturulma tarihini her zaman "şu an" olarak set eder

        // Türün başlangıçta aktif olması için varsayılan olarak "Active" status'ünü ayarlayan property.
        public Status Status => Status.Active; // Enum değerlerinden Status.Active atanmış
    }
}

/*
    Genel Özet:
CreateGenreDTO sınıfı, yeni bir tür (genre) oluşturmak için kullanılan bir DTO'dur. Kullanıcı tarafından girilmesi gereken tür ismi için bazı doğrulamalar yapılmıştır. Tür ismi en az 2 karakter içermeli ve zorunlu bir alandır. Oluşturulma tarihi (CreatedDate) ve durum (Status) gibi alanlar ise otomatik olarak set edilir; yeni bir tür oluşturulduğunda durum aktif (Status.Active) olacaktır.

Bu DTO, tür oluşturma işlemi sırasında sunucuya gönderilecek verilerin taşıyıcısı olarak kullanılır ve sunucu tarafında gerekli doğrulamalar için hazırlanmıştır.
 */