using OnionProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Domain.AbstractRepositories
{
    public interface ICommentRepo : IBaseRepo<Comment>
    {
        // Belirli bir post ID'ye göre yorumları getir
        Task<List<Comment>> GetByPostIdAsync(int postId);

        // Yorum ekleme metodu
        Task AddAsync(Comment comment);

        // Yorum güncelleme metodu
        Task UpdateAsync(Comment comment);

        // Yorum silme metodu
        Task DeleteAsync(int id);

        // Belirli bir kullanıcıya göre yorumları getir
        Task<List<Comment>> GetByUserIdAsync(string userId);

        // Tüm yorumları getir
        Task<List<Comment>> GetAllCommentsAsync();
        Task<string?> GetCommentByIdAsync(int id);
    }


}
