using Microsoft.EntityFrameworkCore.Query;
using OnionProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnionProject.Domain.AbstractRepositories
{
    public interface IAppUserRepo : IBaseRepo<AppUser>
    {
        // Kullanıcıyı kullanıcı adıyla bulma
        Task<AppUser> GetByUsername(string username);

        // Kullanıcıyı e-posta adresi ile bulma
        Task<AppUser> GetByEmail(string email);
        Task<AppUser> GetById(string userId);
        Task<List<AppUser>> GetAll();
    }
}
