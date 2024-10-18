using OnionProject.Application.Models.DTOs;
using OnionProject.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.AbstractServices
{
    public interface IAppUserService
    {
        Task CreateUser(AppUser user);
        Task UpdateUser(AppUser user);
        Task DeleteUser(string userId);
        Task<AppUser> GetUserById(string userId);
        Task<AppUser> GetUserByUsername(string username);
        Task<AppUser> GetUserByEmail(string email);
        Task<List<AppUser>> GetAllUsers();
        Task<bool> UpdateUserProfileAsync(string? userId, ProfileUpdateDTO updateProfileDto);
    }
}
