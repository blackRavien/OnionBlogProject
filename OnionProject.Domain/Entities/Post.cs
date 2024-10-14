using Microsoft.AspNetCore.Http;  // IFormFile sınıfını kullanmak için gerekli
using OnionProject.Domain.Enum;  // Status enum'ını kullanmak için gerekli
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;  // NotMapped attribute'ü için gerekli
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Domain.Entities
{
    public class Post : IBaseEntity
    {
        // Gönderinin benzersiz kimlik numarası
        public int Id { get; set; }

        // Gönderinin başlığı
        public string Title { get; set; }

        // Gönderinin içeriği
        public string Content { get; set; }

        // Gönderinin resim yolunu tutar
        public string? ImagePath { get; set; }

        // Veritabanına kaydedilmeyen, gönderinin dosya yükleme işlemi için kullanılan alan
        [NotMapped] // Bu özellik veritabanına gönderilmez
        public IFormFile? UploadPath { get; set; } // Gönderinin resmini yüklemek için kullanılır

        // IBaseEntity'den gelen özellikler
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }

        // Navigasyon Özellikleri
        public int AuthorId { get; set; } // Yazarın kimlik numarası (Yabancı anahtar)
        public Author? Author { get; set; } // Yazar ile ilişki (nullable)

        public int GenreId { get; set; } // Türün kimlik numarası (Yabancı anahtar)
        public Genre? Genre { get; set; } // Tür ile ilişki (nullable)
    }
}


/*
        Açıklamalar:
Özellikler:
Id: Her Post (Gönderi) için benzersiz bir kimlik numarasıdır. Bu, gönderilerin veritabanında benzersiz bir şekilde tanımlanmasını sağlar.

Title: Gönderinin başlığını tutar. Gönderinin konusunu veya ana fikrini ifade eder.

Content: Gönderinin içeriğini tutar. Gönderinin detaylarını, yazısını veya açıklamasını içerir.

ImagePath: Gönderinin resminin dosya yolunu tutar. Bu, gönderinin sunucuda saklanan görselinin yerini belirtir.

UploadPath: IFormFile tipinde olan bu özellik, dosya yükleme işlemleri için kullanılır. [NotMapped] attribute'ü ile bu özellik veritabanına kaydedilmez. Bu, gönderinin bir resmini yüklemek için kullanılan geçici bir alandır.

IBaseEntity Özellikleri:
CreatedDate: Gönderinin oluşturulma tarihi.

UpdatedDate: Gönderinin güncellenme tarihi (opsiyoneldir, yani null olabilir).

DeletedDate: Gönderinin silinme tarihi (opsiyoneldir, yani null olabilir).

Status: Gönderinin durumu. Status enum'ı kullanılarak, gönderinin aktif, pasif veya silinmiş olduğunu belirtir.

Navigasyon Özellikleri:
AuthorId: Gönderinin yazarı ile ilişkilendirilmiş bir yabancı anahtardır. Gönderinin hangi yazar tarafından yazıldığını belirtir.

Author: Author (Yazar) ile Post (Gönderi) arasındaki ilişkiyi temsil eder. Bu, bir gönderinin bir yazarı olduğunu belirtir ve bu ilişki nullable (opsiyoneldir) olabilir.

GenreId: Gönderinin türü ile ilişkilendirilmiş bir yabancı anahtardır. Gönderinin hangi türde olduğunu belirtir.

Genre: Genre (Tür) ile Post (Gönderi) arasındaki ilişkiyi temsil eder. Bu, bir gönderinin bir türe ait olduğunu belirtir ve bu ilişki nullable (opsiyoneldir) olabilir.
 */