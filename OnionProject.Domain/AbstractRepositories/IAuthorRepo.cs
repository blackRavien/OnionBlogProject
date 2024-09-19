using OnionProject.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionProject.Domain.AbstractRepositories
{
    public interface IAuthorRepo : IBaseRepo<Author>
    {
        // Tüm yazarları getirme
        Task<List<Author>> GetAll();

        // Yazarın ad ve soyadına göre yazar bulma
        Task<Author> GetByFullName(string firstName, string lastName);

        // ID'ye göre yazar bulma
        Task<Author> GetById(int id);

        // Yazarın yazılarını getirme
        Task<List<Post>> GetPostsByAuthor(int authorId);
    }
}
