using OnionProject.Application.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.AbstractServices
{
    public interface ICommentService
    {
        Task AddCommentAsync(CreateCommentDTO commentDto);
        Task<List<GetCommentDTO>> GetCommentsByPostIdAsync(int postId);
    }

}
