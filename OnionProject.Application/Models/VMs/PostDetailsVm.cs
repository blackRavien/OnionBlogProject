using System;
using System.Collections.Generic;

namespace OnionProject.Application.Models.VMs
{
    // Gönderinin detaylarını tutan ViewModel sınıfı
    public class PostDetailsVm
    {
        public int Id { get; set; } // Gönderinin ID'si
        public string? Title { get; set; } // Gönderinin başlığı
        public string? Content { get; set; } // Gönderinin içeriği
        public string? ImagePath { get; set; } // Gönderinin resmi
        public string? GenreName { get; set; } // Türün adı
        public string? AuthorFirstName { get; set; } // Yazarın adı
        public string? AuthorLastName { get; set; } // Yazarın soyadı
        public string AuthorFullName => $"{AuthorFirstName} {AuthorLastName}"; // Yazarın tam adı
        public List<CommentVm>? Comments { get; set; } // Gönderiye ait yorumlar
        public DateTime CreatedDate { get; set; } // Gönderinin oluşturulma tarihi
        public AuthorDetailVm? AuthorDetailVm { get; set; } // Yazarın detayları
    }
}

/*
    Genel Özet:
PostDetailsVm sınıfı, bir gönderinin detaylarını görüntülemek için kullanılan bir ViewModel'dir. Bu sınıf, gönderinin tüm bilgilerini ve ilgili yorumları içerir.

Alanlar:
Id: Gönderinin benzersiz tanımlayıcısı. Bu alan zorunlu ve her gönderi için farklıdır.

Title: Gönderinin başlığı. Nullable olarak tanımlanmış, böylece başlık verilmeyebilir.

Content: Gönderinin içeriği. Nullable olup içeriğin olmaması durumunu destekler.

ImagePath: Gönderinin görselinin yolu. Nullable olarak tanımlanmıştır.

GenreName: Gönderinin ait olduğu türün adı. Nullable olup tür bilgisi verilmeyebilir.

AuthorFirstName: Yazarın adı. Nullable, bu yüzden yazar adı verilmediğinde null olabilir.

AuthorLastName: Yazarın soyadı. Nullable.

AuthorFullName: Yazarın tam adını döndüren bir özellik. AuthorFirstName ve AuthorLastName birleşiminden oluşur.

Comments: Gönderiye ait yorumları tutan bir liste. CommentVm türündedir ve nullable olarak tanımlanmıştır.

CreatedDate: Gönderinin oluşturulma tarihini saklar.

AuthorDetailVm: Yazarın detaylarını tutan başka bir ViewModel. Nullable, bu yüzden yazar detayları sağlanmayabilir.

Kullanım:
Bu ViewModel, bir gönderinin detay sayfasında kullanılmak üzere tasarlanmıştır. Gönderinin başlığı, içeriği, yazarı, görseli ve yorumları gibi bilgileri içerir.
 */