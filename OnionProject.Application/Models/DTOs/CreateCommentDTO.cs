using System; // Temel C# sınıflarını içerir
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Model doğrulama için gerekli sınıflar
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    // Yorum oluşturma işlemi sırasında kullanılacak DTO sınıfı
    public class CreateCommentDTO
    {
        // Yorum içeriğinin zorunlu olduğunu belirtiyoruz
        [Required(ErrorMessage = "Yorum içeriği zorunludur.")]
        public string Content { get; set; } // Yorumun metinsel içeriği

        // Hangi gönderiye (posta) yorum yapıldığını belirtmek için kullanılan alan
        public int PostId { get; set; } // Yorumun yapıldığı postun ID'si

        // Eğer yorumu bir admin yazar ekliyorsa, bu alanda yazarın ID'si tutulur
        public int? AuthorId { get; set; } // Admin kullanıcı için yazar ID'si (opsiyonel)

        // Eğer yorumu standart bir kullanıcı ekliyorsa, bu alanda kullanıcının ID'si tutulur
        public string? UserId { get; set; } // Standart kullanıcı için user ID'si (opsiyonel)

        // Yorum yazanın kullanıcı adını tutmak için bir alan. Ancak şu an kullanılmıyor, isteğe bağlı eklenebilir.
        //public string UserName { get; set; }
    }
}

/*
    Genel Özet:
Bu CreateCommentDTO sınıfı, yeni bir yorum oluşturmak için kullanılan DTO'dur. İçerisinde yorumun içeriği, hangi gönderiye yapıldığı (PostId), yorumu ekleyen kişinin admin mi yoksa standart kullanıcı mı olduğunu belirlemek için AuthorId ve UserId gibi bilgiler bulunur. DataAnnotations kullanılarak yorum içeriğinin zorunlu olduğu belirtilmiş ve doğru verilerin girilmesi sağlanmıştır. Bu DTO, yorum ekleme işlemlerinde kullanıcının API'ye veri göndermesi için bir veri taşıyıcı olarak görev yapar.
 */