using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using System.Net.Http;

namespace OnionProject.MVC.Controllers
{
    public class ContactController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string uri = "https://localhost:7296"; // API URL'si

        public ContactController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactMessage(CreateContactMessageDTO model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("ContactUs", "Home");

            var response = await _httpClient.PostAsJsonAsync($"{uri}/api/ContactApi", model);


            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Mesajiniz alinmistir. Bize ulastiginiz icin tesekkur ederiz!";
                return RedirectToAction("ContactUs", "Home");
            }
                
            else
            {
                TempData["ErrorMessage"] = "Mesaj gönderilirken bir hata oluştu.";
                return RedirectToAction("ContactUs", "Home"); // Hata yönetimi
            }
                
        }
    }
}
