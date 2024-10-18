using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnionProject.API.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserPostApiController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IAppUserRepo _appUserRepo;
        private readonly ICommentService _commentService;
        private readonly IAppUserService _appUserService;
        private readonly ILogger _logger;

        public UserPostApiController(IPostService postService, ICommentService commentService, IAppUserService appUserService, IAppUserRepo appUserRepo)
        {
            _postService = postService;
            _commentService = commentService;
            _appUserService = appUserService;
            _appUserRepo = appUserRepo;
        }

        // Tüm gönderileri almak için
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetPosts();

            var postVms = posts.Select(post => new
            {
                post.Id,
                post.Title,
                post.Content,
                post.GenreName,
                post.AuthorFirstName,
                post.AuthorLastName,
                post.CreatedDate,
                ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}"

            }).ToList();


            return Ok(postVms);
        }

        // Belirli bir gönderinin detaylarını almak için
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var postDetail = await _postService.GetDetail(id);
            if (postDetail == null)
            {
                return NotFound();
            }

            // PostDetailsVm oluşturma
            var postDetailsVm = new PostDetailsVm
            {
                Id = postDetail.Id,
                Title = postDetail.Title,
                Content = postDetail.Content,
                GenreName = postDetail.GenreName,
                AuthorFirstName = postDetail.AuthorFirstName,
                AuthorLastName = postDetail.AuthorLastName,
                CreatedDate = postDetail.CreatedDate,
                Comments = postDetail.Comments,
                AuthorDetailVm = postDetail.AuthorDetailVm,
                // ImagePath'i tam URL olarak ayarla
                ImagePath = $"{postDetail.ImagePath.TrimStart('/')}" // Tam URL'yi oluştur
            };

            return Ok(postDetailsVm);
        }

        [HttpPost]
        
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO createCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _commentService.AddCommentAsync(createCommentDto);
            return Ok("Yorum başarıyla eklendi!");
        }


        [HttpDelete("DeleteComment/{commentId}")]
        
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _commentService.GetCommentsByPostIdAsync(commentId); // Yorumu bul

            if (comment == null)
            {
                return NotFound(new { message = "Yorum bulunamadı." });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Mevcut kullanıcının ID'sini al

            // Yorumu sil
            await _commentService.DeleteCommentAsync(commentId);

            return Ok(new { message = "Yorum başarıyla silindi." });
        }




        [HttpGet("GetProfile/{userId}")]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var userProfile = await _appUserService.GetUserById(userId);

            if (userProfile == null)
            {
                return NotFound();
            }

            var profileVm = new ProfileVm
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                UserName = userProfile.UserName,
                Email = userProfile.Email,
                PhoneNumber = userProfile.PhoneNumber,
                
            };

            return Ok(profileVm);
        }



        [HttpPost("UpdateProfile/{userId}")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDTO updateProfileDto, string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kullanıcıyı veritabanından al
            var user = await _appUserRepo.GetById(userId);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            // Profil bilgilerini güncelle
            user.FirstName = updateProfileDto.FirstName ?? user.FirstName;
            user.LastName = updateProfileDto.LastName ?? user.LastName;
            user.UserName = updateProfileDto.UserName ?? user.UserName;
            user.Email = updateProfileDto.Email ?? user.Email;
            user.PhoneNumber = updateProfileDto.PhoneNumber ?? user.PhoneNumber;

            // Şifre güncelleme kontrolü
            if (!string.IsNullOrEmpty(updateProfileDto.Password))
            {
                var passwordHasher = new PasswordHasher<AppUser>();
                user.PasswordHash = passwordHasher.HashPassword(user, updateProfileDto.Password);
            }


            // Kullanıcıyı güncelle
            await _appUserRepo.Update(user);

            return Ok("Profil başarıyla güncellendi.");
        }




    }
}
