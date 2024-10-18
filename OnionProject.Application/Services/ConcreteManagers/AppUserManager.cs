using OnionProject.Domain.Entities;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Application.Services.AbstractServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnionProject.Application.Models.DTOs;
using Microsoft.AspNetCore.Identity;

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


        public async Task<bool> UpdateUserProfileAsync(string? userId, ProfileUpdateDTO updateProfileDto)
        {
            // Kullanıcı ID'si boşsa false döner
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            // Kullanıcıyı veritabanından al
            var user = await _appUserRepo.GetById(userId);

            // Eğer kullanıcı bulunamazsa false döner
            if (user == null)
            {
                return false;
            }

            // Profil bilgilerini güncelle
            user.FirstName = updateProfileDto.FirstName;
            user.LastName = updateProfileDto.LastName;
            user.Email = updateProfileDto.Email; // E-posta
            user.PhoneNumber = updateProfileDto.PhoneNumber; // Telefon numarası

            // Şifre güncelleme kontrolü
            if (!string.IsNullOrEmpty(updateProfileDto.Password))
            {
                // Şifreyi hash'le ve güncelle
                var passwordHasher = new PasswordHasher<AppUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, updateProfileDto.Password);
            }

            // Kullanıcıyı güncelle
            await _appUserRepo.Update(user);

            // Güncellemenin başarılı olup olmadığını kontrol et
            return true;
        }



        //public async Task<bool> UpdateUserProfileAsync(string? userId, ProfileUpdateDTO updateProfileDto)
        //{
        //    // Kullanıcı ID'si boşsa false döner
        //    if (string.IsNullOrEmpty(userId))
        //    {
        //        return false;
        //    }

        //    // Kullanıcıyı veritabanından al
        //    var user = await _appUserRepo.GetById(userId);

        //    // Eğer kullanıcı bulunamazsa false döner
        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    // Profil bilgilerini güncelle
        //    user.FirstName = updateProfileDto.FirstName;
        //    user.LastName = updateProfileDto.LastName;
        //    user.PasswordHash = updateProfileDto.Password;
        //    user.Email = updateProfileDto.Email; // E-posta
        //    user.PhoneNumber = updateProfileDto.PhoneNumber; // Telefon numarası


        //    // Kullanıcıyı güncelle
        //    await _appUserRepo.Update(user);

        //    // Güncellemenin başarılı olup olmadığını kontrol et
        //    return true;
        //}


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
           // return await _appUserRepo.GetById(userId);

            var user = await _appUserRepo.GetById(userId);

            if (user == null)
            {
                // Kullanıcı bulunamadı
                return null;
            }

            return user;
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
