using Microsoft.EntityFrameworkCore;  // Entity Framework Core ile ilgili sınıfları içerir
using OnionProject.Domain.AbstractRepositories;  // Generic repository arayüzünü içerir
using OnionProject.Domain.Entities;  // Domain entity'lerini içerir
using OnionProject.Infrastructure.ConcreteRepositories;  // Concrete repository sınıflarını içerir
using OnionProject.Infrastructure.Context;  // DbContext sınıfını içerir

// Author entity'si için concrete repository sınıfı
public class AuthorRepo : BaseRepo<Author>, IAuthorRepo
{
    // Constructor, DbContext'i dependency injection ile alır
    public AuthorRepo(AppDbContext context) : base(context)
    {
    }

    // Tüm yazarları listeleme metodu
    public async Task<List<Author>> GetAll()
    {
        return await _context.Authors.ToListAsync(); // Tüm yazarları veritabanından getirir
    }

    // Yazar ve yazara ait postlar ile birlikte listeleme metodu
    public async Task<List<Author>> GetAuthorsWithPosts()
    {
        return await _context.Authors
            .Include(a => a.Posts) // Postları da dahil eder
            .ToListAsync(); // Sonuçları liste olarak döner
    }

    // Yazarın tam adına göre arama metodu
    public async Task<Author> GetByFullName(string firstName, string lastName)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName); // Ad ve soyadına göre yazar arar
    }

    // ID'ye göre yazarı getiren metot
    public async Task<Author> GetById(int id)
    {
        return await _context.Authors
            .Include(a => a.Posts) // Postları dahil eder
            .FirstOrDefaultAsync(a => a.Id == id); // ID'ye göre yazar arar
    }

    // Belirli bir yazara ait tüm postları getiren metot
    public async Task<List<Post>> GetPostsByAuthor(int authorId)
    {
        var author = await _context.Authors
            .Include(a => a.Posts) // Postları dahil eder
            .FirstOrDefaultAsync(a => a.Id == authorId); // ID'ye göre yazar arar

        return author?.Posts.ToList() ?? new List<Post>(); // Yazar varsa postları döner, yoksa boş liste
    }
}
