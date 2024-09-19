using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.AbstractServices
{
    public interface IGenreService
    {
        Task<GenreVm> CreateGenre(CreateGenreDTO createGenreDTO);
        Task<GenreVm> UpdateGenre(UpdateGenreDTO updateGenreDTO);
        Task DeleteGenre(int id);
        Task<GenreVm> GetGenreById(int id);
        Task<List<GenreVm>> GetAllGenres();
        Task<GenreVm> GetGenreByName(string name);
        Task<List<PostVm>> GetPostsByGenre(int genreId);
    }
}
