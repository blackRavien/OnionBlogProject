using System;

namespace OnionProject.Application.Models.VMs
{
    public class AuthorDetailVm
    {
        // Temel bilgiler
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImagePath { get; set; }

        // Tarihler
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }  // Güncellenmiş tarih, nullable olabilir
        public DateTime? DeletedDate { get; set; }  // Silinme tarihi, nullable olabilir

        // Durum bilgisi
        public string Status { get; set; }  // Author'ın aktif/pasif durumu

        // Ekstra bilgiler
        public string Biography { get; set; }  // Yazarın biyografisi
        public string Email { get; set; }  // Yazarın e-posta adresi
        public string PhoneNumber { get; set; }  // Yazarın telefon numarası

        // İlişkili veriler
        public int NumberOfPosts { get; set; }  // Yazarın sahip olduğu yazı sayısı
        public string FullName => $"{FirstName} {LastName}";  // Full name hesaplanmış özellik
    }
}
