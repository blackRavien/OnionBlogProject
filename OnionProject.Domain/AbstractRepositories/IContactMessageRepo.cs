using OnionProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Domain.AbstractRepositories
{
    public interface IContactMessageRepo : IBaseRepo<ContactMessage>
    {
        Task AddAsync(ContactMessage message);
    }
}
