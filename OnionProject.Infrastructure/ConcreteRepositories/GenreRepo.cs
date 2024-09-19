using Microsoft.EntityFrameworkCore;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using OnionProject.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.ConcreteRepositories
{
    public class GenreRepo : BaseRepo<Genre>, IGenreRepo
    {
        public GenreRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<Genre> GetById(int id)
        {
            return await _context.Genres
                .Where(g => g.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<Genre> GetByName(string name)
        {
            return await _context.Genres
                .Where(g => g.Name == name)
                .SingleOrDefaultAsync();
        }

        // Özel metodlar eklemek istiyorsanız buraya ekleyebilirsiniz.
        // Örneğin:

        // Belirli bir isme sahip türleri getiren bir metod
        public async Task<List<Genre>> GetGenresByName(string name)
        {
            return await _context.Genres
                .Where(g => g.Name.Contains(name))
                .ToListAsync();
        }

        // İlgili postları olan türleri getiren bir metod
        public async Task<List<Genre>> GetGenresWithPosts()
        {
            return await _context.Genres
                .Include(g => g.Posts)
                .ToListAsync();
        }

        public async Task<List<Post>> GetPostsByGenre(int genreId)
        {
            return await _context.Posts
                .Where(p => p.GenreId == genreId)
                .ToListAsync();
        }
    }
}
