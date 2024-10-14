using AutoMapper;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.ConcreteManagers
{
    public class ContactMessageManager : IContactMessageService
    {
        private readonly IContactMessageRepo _repo;
        private readonly IMapper _mapper;

        public ContactMessageManager(IContactMessageRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task AddMessageAsync(CreateContactMessageDTO dto)
        {
            var message = _mapper.Map<ContactMessage>(dto);
            await _repo.AddAsync(message);
        }

        public async Task AddMessageAsync(ContactMessage contactMessage)
        {
            await _repo.AddAsync(contactMessage);
        }

        // Diğer işlemler...
    }
}
