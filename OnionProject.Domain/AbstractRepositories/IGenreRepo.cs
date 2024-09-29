using OnionProject.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionProject.Domain.AbstractRepositories
{
    public interface IGenreRepo : IBaseRepo<Genre>
    {
        // Tür adına göre türü getirme
        Task<Genre> GetByName(string name);

        // Tür ile ilişkili yazıları getirme
        Task<List<Post>> GetPostsByGenre(int genreId);

        Task<Genre> GetById(int id);
        Task<List<Genre>> GetAll();

        Task Add(Genre genre);

    }
}
