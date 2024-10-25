using AutoMapper; // Nesne eşleme için AutoMapper kütüphanesi
using OnionProject.Application.Models.DTOs; // DTO sınıflarını kullanmak için gerekli olan isim alanı
using OnionProject.Application.Services.AbstractServices; // İletişim mesajı hizmetinin arayüzünü kullanmak için gerekli olan isim alanı
using OnionProject.Domain.AbstractRepositories; // Repository arayüzlerini kullanmak için gerekli olan isim alanı
using OnionProject.Domain.Entities; // Domain varlıklarını kullanmak için gerekli olan isim alanı
using System; // Temel sistem işlevleri için gerekli olan isim alanı
using System.Collections.Generic; // Koleksiyonlar için gerekli olan isim alanı
using System.Linq; // LINQ sorguları için gerekli olan isim alanı
using System.Text; // String işleme için gerekli olan isim alanı
using System.Threading.Tasks; // Asenkron programlama için gerekli olan isim alanı

namespace OnionProject.Application.Services.ConcreteManagers
{
    // İletişim mesajı hizmetinin uygulanması
    public class ContactMessageManager : IContactMessageService
    {
        private readonly IContactMessageRepo _repo; // İletişim mesajı repository'si
        private readonly IMapper _mapper; // AutoMapper nesnesi

        // Yapıcı metod, bağımlılık enjeksiyonu ile gerekli bileşenleri alır
        public ContactMessageManager(IContactMessageRepo repo, IMapper mapper)
        {
            _repo = repo; // Repository'yi atama
            _mapper = mapper; // Mapper'ı atama
        }

        // Yeni bir iletişim mesajı ekler (DTO ile)
        public async Task AddMessageAsync(CreateContactMessageDTO dto)
        {
            var message = _mapper.Map<ContactMessage>(dto); // DTO'yu ContactMessage nesnesine dönüştürme
            await _repo.AddAsync(message); // Repository üzerinden yeni mesajı ekleme
        }

        // Yeni bir iletişim mesajı ekler (ContactMessage nesnesi ile)
        public async Task AddMessageAsync(ContactMessage contactMessage)
        {
            await _repo.AddAsync(contactMessage); // Belirtilen ContactMessage nesnesini repository üzerinden ekleme
        }
    }
}
/*
    Açıklamalar
Namespace Kullanımı:

AutoMapper: Nesne eşleme işlemlerini gerçekleştirmek için kullanılan bir kütüphanedir.
OnionProject.Application.Models.DTOs: DTO (Data Transfer Object) sınıflarını içeren isim alanıdır.
OnionProject.Application.Services.AbstractServices: İletişim mesajı hizmetinin arayüzünü içeren isim alanıdır.
OnionProject.Domain.AbstractRepositories: Repository arayüzlerini içeren isim alanıdır.
OnionProject.Domain.Entities: Uygulamanın domain varlıklarını içeren isim alanıdır.
İşlevsellik:

ContactMessageManager Sınıfı: IContactMessageService arayüzünü uygulayan sınıf. İletişim mesajları ile ilgili işlemleri gerçekleştirir.

Yapıcı Metod:

IContactMessageRepo repo: İletişim mesajlarını yönetmek için kullanılan repository.
IMapper mapper: AutoMapper nesnesi, DTO ve varlıklar arasında dönüşüm işlemleri için kullanılır.
AddMessageAsync Metodu (DTO ile):

CreateContactMessageDTO dto: Yeni iletişim mesajı eklemek için gereken verileri içeren DTO nesnesi.
_mapper.Map<ContactMessage>(dto): DTO'yu ContactMessage nesnesine dönüştürür.
await _repo.AddAsync(message): Repository aracılığıyla yeni iletişim mesajını ekler.
AddMessageAsync Metodu (Nesne ile):

ContactMessage contactMessage: Eklenmek istenen ContactMessage nesnesi.
await _repo.AddAsync(contactMessage): Repository üzerinden belirtilen ContactMessage nesnesini ekler.
Genel Değerlendirme
ContactMessageManager sınıfı, iletişim mesajları ile ilgili temel işlemleri gerçekleştirmek için tasarlanmıştır. İki farklı AddMessageAsync metodu, biri DTO kullanarak diğeri doğrudan varlık nesnesi ile iletişim mesajı eklemeyi sağlar. Bu yapı, uygulamanın temiz ve modüler bir mimariye sahip olmasına yardımcı olur. AutoMapper kullanımı, veri transfer nesneleri ile domain varlıkları arasında dönüşüm yaparak kodun tekrarını azaltır ve okunabilirliği artırır.
 */