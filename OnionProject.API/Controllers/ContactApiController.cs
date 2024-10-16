using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.Entities;

namespace OnionProject.API.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ContactApiController : ControllerBase
    {
        private readonly IContactMessageService _contactService;

        public ContactApiController(IContactMessageService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContactMessage(CreateContactMessageDTO dto)
        {
            var contactMessage = new ContactMessage
            {
                Name = dto.Name,
                Email = dto.Email,
                Message = dto.Message,
                SubmittedDate = DateTime.Now,
            };

            await _contactService.AddMessageAsync(contactMessage);
            return Ok();
        }
    }
}
