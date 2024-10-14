using OnionProject.Application.Models.DTOs;
using OnionProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.AbstractServices
{
    public interface IContactMessageService
    {
        Task AddMessageAsync(ContactMessage contactMessage);
        Task AddMessageAsync(CreateContactMessageDTO dto);
    }
}
