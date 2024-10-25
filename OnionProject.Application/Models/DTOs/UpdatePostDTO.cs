using Microsoft.AspNetCore.Http; // Dosya yükleme için gerekli namespace
using OnionProject.Domain.Enum; // Status enum'u için gerekli namespace
using System;
using System.ComponentModel.DataAnnotations; // Veri doğrulama için gerekli namespace

namespace OnionProject.Application.Models.DTOs
{
    // Gönderi güncelleme işlemleri için DTO sınıfı
    public class UpdatePostDTO
    {
        // Gönderinin mevcut id'sini almak için gerekli. Bu yüzden zorunlu olarak işaretledik.
        [Required(ErrorMessage = "Gönderi ID'si gereklidir.")]
        public int Id { get; set; } // Gönderinin sistemdeki kimliği

        // Gönderinin başlığını güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "Başlık girilmesi zorunludur.")] // Zorunlu alan
        [MinLength(3, ErrorMessage = "Başlığın en az 3 karakterli olması gereklidir.")] // Minimum uzunluk kontrolü
        [Display(Name = "Başlık")] // Görsel düzenleme için etiket
        public string Title { get; set; } // Gönderinin başlığı

        // Gönderinin içeriğini güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "İçeriğin girilmesi zorunludur.")] // Zorunlu alan
        [Display(Name = "İçerik")] // Görsel düzenleme için etiket
        public string Content { get; set; } // Gönderinin içeriği

        // Gönderinin yeni resmini güncellemek için dosya yükleme özelliği.
        public IFormFile? UploadPath { get; set; } // Yeni resim yükleme alanı

        // Gönderinin mevcut resim yolu (opsiyonel).
        public string? ImagePath { get; set; } // Mevcut resim yolu

        // Gönderinin durumunu (aktif, pasif vs.) güncellemek için gerekli alan.
        public Status Status { get; set; } // Gönderinin durumu

        // Gönderinin güncelleme tarihini tutan alan. Güncelleme işleminde tarih otomatik olarak atanır.
        public DateTime? UpdatedDate { get; set; } // Güncelleme tarihi

        // Gönderinin yazarını güncellemek için gerekli olan yazar id'si.
        [Required(ErrorMessage = "Yazar ID'si gereklidir.")]
        public int AuthorId { get; set; } // Yazarın kimliği

        // Gönderinin türünü güncellemek için gerekli olan tür id'si.
        [Required(ErrorMessage = "Tür ID'si gereklidir.")]
        public int GenreId { get; set; } // Türün kimliği

        // Gönderinin oluşturulma tarihini tutan alan.
        public DateTime? CreatedDate { get; set; } // Oluşturulma tarihi
    }
}

/*
    Genel Özet:
UpdatePostDTO sınıfı, gönderi güncelleme işlemleri için kullanılan bir DTO'dur. Aşağıda sınıfın içerdiği alanlar ve açıklamaları yer almaktadır:

Id: Gönderinin sistemdeki kimliğini belirtir. Bu alan zorunludur.
Title: Gönderinin başlığını güncellemek için kullanılır. Zorunlu bir alandır ve en az 3 karakter uzunluğunda olmalıdır.
Content: Gönderinin içeriğini güncellemek için kullanılır. Bu alan da zorunludur.
UploadPath: Yeni bir gönderi resmi yüklemek için kullanılan alan. Opsiyonel bir dosya yükleme özelliğidir.
ImagePath: Gönderinin mevcut resim yolunu belirtir. Bu alan opsiyoneldir.
Status: Gönderinin durumunu (aktif, pasif vb.) belirtir. Status enum'undan gelir.
UpdatedDate: Gönderinin güncelleme tarihini belirtir. Bu alan opsiyoneldir.
AuthorId: Gönderinin yazarını güncellemek için gerekli olan yazar kimliğidir. Zorunlu bir alandır.
GenreId: Gönderinin türünü güncellemek için gerekli olan tür kimliğidir. Zorunlu bir alandır.
CreatedDate: Gönderinin oluşturulma tarihini belirtir. Bu alan opsiyoneldir.
Bu DTO, gönderi bilgilerini güncellerken gerekli verileri toplamak ve doğrulamak amacıyla tasarlanmıştır.
 */