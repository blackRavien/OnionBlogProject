using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
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

        public CommentManager(ICommentRepo commentRepository)
        {
            _commentRepository = commentRepository;
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
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }

}
