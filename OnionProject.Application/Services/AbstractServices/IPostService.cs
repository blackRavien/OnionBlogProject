using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.AbstractServices
{
    public interface IPostService
    {
        Task Create(CreatePostDTO model);
        Task Update(UpdatePostDTO model);
        Task Delete(int id);
        Task<List<PostVm>> GetPosts();
        Task<PostDetailsVm> GetDetail(int id);
        Task<UpdatePostDTO> GetById(int id);
        Task<List<PostVm>> GetPostsByTitle(string title);
        Task<List<PostVm>> GetPostsByAuthor(int authorId);
        Task<List<PostVm>> GetPostsByGenre(int genreId);
        Task<List<PostVm>> GetPostsByDateRange(DateTime startDate, DateTime endDate);
        Task<List<PostVm>> GetPostsWithAuthorAndGenre();
    }
}
