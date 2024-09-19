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
    public class PostRepo : BaseRepo<Post>, IPostRepo
    {
        public PostRepo(AppDbContext context) : base(context)
        {
        }

        // Özel metodlar ekleyebilirsiniz.
        // Örneğin:

        // Belirli bir yazarın yazılarını getiren bir metod
        public async Task<List<Post>> GetPostsByAuthor(int authorId)
        {
            return await _context.Posts
                .Where(p => p.AuthorId == authorId)
                .ToListAsync();
        }

        // Belirli bir türdeki yazıları getiren bir metod
        public async Task<List<Post>> GetPostsByGenre(int genreId)
        {
            return await _context.Posts
                .Where(p => p.GenreId == genreId)
                .ToListAsync();
        }

        // Yayınlanma tarihi aralığına göre yazıları getiren bir metod
        public async Task<List<Post>> GetPostsByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.Posts
                .Where(p => p.CreatedDate >= startDate && p.CreatedDate <= endDate)
                .ToListAsync();
        }

        // İlgili yazar ve türü içeren yazıları getiren bir metod
        public async Task<List<Post>> GetPostsWithAuthorAndGenre()
        {
            return await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Genre)
                .ToListAsync();
        }

        // Başlığa göre postları getirir
        public async Task<List<Post>> GetPostsByTitle(string title)
        {
            return await _context.Posts
                .Where(p => p.Title.Contains(title))
                .ToListAsync();
        }

        // Tüm postları getirir
        public async Task<List<Post>> GetAll()
        {
            return await _context.Posts
                .ToListAsync();
        }

        // Belirli bir ID'ye sahip postu getirir
        public async Task<Post> GetById(int id)
        {
            return await _context.Posts
                .FindAsync(id);
        }
    }
}
