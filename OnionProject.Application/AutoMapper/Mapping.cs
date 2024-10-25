using AutoMapper;
using OnionProject.Application.Models.DTOs; // DTO sınıflarını kullanabilmek için dahil edilen namespace
using OnionProject.Application.Models.VMs;  // ViewModel (VM) sınıflarını kullanmak için eklenmiş namespace
using OnionProject.Domain.Entities;         // Domain katmanındaki entity sınıflarını kullanmak için eklenen namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.AutoMapper // AutoMapper işlemleri için ayrı bir namespace
{
    public class Mapping : Profile // AutoMapper'dan gelen Profile sınıfından türetilen Mapping sınıfı
    {
        public Mapping() // Mapping sınıfının yapıcı metodu
        {
            // Entity'ler ile DTO'lar arasında çift yönlü (ReverseMap) mapping işlemi yapıyoruz
            // Create Mapping
            CreateMap<Author, CreateAuthorDTO>().ReverseMap(); // Author entity'si ile CreateAuthorDTO arasında çift yönlü map işlemi
            CreateMap<Post, CreatePostDTO>().ReverseMap(); // Post entity'si ile CreatePostDTO arasında map işlemi
            CreateMap<Genre, CreateGenreDTO>().ReverseMap(); // Genre entity'si ile CreateGenreDTO arasında map işlemi
            CreateMap<Comment, CreateCommentDTO>().ReverseMap(); // Comment entity'si ile CreateCommentDTO arasında map işlemi

            // Update Mapping
            CreateMap<AppUser, UpdateAppUserDTO>().ReverseMap(); // AppUser entity'si ile UpdateAppUserDTO arasında map işlemi
            CreateMap<Author, UpdateAuthorDTO>().ReverseMap(); // Author entity'si ile UpdateAuthorDTO arasında map işlemi
            CreateMap<Genre, UpdateGenreDTO>().ReverseMap(); // Genre entity'si ile UpdateGenreDTO arasında map işlemi
            CreateMap<Post, UpdatePostDTO>().ReverseMap(); // Post entity'si ile UpdatePostDTO arasında map işlemi

            // Register and Login Mapping
            // Kullanıcı kayıt ve giriş işlemleri için map'lemeler
            CreateMap<AppUser, RegisterAppUserDTO>().ReverseMap(); // AppUser ile RegisterAppUserDTO arasında map işlemi
            CreateMap<AppUser, LoginDTO>().ReverseMap(); // AppUser ile LoginDTO arasında map işlemi

            // ViewModel (VM) Mapping
            // Kullanıcı arayüzünde kullanılan view model'ler ile entity'ler arasında mapping işlemleri
            CreateMap<Author, AuthorDetailVm>().ReverseMap(); // Author entity'si ile AuthorDetailVm arasında map işlemi
            CreateMap<Author, AuthorVm>().ReverseMap(); // Author entity'si ile AuthorVm arasında map işlemi
            CreateMap<Genre, GenreVm>().ReverseMap(); // Genre entity'si ile GenreVm arasında map işlemi
            CreateMap<Post, GetPostsVm>().ReverseMap(); // Post entity'si ile GetPostsVm arasında map işlemi
            CreateMap<Post, PostDetailsVm>().ReverseMap(); // Post entity'si ile PostDetailsVm arasında map işlemi
            CreateMap<Post, PostVm>().ReverseMap(); // Post entity'si ile PostVm arasında map işlemi
            CreateMap<AppUser, RegisterVm>().ReverseMap(); // AppUser entity'si ile RegisterVm arasında map işlemi
            CreateMap<AppUser, LoginVm>().ReverseMap(); // AppUser entity'si ile LoginVm arasında map işlemi
            CreateMap<Comment, CommentVm>().ReverseMap(); // Comment entity'si ile CommentVm arasında map işlemi
            CreateMap<UpdatePostDTO, PostDetailsVm>().ReverseMap(); // UpdatePostDTO ile PostDetailsVm arasında map işlemi
        }
    }
}

/*
    Genel Özet:
Bu Mapping.cs dosyası, AutoMapper kullanarak entity'ler (veritabanı nesneleri) ile DTO'lar (Data Transfer Objects) ve ViewModel'ler (UI katmanı için modeller) arasında mapping işlemlerini gerçekleştirir. ReverseMap kullanarak çift yönlü mapping yapılmış, yani entity'den DTO'ya ve DTO'dan entity'ye dönüşümler sağlanmıştır. Bu yapı, veritabanı ile UI katmanı arasında veri alışverişi sırasında kullanılan modellerin sorunsuz dönüşmesini sağlar. Her bir CreateMap ifadesi, belirli bir entity ile DTO/VM arasında otomatik dönüşüm sağlar, böylece manuel eşleme yapma zorunluluğunu ortadan kaldırır.
 */