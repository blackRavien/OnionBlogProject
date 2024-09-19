using AutoMapper;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.ConcreteManagers
{
    public class GenreManager : IGenreService
    {
        private readonly IGenreRepo _genreRepo;
        private readonly IMapper _mapper;

        public GenreManager(IGenreRepo genreRepo, IMapper mapper)
        {
            _genreRepo = genreRepo;
            _mapper = mapper;
        }

        public async Task<GenreVm> CreateGenre(CreateGenreDTO createGenreDTO)
        {
            var genre = _mapper.Map<Genre>(createGenreDTO);
            await _genreRepo.Create(genre);
            return _mapper.Map<GenreVm>(genre);
        }

        public async Task<GenreVm> UpdateGenre(UpdateGenreDTO updateGenreDTO)
        {
            var genre = await _genreRepo.GetById(updateGenreDTO.Id);
            if (genre == null) return null;
            _mapper.Map(updateGenreDTO, genre);
            await _genreRepo.Update(genre);
            return _mapper.Map<GenreVm>(genre);
        }

        public async Task DeleteGenre(int id)
        {
            var genre = await _genreRepo.GetById(id);
            if (genre != null)
            {
                await _genreRepo.Delete(genre);
            }
        }

        public async Task<GenreVm> GetGenreById(int id)
        {
            var genre = await _genreRepo.GetById(id);
            return _mapper.Map<GenreVm>(genre);
        }

        public async Task<List<GenreVm>> GetAllGenres()
        {
            var genres = await _genreRepo.GetDefaults(x => true);
            return _mapper.Map<List<GenreVm>>(genres);
        }

        public async Task<GenreVm> GetGenreByName(string name)
        {
            var genre = await _genreRepo.GetByName(name);
            return _mapper.Map<GenreVm>(genre);
        }

        public async Task<List<PostVm>> GetPostsByGenre(int genreId)
        {
            var posts = await _genreRepo.GetPostsByGenre(genreId);
            return _mapper.Map<List<PostVm>>(posts);
        }
    }
}
