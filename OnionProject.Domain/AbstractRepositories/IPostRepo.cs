using OnionProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionProject.Domain.AbstractRepositories
{
    public interface IPostRepo : IBaseRepo<Post>
    {
        // Başlığa göre postları getirir
        Task<List<Post>> GetPostsByTitle(string title);

        // Belirli bir yazara ait postları getirir
        Task<List<Post>> GetPostsByAuthor(int authorId);

        // Belirli bir türe ait postları getirir
        Task<List<Post>> GetPostsByGenre(int genreId);

        // Yayınlanma tarihi aralığına göre postları getirir
        Task<List<Post>> GetPostsByDateRange(DateTime startDate, DateTime endDate);

        // İlgili yazar ve tür ile postları getirir
        Task<List<Post>> GetPostsWithAuthorAndGenre();

        //Tüm postları getirir
        Task<List<Post>> GetAll();

        //Id'lerine göre Post'ları getirir.
        Task<Post> GetById(int id);
    }
}
