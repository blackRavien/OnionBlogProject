using OnionProject.Domain.Entities;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Application.Services.AbstractServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.ConcreteManagers
{
    public class AppUserManager : IAppUserService
    {
        private readonly IAppUserRepo _appUserRepo;

        public AppUserManager(IAppUserRepo appUserRepo)
        {
            _appUserRepo = appUserRepo;
        }

        public async Task CreateUser(AppUser user)
        {
            await _appUserRepo.Create(user);
        }

        public async Task UpdateUser(AppUser user)
        {
            await _appUserRepo.Update(user);
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _appUserRepo.GetById(userId);
            if (user != null)
            {
                await _appUserRepo.Delete(user);
            }
        }

        public async Task<AppUser> GetUserById(string userId)
        {
            return await _appUserRepo.GetById(userId);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _appUserRepo.GetByUsername(username);
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _appUserRepo.GetByEmail(email);
        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            // Tüm kullanıcıları getiren bir metod yazmalısınız
            // Eğer repo'da böyle bir metod yoksa, bir metod eklemelisiniz.
            return await _appUserRepo.GetAll(); // Bu metodun repo'da olması gerekiyor.
        }
    }
}
