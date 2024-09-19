using OnionProject.Domain.Enum;  // Status enum'ını kullanmak için gerekli
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Domain.Entities
{
    public class Genre : IBaseEntity
    {
        // Constructor ile Posts listesi başlatılabilir. Alternatif olarak, alan doğrudan başlatılabilir.
        //public Genre()
        //{
        //    Posts = new List<Post>();
        //}

        // Türün benzersiz kimlik numarası
        public int Id { get; set; }

        // Türün adı
        public string Name { get; set; }

        // IBaseEntity'den gelen özellikler
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }

        // Navigasyon özelliği: Bir türün birden fazla gönderisi olabilir
        public List<Post> Posts { get; set; } = new List<Post>(); // Varsayılan olarak boş bir liste
    }
}

/*
 Genre sınıfı, gönderilere atanacak türleri (kategorileri) temsil eder.
IBaseEntity arayüzünü implemente ederek, oluşturulma, güncellenme, silinme tarihleri ve durum bilgisi gibi ortak entity özelliklerine sahiptir.
Posts özelliği, bir tür ile o türe ait gönderiler arasındaki ilişkiyi yönetir. Bir Genre, birden fazla gönderiye atanabilir.
Genre sınıfı, veritabanında hatalı durumlarla karşılaşmamak için Posts listesini varsayılan olarak başlatır.
 */