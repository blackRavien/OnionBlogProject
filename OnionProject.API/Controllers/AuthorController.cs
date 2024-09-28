﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Services.AbstractServices;

namespace OnionProject.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var authorList = await _authorService.GetAuthors();
            return Ok(authorList); // Burada AuthorVm kullanılıyor.
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var authorDetail = await _authorService.GetDetail(id);
            if (authorDetail == null)
            {
                return NotFound();
            }
            return Ok(authorDetail);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateAuthorDTO author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool exists = await _authorService.IsAuthorExists(author.FirstName, author.LastName);
            if (exists)
            {
                return Conflict("Bu isimde bir yazar zaten mevcut.");
            }

            // Resim dosyasının varlığını kontrol et
            if (author.Image != null)
            {
                // Dosya adını oluştur
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(author.Image.FileName);

                // Dosya kaydetme yolu
                var path = Path.Combine(Directory.GetCurrentDirectory(), "../OnionProject.MVC/wwwroot/images", fileName);


                // Dosyayı kaydet
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await author.Image.CopyToAsync(stream);
                }

                // Veritabanına kaydedilecek resim yolu
                author.ImagePath = "/images/" + fileName;
            }

            await _authorService.Create(author);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromForm] UpdateAuthorDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Güncelleme işlemi sadece yapılacak, sonuç kontrol edilmeyecek.
            await _authorService.Update(model);

            return Ok(); // Başarıyla sonuçlanmış kabul edilecek.
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Yazarın var olup olmadığını kontrol et
            var author = await _authorService.GetById(id);
            if (author == null)
            {
                return NotFound(new { message = "Yazar bulunamadı" });
            }

            // Yazar silme işlemini gerçekleştir
            await _authorService.Delete(id);

            // Başarılı olduğunda 204 No Content dön
            return NoContent(); // Silme başarılı, 204 No Content döner
        }



    }
}
