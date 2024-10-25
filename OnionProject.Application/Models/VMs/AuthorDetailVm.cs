using System;

namespace OnionProject.Application.Models.VMs
{
    // Yazar detaylarını içeren ViewModel sınıfı
    public class AuthorDetailVm
    {
        // Temel bilgiler
        public int Id { get; set; } // Yazarın benzersiz kimliği
        public string FirstName { get; set; } // Yazarın adı
        public string LastName { get; set; } // Yazarın soyadı
        public string ImagePath { get; set; } // Yazarın profil resminin yolu

        // Tarihler
        public DateTime CreatedDate { get; set; } // Yazarın oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // Güncellenme tarihi, nullable olabilir
        public DateTime? DeletedDate { get; set; } // Silinme tarihi, nullable olabilir

        // Durum bilgisi
        public string Status { get; set; } // Yazarın aktif/pasif durumu

        // Ekstra bilgiler
        public string Biography { get; set; } // Yazarın biyografisi
        public string Email { get; set; } // Yazarın e-posta adresi
        public string PhoneNumber { get; set; } // Yazarın telefon numarası

        // İlişkili veriler
        public int NumberOfPosts { get; set; } // Yazarın sahip olduğu yazı sayısı
        public string FullName => $"{FirstName} {LastName}"; // Full name hesaplanmış özellik
    }
}

/*
    Genel Özet:
AuthorDetailVm sınıfı, yazar detaylarını görüntülemek için kullanılan bir ViewModel'dir. Aşağıda sınıfın içerdiği alanlar ve açıklamaları yer almaktadır:

Id: Yazarın sistemdeki benzersiz kimliğini belirtir.
FirstName: Yazarın adını tutar.
LastName: Yazarın soyadını tutar.
ImagePath: Yazarın profil resminin dosya yolunu tutar.
Tarihler:
CreatedDate: Yazarın oluşturulma tarihini belirtir.
UpdatedDate: Yazarın en son güncellenme tarihini belirtir. Bu alan nullable olarak tanımlanmıştır, böylece yazar hiç güncellenmemişse null değer alabilir.
DeletedDate: Yazarın silinme tarihini tutar. Nullable olarak tanımlanmıştır.
Durum Bilgisi:
Status: Yazarın aktif veya pasif durumunu belirtir. Bu, yazarın durumu hakkında bilgi verir.
Ekstra Bilgiler:
Biography: Yazarın biyografisini tutar.
Email: Yazarın e-posta adresini tutar.
PhoneNumber: Yazarın telefon numarasını tutar.
İlişkili Veriler:
NumberOfPosts: Yazarın sahip olduğu toplam yazı sayısını tutar.
FullName: Yazarın tam adını döndüren hesaplanmış bir özelliktir. FirstName ve LastName alanlarını birleştirerek oluşturur.
Kullanım:
Bu ViewModel, yazar detaylarını görüntülemek için kullanılır ve hem temel bilgileri hem de ilişkili verileri içerir. Bu, yazarlarla ilgili kapsamlı bir bilgi seti sunar.
 */