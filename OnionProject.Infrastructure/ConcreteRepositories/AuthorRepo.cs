using Microsoft.EntityFrameworkCore;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using OnionProject.Infrastructure.ConcreteRepositories;
using OnionProject.Infrastructure.Context;

public class AuthorRepo : BaseRepo<Author>, IAuthorRepo
{
    public AuthorRepo(AppDbContext context) : base(context)
    {
    }

    // Tüm yazarları listeleme
    public async Task<List<Author>> GetAll()
    {
        return await _context.Authors.ToListAsync(); // Tüm yazarları veritabanından getirir
    }

    // Yazar ve yazara ait postlar ile birlikte listeleme
    public async Task<List<Author>> GetAuthorsWithPosts()
    {
        return await _context.Authors
            .Include(a => a.Posts) // Postları da dahil eder
            .ToListAsync(); // Sonuçları liste olarak döner
    }

    // Yazarın tam adına göre arama
    public async Task<Author> GetByFullName(string firstName, string lastName)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.FirstName == firstName && a.LastName == lastName); // Ad ve soyadına göre yazar arar
    }

    // ID'ye göre yazarı getirir
    public async Task<Author> GetById(int id)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == id); // ID'ye göre yazar arar
    }

    // Belirli bir yazara ait tüm postları getirir
    public async Task<List<Post>> GetPostsByAuthor(int authorId)
    {
        var author = await _context.Authors
            .Include(a => a.Posts) // Postları dahil eder
            .FirstOrDefaultAsync(a => a.Id == authorId); // ID'ye göre yazar arar

        return author?.Posts.ToList() ?? new List<Post>(); // Yazar varsa postları döner, yoksa boş liste
    }
}
