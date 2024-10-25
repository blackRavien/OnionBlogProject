using OnionProject.Application.Models.DTOs; // DTO sınıflarını kullanmak için gerekli olan isim alanı
using OnionProject.Domain.Entities; // Domain varlıklarını kullanmak için gerekli olan isim alanı
using System; // Temel sistem işlevleri için gerekli olan isim alanı
using System.Collections.Generic; // Koleksiyonlar için gerekli olan isim alanı
using System.Linq; // LINQ sorguları için gerekli olan isim alanı
using System.Text; // String işleme için gerekli olan isim alanı
using System.Threading.Tasks; // Asenkron programlama için gerekli olan isim alanı

namespace OnionProject.Application.Services.AbstractServices
{
    // İletişim mesajları ile ilgili hizmetlerin arayüzü
    public interface IContactMessageService
    {
        // İletişim mesajını eklemek için metod
        Task AddMessageAsync(ContactMessage contactMessage);

        // İletişim mesajı DTO'sunu eklemek için metod
        Task AddMessageAsync(CreateContactMessageDTO dto);
    }
}

/*
    Açıklamalar
Namespace Kullanımı:

OnionProject.Application.Models.DTOs: DTO (Data Transfer Object) sınıflarını içeren isim alanıdır.
OnionProject.Domain.Entities: Uygulamanın domain varlıklarını içeren isim alanıdır.
Arayüz (Interface):

IContactMessageService: Bu arayüz, iletişim mesajları ile ilgili hizmetleri tanımlamak için kullanılır. Arayüz, bu tür hizmetlerin uygulanabilirliğini sağlar ve bağımlılıkların yönetilmesine yardımcı olur.
Metod Tanımları:

AddMessageAsync(ContactMessage contactMessage):

Bu metod, ContactMessage türünde bir nesne alarak, iletişim mesajını eklemeyi amaçlar.
Task: Asenkron bir işlem döndürdüğünü belirtir. Bu, metodun çağrıldığında hemen geri döneceği, ancak işlemin tamamlanmasının daha sonra olacağı anlamına gelir.
AddMessageAsync(CreateContactMessageDTO dto):

Bu metod, CreateContactMessageDTO türünde bir nesne alarak, iletişim mesajını eklemeyi amaçlar. DTO, genellikle veri taşıma işlemleri için kullanılır ve uygulamanın farklı katmanları arasında veri iletimini kolaylaştırır.
Task: Bu metodun da asenkron bir işlem döndürdüğünü belirtir.
Genel Değerlendirme
IContactMessageService arayüzü, iletişim mesajları ile ilgili temel işlevlerin uygulanmasını sağlayan bir sözleşmedir. Bu arayüz, iki farklı metodu tanımlayarak, hem doğrudan ContactMessage nesneleri ile hem de DTO kullanarak mesaj eklemeye olanak tanır. Bu yapı, uygulamanın modülerliğini ve test edilebilirliğini artırır. Bağımlılık enjeksiyonu kullanılarak, bu arayüzün uygulanması ve yönetimi daha esnek hale gelir. Bu sayede, farklı uygulama gereksinimleri için kolayca farklı implementasyonlar oluşturulabilir.
 */