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
    // Genre entity'si için concrete repository sınıfı
    public class GenreRepo : BaseRepo<Genre>, IGenreRepo
    {
        // Constructor, DbContext'i dependency injection ile alır
        public GenreRepo(AppDbContext context) : base(context)
        {

        }

        // Tüm türleri getiren metot
        public async Task<List<Genre>> GetAll()
        {
            return await _context.Genres.ToListAsync(); // Tüm türleri veritabanından getirir
        }

        // ID'ye göre tür getiren metot
        public async Task<Genre> GetById(int id)
        {
            return await _context.Genres
                .Where(g => g.Id == id) // ID'ye göre tür filtreler
                .SingleOrDefaultAsync(); // Tek bir sonuç döner
        }

        // İsme göre tür getiren metot
        public async Task<Genre> GetByName(string name)
        {
            return await _context.Genres
                .Where(g => g.Name == name) // İsme göre tür filtreler
                .SingleOrDefaultAsync(); // Tek bir sonuç döner
        }

        // Belirli bir isme sahip türleri getiren özel metod
        public async Task<List<Genre>> GetGenresByName(string name)
        {
            return await _context.Genres
                .Where(g => g.Name.Contains(name)) // İsmi içeren türleri filtreler
                .ToListAsync(); // Sonuçları liste olarak döner
        }

        // İlgili postları olan türleri getiren metot
        public async Task<List<Genre>> GetGenresWithPosts()
        {
            return await _context.Genres
                .Include(g => g.Posts) // Postları dahil eder
                .ToListAsync(); // Sonuçları liste olarak döner
        }

        // Belirli bir türe ait postları getiren metot
        public async Task<List<Post>> GetPostsByGenre(int genreId)
        {
            return await _context.Posts
                .Where(p => p.GenreId == genreId) // GenreId'ye göre postları filtreler
                .ToListAsync(); // Sonuçları liste olarak döner
        }

        // Yeni bir tür ekleyen metot
        public async Task Add(Genre genre)
        {
            await _context.Genres.AddAsync(genre); // Yeni türü veritabanına ekler
            await _context.SaveChangesAsync(); // Değişiklikleri kaydeder
        }
    }
}
