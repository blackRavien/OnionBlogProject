using Microsoft.AspNetCore.Http;  // IFormFile sınıfını kullanmak için gerekli
using Microsoft.AspNetCore.Identity;  // IdentityUser sınıfını kullanmak için gerekli
using OnionProject.Domain.Enum;  // Status enum'ını kullanmak için gerekli
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;  // NotMapped attribute'ü için gerekli
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Domain.Entities
{
    // AppUser sınıfı ASP.NET Core IdentityUser'dan türetilmiştir ve ayrıca IBaseEntity'yi de implemente eder.
    public class AppUser : IdentityUser, IBaseEntity
    {
        // Kullanıcının adı
        public string FirstName { get; set; }

        // Kullanıcının soyadı
        public string LastName { get; set; }

        // Profil fotoğrafının dosya yolu (string olarak saklanır)
        public string ImagePath { get; set; }

        // Bu özellik veritabanında saklanmaz (NotMapped). Kullanıcı fotoğrafını yüklerken kullanılacak.
        [NotMapped]
        public IFormFile UploadPath { get; set; }

        // Kullanıcının oluşturulma tarihi
        public DateTime CreatedDate { get; set; }

        // Kullanıcının güncellenme tarihi (opsiyonel, null olabilir)
        public DateTime? UpdatedDate { get; set; }

        // Kullanıcının silinme tarihi (opsiyonel, null olabilir)
        public DateTime? DeletedDate { get; set; }

        // Kullanıcının durumu (Aktif, Pasif, Silindi vb. için Status enum'ı)
        public Status Status { get; set; }
    }
}


/*
        IdentityUser: AppUser sınıfı, ASP.NET Core'un IdentityUser sınıfından türetilmiş. Bu, kullanıcı kimlik doğrulaması ve yetkilendirme işlemleri için gerekli olan temel kullanıcı özelliklerini sağlar. Bu özellikler, kullanıcı adı, e-posta, şifre gibi bilgileri içerir.

IBaseEntity: AppUser, daha önce açıklanan IBaseEntity arayüzünü implemente eder, bu da her kullanıcı için CreatedDate, UpdatedDate, DeletedDate ve Status gibi özellikleri zorunlu hale getirir.

Alanlar:
FirstName ve LastName: Kullanıcının adı ve soyadı. IdentityUser'ın sağladığı temel kullanıcı bilgilerine ek olarak tanımlanmış.

ImagePath: Kullanıcının profil resminin dosya yolunu tutar. Bu, sunucuda dosya depolama için kullanılır.

UploadPath: IFormFile tipinde olup, kullanıcıdan gelen dosya yükleme işlemi için kullanılır. [NotMapped] özelliği ile bu alan veritabanına yansıtılmaz, sadece uygulama seviyesinde dosya yükleme işlemleri için kullanılır.

IBaseEntity Özellikleri:
CreatedDate: Kullanıcının oluşturulma tarihi.
UpdatedDate: Kullanıcı bilgilerinin güncellenme tarihi (opsiyonel).
DeletedDate: Kullanıcı silinmişse, bu tarihi tutar (opsiyonel).
Status: Kullanıcının aktif, pasif ya da silinmiş olduğunu belirten durum. Bu durumlar Status enum'ı ile yönetilir.
 */