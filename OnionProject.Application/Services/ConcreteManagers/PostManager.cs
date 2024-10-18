using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using OnionProject.Infrastructure.Context;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.ConcreteManagers
{
    public class PostManager : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IPostRepo _postRepo;
        private readonly AppDbContext _appDbContext;
        private readonly IAuthorService _authorService;

        public PostManager(IMapper mapper, IPostRepo postRepo, IAuthorService authorService)
        {
            _mapper = mapper;
            _postRepo = postRepo;
            _authorService = authorService;
        }

        public async Task Create(CreatePostDTO model)
        {
            var post = _mapper.Map<Post>(model);

            if (model.UploadPath is not null)
            {
                using var image = Image.Load(model.UploadPath.OpenReadStream());
                image.Mutate(x => x.Resize(600, 500));
                Guid guid = Guid.NewGuid();
                image.Save($"wwwroot/images/{guid}.jpg");
                post.ImagePath = $"/images/{guid}.jpg";
            }
            else
            {
                post.ImagePath = $"/images/defaultPhoto.jpg";
            }

            await _postRepo.Create(post);
        }

        public async Task Update(UpdatePostDTO model)
        {
            // Öncelikle mevcut postu bul
            var post = await _postRepo.GetById(model.Id);
            if (post == null)
            {
                throw new Exception("Post bulunamadı."); // Post yoksa hata at
            }

            // Modelden gelen verileri mevcut post nesnesine aktar
            _mapper.Map(model, post);

            if (model.UploadPath != null)
            {
                using var stream = model.UploadPath.OpenReadStream();
                using var image = Image.Load(stream);
                image.Mutate(x => x.Resize(600, 500));
                Guid guid = Guid.NewGuid();
                image.Save($"wwwroot/images/{guid}.jpg");
                post.ImagePath = $"/images/{guid}.jpg";
            }

            await _postRepo.Update(post);
        }


        public async Task Delete(int id)
        {
            var post = await _postRepo.GetById(id);
            if (post != null)
            {
                await _postRepo.Delete(post);
            }
        }

        public async Task<List<PostVm>> GetPosts()
        {
            var posts = await _postRepo.GetAll();
            return _mapper.Map<List<PostVm>>(posts);
        }

        public async Task<PostDetailsVm> GetDetail(int id)
        {
            var post = await _postRepo.GetById(id);
            if (post == null)
            {
                throw new Exception("Post bulunamadı.");
            }

            // Yazar bilgilerini yüklemek için Include kullanın
            var author = await _authorService.GetDetail(post.AuthorId); // Yazar bilgilerini al
            if (author == null)
            {
                throw new Exception($"Yazar bulunamadı: {post.AuthorId}");
            }

            // PostDetailsVm oluşturma
            var postDetailsVm = _mapper.Map<PostDetailsVm>(post);

            // Post resmi için tam URL oluşturma
            postDetailsVm.ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}";

            // Yazar bilgilerini ve resmini ekle
            postDetailsVm.AuthorDetailVm = author;
            postDetailsVm.AuthorDetailVm.ImagePath = $"https://localhost:7296/{author.ImagePath.TrimStart('/')}";

            return postDetailsVm;
        }


        public async Task<UpdatePostDTO> GetById(int id)
        {
            var post = await _postRepo.GetById(id);
            return _mapper.Map<UpdatePostDTO>(post);
        }

        public async Task<List<PostVm>> GetPostsByTitle(string title)
        {
            var posts = await _postRepo.GetPostsByTitle(title);
            return _mapper.Map<List<PostVm>>(posts);
        }

        public async Task<List<PostVm>> GetPostsByAuthor(int authorId)
        {
            var posts = await _postRepo.GetPostsByAuthor(authorId);
            return _mapper.Map<List<PostVm>>(posts);
        }

        public async Task<List<PostVm>> GetPostsByGenre(int genreId)
        {
            var posts = await _postRepo.GetPostsByGenre(genreId);
            return _mapper.Map<List<PostVm>>(posts);
        }

        public async Task<List<PostVm>> GetPostsByDateRange(DateTime startDate, DateTime endDate)
        {
            var posts = await _postRepo.GetPostsByDateRange(startDate, endDate);
            return _mapper.Map<List<PostVm>>(posts);
        }

        public async Task<List<PostVm>> GetPostsWithAuthorAndGenre()
        {
            var posts = await _postRepo.GetPostsWithAuthorAndGenre();
            return _mapper.Map<List<PostVm>>(posts);
        }

       

    }
}
