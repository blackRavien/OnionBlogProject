using Microsoft.EntityFrameworkCore;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using OnionProject.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.ConcreteManagers
{
    public class CommentManager : ICommentService
    {
        private readonly ICommentRepo _commentRepository;
        private readonly AppDbContext _context;
        public CommentManager(ICommentRepo commentRepository, AppDbContext context)
        {
            _commentRepository = commentRepository;
            _context = context;
        }

        public async Task AddCommentAsync(CreateCommentDTO commentDto)
        {
            var comment = new Comment
            {
                Content = commentDto.Content,
                PostId = commentDto.PostId,
                UserId = commentDto.UserId,
            };
            await _commentRepository.AddAsync(comment);
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            // Yorumu silmek için repository üzerinden silme işlemini gerçekleştir
            await _commentRepository.DeleteAsync(commentId);
        }


        
        public async Task<List<GetCommentDTO>> GetCommentsByPostIdAsync(int postId)
        {
            var comments = await _commentRepository.GetByPostIdAsync(postId);
            // Dönüşüm işlemi
            return comments.Select(c => new GetCommentDTO
            {
                Id = c.Id,
                Content = c.Content,
                PostId = c.PostId,
                UserId = c.UserId,
                CreatedAt = c.CreatedAt,
                UserName = _context.Users
                .Where(u => u.Id == c.UserId)
                .Select(u => u.UserName)
                .FirstOrDefault() // Kullanıcının UserName'ini getiriyoruz
            }).ToList();
        }
    }

}
