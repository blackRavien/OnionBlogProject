using System; // Temel C# sınıf kütüphanesi
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    // İletişim mesajı oluşturmak için kullanılacak DTO sınıfı
    public class CreateContactMessageDTO
    {
        // Kullanıcının adını tutan alan
        public string Name { get; set; }

        // Kullanıcının e-posta adresini tutan alan
        public string Email { get; set; }

        // Kullanıcının iletmek istediği mesajı tutan alan
        public string Message { get; set; }
    }
}

/*
    Genel Özet:
CreateContactMessageDTO, iletişim formundan gelen kullanıcı mesajlarını toplamak amacıyla oluşturulmuş bir DTO'dur. Kullanıcının adını (Name), e-posta adresini (Email) ve iletmek istediği mesajı (Message) içeren alanlar bulunur. Bu DTO, kullanıcıların web sitesindeki iletişim formunu doldurup gönderdiklerinde, mesajlarını API'ye veya sunucuya taşıyan bir yapı görevi görür. Doğrulama eklenmemiş, ancak ihtiyaç duyulursa alanlar için Required veya EmailAddress gibi doğrulama eklenebilir.
 */