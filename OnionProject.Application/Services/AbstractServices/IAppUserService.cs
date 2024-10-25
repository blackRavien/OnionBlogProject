using OnionProject.Application.Models.DTOs;
using OnionProject.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.AbstractServices
{
    public interface IAppUserService
    {
        // Kullanıcı oluşturma
        Task CreateUser(AppUser user);

        // Kullanıcı güncelleme
        Task UpdateUser(AppUser user);

        // Kullanıcı silme
        Task DeleteUser(string userId);

        // ID ile kullanıcı alma
        Task<AppUser> GetUserById(string userId);

        // Kullanıcı ad ile kullanıcı alma
        Task<AppUser> GetUserByUsername(string username);

        // E-posta ile kullanıcı alma
        Task<AppUser> GetUserByEmail(string email);

        // Tüm kullanıcıları alma
        Task<List<AppUser>> GetAllUsers();

        // Kullanıcı profilini güncelleme
        Task<bool> UpdateUserProfileAsync(string? userId, ProfileUpdateDTO updateProfileDto);
    }
}

/*
    Genel Özet:
IAppUserService, kullanıcı işlemlerini yönetmek için gerekli olan metodları tanımlayan bir arayüzdür. Bu arayüz, kullanıcıların oluşturulması, güncellenmesi, silinmesi ve alınması gibi işlemleri gerçekleştirmek için kullanılacaktır.

Metodlar:
CreateUser(AppUser user): Yeni bir kullanıcı oluşturmak için kullanılacak metot. Parametre olarak bir AppUser nesnesi alır ve kullanıcının veritabanına eklenmesini sağlar.

UpdateUser(AppUser user): Mevcut bir kullanıcının bilgilerini güncellemek için kullanılır. Parametre olarak güncellenecek AppUser nesnesi alır.

DeleteUser(string userId): Verilen kullanıcı ID'sine göre kullanıcıyı siler. Kullanıcıyı silme işlemi için bir kullanıcı ID'si parametre olarak alınır.

GetUserById(string userId): Kullanıcı ID'si ile belirli bir kullanıcıyı almak için kullanılır. Bu metot, verilen ID'ye karşılık gelen AppUser nesnesini döner.

GetUserByUsername(string username): Kullanıcı adını kullanarak kullanıcıyı almak için kullanılır. Belirli bir kullanıcı adına sahip AppUser nesnesini döner.

GetUserByEmail(string email): E-posta adresine göre kullanıcıyı almak için kullanılır. Verilen e-posta ile ilişkili AppUser nesnesini döner.

GetAllUsers(): Tüm kullanıcıların listesini almak için kullanılır. Bir List<AppUser> döner.

UpdateUserProfileAsync(string? userId, ProfileUpdateDTO updateProfileDto): Kullanıcı profilini güncellemek için kullanılır. Kullanıcının ID'sini ve güncellemeleri içeren bir DTO alır. Güncelleme başarılı ise true, aksi halde false döner.
 */