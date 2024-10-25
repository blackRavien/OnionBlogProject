using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs; // DTO modellerinin bulunduğu namespace
using OnionProject.Application.Services.AbstractServices; // Hizmet arayüzlerinin bulunduğu namespace
using OnionProject.Domain.Entities; // Domain katmanındaki varlıkların bulunduğu namespace

namespace OnionProject.API.Controllers
{
    // API kontrolör sınıfı, HTTP isteklerini karşılamak için özellikler içerir.
    [ApiController] // Bu sınıfın bir API kontrolörü olduğunu belirtir.
    [Route("api/[controller]")] // İstemci isteklerinin yönlendirilmesini sağlamak için URL rota tanımı.
    public class ContactApiController : ControllerBase
    {
        // IContactMessageService arayüzüne ait bir örneği tutan değişken.
        private readonly IContactMessageService _contactService;

        // Dependency Injection ile IContactMessageService örneğini alır.
        public ContactApiController(IContactMessageService contactService)
        {
            _contactService = contactService;
        }

        // İstemciden gelen iletişim mesajını kaydeden HTTP POST metodu.
        [HttpPost]
        public async Task<IActionResult> CreateContactMessage(CreateContactMessageDTO dto)
        {
            // DTO'dan ContactMessage varlığı oluştur.
            var contactMessage = new ContactMessage
            {
                Name = dto.Name, // İsim
                Email = dto.Email, // E-posta
                Message = dto.Message, // Mesaj
                SubmittedDate = DateTime.Now, // Gönderim tarihi
            };

            // Mesajı asenkron olarak kaydet.
            await _contactService.AddMessageAsync(contactMessage);

            // Başarılı bir yanıt döndür.
            return Ok();
        }
    }
}

/*
    Özet
ContactApiController sınıfı, iletişim mesajlarını almak ve kaydetmek için bir API kontrolörüdür. IContactMessageService arayüzünü kullanarak iletişim mesajlarını veritabanına ekleyen bir CreateContactMessage metodu içerir. Bu metod, bir CreateContactMessageDTO nesnesi alır ve içindeki bilgileri kullanarak bir ContactMessage varlığı oluşturur. Oluşturulan bu varlık, hizmet katmanı aracılığıyla asenkron olarak veritabanına kaydedilir. Metod, başarılı bir işlem sonrasında HTTP 200 OK yanıtı döndürür.
 */
