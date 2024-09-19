using AutoMapper;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.AutoMapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Create Mapping
            CreateMap<Author, CreateAuthorDTO>().ReverseMap();
            CreateMap<Post, CreatePostDTO>().ReverseMap();
            CreateMap<Genre, CreateGenreDTO>().ReverseMap();
            

            //Update Mapping
            CreateMap<AppUser, UpdateAppUserDTO>().ReverseMap();
            CreateMap<Author, UpdateAuthorDTO>().ReverseMap();
            CreateMap<Genre, UpdateGenreDTO>().ReverseMap();
            CreateMap<Post, UpdatePostDTO>().ReverseMap();

            //Register and Login Mapping
            CreateMap<AppUser, RegisterAppUserDTO>().ReverseMap();
            CreateMap<AppUser, LoginDTO>().ReverseMap();


            //VMS
            CreateMap<Author, AuthorDetailVm>().ReverseMap();
            CreateMap<Author, AuthorVm>().ReverseMap();
            CreateMap<Genre, GenreVm>().ReverseMap();
            CreateMap<Post, GetPostsVm>().ReverseMap();
            CreateMap<Post, PostDetailsVm>().ReverseMap();
            CreateMap<Post, PostVm>().ReverseMap();
            CreateMap<AppUser, RegisterVm>().ReverseMap();
            CreateMap<AppUser, LoginVm>().ReverseMap();

        }
        
    }
}
