using Microsoft.EntityFrameworkCore;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using OnionProject.Infrastructure.Context;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.ConcreteRepositories
{
    public class CommentRepo : BaseRepo<Comment>, ICommentRepo
    {
        private readonly AppDbContext _context;

        public CommentRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Yorum ekleme metodu
        public async Task AddAsync(Comment comment)
        {
            // Yeni bir yorumu veri tabanına ekler
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync(); // Değişiklikleri kaydet
        }

        // Yorum silme metodu
        public async Task DeleteAsync(int id)
        {
            // Verilen ID'ye sahip yorumu bulur
            var comment = await _context.Comments.FindAsync(id);

            if (comment != null)
            {
                _context.Comments.Remove(comment); // Yorumu siler
                await _context.SaveChangesAsync(); // Değişiklikleri kaydet
            }
        }

        // Tüm yorumları getiren metot (Admin panelinde kullanılabilir)
        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            // Tüm yorumları döndürür
            return await _context.Comments.ToListAsync();
        }

        // Belirli bir post (gönderi) ID'sine göre yorumları getiren metot
        public async Task<List<Comment>> GetByPostIdAsync(int postId)
        {
            // Belirli bir gönderiye ait tüm yorumları getirir
            return await _context.Comments
                .Where(c => c.PostId == postId)
                .ToListAsync();
        }

        // Belirli bir kullanıcıya göre yorumları getiren metot
        public async Task<List<Comment>> GetByUserIdAsync(string userId)
        {
            // Belirtilen kullanıcıya ait tüm yorumları getirir
            return await _context.Comments
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<string?> GetCommentByIdAsync(int id)
        {
            // Belirtilen ID ile eşleşen yorumu veritabanından bul
            var comment = await _context.Comments
                .Where(c => c.Id == id)
                .Select(c => c.Content) // Yalnızca içerik (string) döndür
                .FirstOrDefaultAsync();

            return comment; // Eğer bulunursa içeriği döndür, yoksa null döner
        }




        // Yorum güncelleme metodu
        public async Task UpdateAsync(Comment comment)
        {
            // Yorumun durumunu "Modified" olarak işaretler ve kaydeder
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync(); // Değişiklikleri kaydet
        }
    }


}
