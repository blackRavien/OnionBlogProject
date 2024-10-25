using System; // Temel C# sınıf kütüphanesi
using System.Collections.Generic; // Koleksiyon sınıfları için gerekli namespace
using System.Linq; // LINQ işlevleri için gerekli namespace
using System.Text; // Metin işlemleri için gerekli namespace
using System.Threading.Tasks; // Asenkron programlama için gerekli namespace

namespace OnionProject.Application.Models.DTOs
{
    // Yorum verilerini almak için kullanılan DTO sınıfı
    public class GetCommentDTO
    {
        // Yorumun benzersiz kimliği
        public int Id { get; set; }

        // Yorumun içeriği
        public string Content { get; set; }

        // Yorumun ait olduğu post'un kimliği
        public int PostId { get; set; }

        // Yorum ekleyen standart kullanıcının kimliği (opsiyonel)
        public string? UserId { get; set; } // yorum ekleyen standart user id'si (sadece user yorum yazarsa kullanılacak)

        // Yorum ekleyen admin yazarın kimliği (opsiyonel)
        public int? AuthorId { get; set; }   // yorum ekleyen admin yazar id'si (sadece admin yorum yazarsa kullanılacak)

        // Yorumun oluşturulma tarihi
        public DateTime CreatedAt { get; set; }

        // Yorumun yazarı olan kullanıcının adı
        public string UserName { get; set; } // Kullanıcı adını ekliyoruz.
    }
}

/*
    Genel Özet:
GetCommentDTO sınıfı, bir yorum bilgisini temsil etmek için kullanılan bir DTO'dur. Bu sınıf, yorumun detaylarını içerir ve veri alımı sırasında kullanılır. Aşağıda sınıfın içerdiği alanlar ve açıklamaları yer almaktadır:

Id: Yorumun benzersiz kimliğidir.
Content: Yorumun içeriğini temsil eder.
PostId: Yorumun ait olduğu gönderinin (post) kimliğidir.
UserId: Yorum ekleyen standart kullanıcının kimliğidir. (Opsiyonel)
AuthorId: Yorum ekleyen admin yazarın kimliğidir. (Opsiyonel)
CreatedAt: Yorumun oluşturulma tarihini tutar.
UserName: Yorumun yazarı olan kullanıcının adını içerir.
Bu DTO, genellikle yorumları görüntülemek için kullanılır ve gerekli bilgileri tek bir yapı altında toplar.
 */