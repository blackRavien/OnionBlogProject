using OnionProject.Domain.Enum;  // Domain katmanındaki Status enum'ını kullanmak için gerekli
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Domain.Entities
{
    // Her varlık için temel özellikleri tanımlayan bir arayüz
    public interface IBaseEntity
    {
        // Oluşturulma tarihini tutan bir alan. Zorunludur.
        DateTime CreatedDate { get; set; }

        // Güncellenme tarihini tutan bir alan. Opsiyoneldir (null olabilir).
        DateTime? UpdatedDate { get; set; }

        // Silinme tarihini tutan bir alan. Opsiyoneldir (null olabilir).
        DateTime? DeletedDate { get; set; }

        // Varlığın durumu (Aktif, Pasif, Silindi vb.) için bir Status enum'ı. Zorunludur.
        Status Status { get; set; }
    }
}

/* 
    Açıklamalar:
IBaseEntity: Bu, projedeki her entity (örneğin, Post, Author, Genre gibi) için temel bir arayüzdür. Yani bu arayüzü uygulayan tüm varlıklar (entity'ler) aynı temel özelliklere sahip olur.

CreatedDate: Her entity'nin oluşturulma tarihini tutar ve zorunlu bir alandır. Bir varlık oluşturulduğunda bu tarih atanır.

UpdatedDate: Entity'nin ne zaman güncellendiğini tutar. Bu alan opsiyoneldir, yani entity ilk oluşturulduğunda bu alan boş olabilir.

DeletedDate: Entity'nin ne zaman silindiğini tutar. Bu alan da opsiyoneldir, çünkü her entity silinmeyebilir.

Status: Bu alan, varlığın durumunu belirler. Status, muhtemelen bir enum (sıralı tür) olarak tanımlanmıştır ve değer olarak varlığın durumunu ifade eder (örneğin, Aktif, Pasif, Silindi gibi).

Kullanım Amacı:
IBaseEntity arayüzü, projedeki tüm entity'ler için ortak özellikleri tanımlar. Bu sayede, tüm entity'lerin CreatedDate, UpdatedDate, DeletedDate ve Status gibi özelliklere sahip olması sağlanır. Bu, kodun daha düzenli ve tutarlı olmasını sağlar.
Her entity bu arayüzü implemente ederek, kendisine özgü özelliklerin yanı sıra bu ortak özellikleri de barındırır. Örneğin, bir Post entity'si bu arayüzü implemente ettiğinde hem kendine özgü alanlara (Title, Content, vs.) hem de CreatedDate, Status gibi alanlara sahip olur.
Bu tür bir yapı, projede ortak özelliklerin tekrar tekrar tanımlanmasını önler ve kodun yönetilebilirliğini artırır.
 */