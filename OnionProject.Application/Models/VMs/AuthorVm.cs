using OnionProject.Domain.Enum;
using System;

namespace OnionProject.Application.Models.VMs
{
    // Yazar bilgilerini içeren ViewModel sınıfı
    public class AuthorVm
    {
        public int Id { get; set; } // Yazarın benzersiz kimliği
        public string FirstName { get; set; } // Yazarın adı
        public string LastName { get; set; } // Yazarın soyadı

        // Ad ve soyadı birleştirerek tam ad döndüren özellik
        public string AuthorFullName => $"{FirstName} {LastName}";

        public DateTime CreatedDate { get; set; } // Yazarın oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // Yazarın güncellenme tarihi, nullable olabilir
        public DateTime? DeletedDate { get; set; } // Yazarın silinme tarihi, nullable olabilir
        public Status Status { get; set; } // Yazarın durumu (aktif/pasif)

        public string ImagePath { get; set; } // Yazarın profil resminin dosya yolu

        public string UploadPath { get; set; } // Yazarın yeni resminin yükleneceği dosya yolu
    }
}
/*
 Genel Özet:
AuthorVm sınıfı, yazar bilgilerini görüntülemek için kullanılan bir ViewModel'dir. Sınıfın içerdiği alanlar ve açıklamaları aşağıda yer almaktadır:

Id: Yazarın sistemdeki benzersiz kimliğini belirtir.
FirstName: Yazarın adını tutar.
LastName: Yazarın soyadını tutar.
Hesaplanmış Özellik:
AuthorFullName: Yazarın adını ve soyadını birleştirerek tam adı döndüren bir özellik. Bu, UI'da yazarın adını daha okunabilir bir şekilde göstermek için kullanışlıdır.
Tarihler:
CreatedDate: Yazarın oluşturulma tarihini belirtir.
UpdatedDate: Yazarın en son güncellenme tarihini belirtir. Nullable olarak tanımlanmıştır.
DeletedDate: Yazarın silinme tarihini tutar. Nullable olarak tanımlanmıştır.
Durum Bilgisi:
Status: Yazarın aktif veya pasif durumunu belirtir. Status enum'unda tanımlı olan değerlerden biri olabilir.
Resim Yolları:
ImagePath: Yazarın profil resminin dosya yolunu tutar.
UploadPath: Yazarın yeni profil resminin yükleneceği dosya yolunu belirtir. Bu alan, eğer kullanıcı yeni bir resim yüklemek istiyorsa kullanışlıdır.
Kullanım:
Bu ViewModel, yazar bilgilerini yönetmek ve görüntülemek için kullanılır. Özellikle UI bileşenlerinde yazarların temel bilgilerini ve ilişkili verilerini sergilemek için uygundur. Bu sınıf, yazarların oluşturulma tarihleri, güncellenme durumları ve resim yolları gibi bilgileri içerir.
 */
