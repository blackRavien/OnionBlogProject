using Microsoft.EntityFrameworkCore;  // Entity Framework Core sınıflarını içerir
using OnionProject.Domain.AbstractRepositories;  // Generic repository arayüzünü içerir
using OnionProject.Domain.Entities;  // Domain entity'lerini içerir
using OnionProject.Infrastructure.Context;  // DbContext sınıfını içerir
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.ConcreteRepositories
{
    // Post entity'si için concrete repository sınıfı
    public class PostRepo : BaseRepo<Post>, IPostRepo
    {
        // Constructor, DbContext'i dependency injection ile alır
        public PostRepo(AppDbContext context) : base(context)
        {
        }

        // Belirli bir yazarın yazılarını getiren metot
        public async Task<List<Post>> GetPostsByAuthor(int authorId)
        {
            return await _context.Posts
                .Where(p => p.AuthorId == authorId) // AuthorId'ye göre postları filtreler
                .ToListAsync(); // Sonuçları liste olarak döner
        }

        // Belirli bir türdeki yazıları getiren metot
        public async Task<List<Post>> GetPostsByGenre(int genreId)
        {
            return await _context.Posts
                .Where(p => p.GenreId == genreId) // GenreId'ye göre postları filtreler
                .ToListAsync(); // Sonuçları liste olarak döner
        }

        // Yayınlanma tarihi aralığına göre yazıları getiren metot
        public async Task<List<Post>> GetPostsByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Posts
                .Where(p => p.CreatedDate >= startDate && p.CreatedDate <= endDate) // Tarih aralığına göre filtreler
                .ToListAsync(); // Sonuçları liste olarak döner
        }

        // Yazar ve tür bilgilerini içeren yazıları getiren metot
        public async Task<List<Post>> GetPostsWithAuthorAndGenre()
        {
            return await _context.Posts
                .Include(p => p.Author) // Yazar bilgisini dahil eder
                .Include(p => p.Genre)  // Tür bilgisini dahil eder
                .ToListAsync(); // Sonuçları liste olarak döner
        }

        // Başlığa göre yazıları getiren metot
        public async Task<List<Post>> GetPostsByTitle(string title)
        {
            return await _context.Posts
                .Where(p => p.Title.Contains(title)) // Başlık içinde geçen postları filtreler
                .ToListAsync(); // Sonuçları liste olarak döner
        }

        // Tüm postları getirir
        public async Task<List<Post>> GetAll()
        {
            return await _context.Posts
                .Include(p => p.Author) // Yazar bilgisini dahil eder
                .Include(p => p.Genre)  // Tür bilgisini dahil eder
                .ToListAsync(); // Sonuçları liste olarak döner
        }

        // ID'ye göre post getirir
        public async Task<Post> GetById(int id)
        {
            return await _context.Posts
                .Include(p => p.Author) // Yazar ilişkisini dahil eder
                .Include(p => p.Genre)  // Tür ilişkisini dahil eder
                .FirstOrDefaultAsync(p => p.Id == id); // ID'ye göre post arar
        }
    }
}
