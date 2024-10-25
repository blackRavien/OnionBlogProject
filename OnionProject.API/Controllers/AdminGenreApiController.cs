using Microsoft.AspNetCore.Mvc; // MVC yapılarına erişim sağlar
using OnionProject.Application.Models.DTOs; // DTO (Data Transfer Object) modellerini içerir
using OnionProject.Application.Models.VMs; // View Model'leri içerir
using OnionProject.Domain.Entities; // Domain entity'lerini içerir
using OnionProject.Domain.AbstractRepositories; // Repository arayüzlerini içerir
using System.Collections.Generic; // Liste yapısı için gerekli
using System.Threading.Tasks; // Asenkron işlemler için gerekli
using OnionProject.Domain.Enum; // Enum tanımlarını içerir
using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için gerekli

[Route("api/[controller]")] // API rotasını belirler
[ApiController] // Bu sınıfın bir API kontrolcüsü olduğunu belirtir
public class AdminGenreApiController : ControllerBase
{
    private readonly IGenreRepo _genreRepo; // Tür (Genre) veritabanı işlemleri için repository

    // Genre repository'yi dependency injection ile alır
    public AdminGenreApiController(IGenreRepo genreRepo)
    {
        _genreRepo = genreRepo;
    }

    // Tüm türleri listeleyen endpoint
    [HttpGet]
    public async Task<ActionResult<List<GenreVm>>> GetAllGenres()
    {
        var genres = await _genreRepo.GetAll(); // Tüm türleri asenkron olarak getirir
        var genreVms = genres.Select(g => new GenreVm { Id = g.Id, Name = g.Name }).ToList(); // Türleri View Model'e dönüştürür
        return Ok(genreVms); // 200 OK ile türleri döner
    }

    // Yeni bir tür eklemek için kullanılan endpoint
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGenreDTO genreDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState); // Model doğruluğunu kontrol eder

        var genre = new Genre
        {
            Name = genreDto.Name, // Tür adını DTO'dan alır
            CreatedDate = genreDto.CreatedDate, // Oluşturulma tarihini DTO'dan alır
            Status = genreDto.Status // Tür durumunu DTO'dan alır
        };

        await _genreRepo.Add(genre); // Yeni türü veritabanına ekler
        return CreatedAtAction(nameof(GetAllGenres), new { id = genre.Id }, genre); // 201 Created yanıtı döner
    }

    // Var olan bir türü güncellemek için kullanılan endpoint
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGenreDTO genreDto)
    {
        if (id != genreDto.Id) return BadRequest(); // ID uyumsuzluğu durumunda 400 Bad Request döner

        var genre = await _genreRepo.GetById(id); // ID'ye göre türü getirir
        if (genre == null) return NotFound(); // Tür bulunamazsa 404 Not Found döner

        genre.Name = genreDto.Name; // Tür adını günceller
        genre.Status = genreDto.Status; // Tür durumunu günceller
        genre.UpdatedDate = DateTime.Now; // Güncelleme tarihini güncel tarih olarak atar

        await _genreRepo.Update(genre); // Türü veritabanında günceller
        return NoContent(); // 204 No Content yanıtı döner
    }

    // Belirli bir türü silmek için kullanılan endpoint
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _genreRepo.GetById(id); // ID'ye göre türü getirir
        if (genre == null) return NotFound(); // Tür bulunamazsa 404 Not Found döner

        await _genreRepo.Delete(genre); // Türü veritabanından siler
        return NoContent(); // 204 No Content yanıtı döner
    }
}

/*
    Genel Açıklama:
AdminGenreApiController, admin kullanıcılar için tür (genre) yönetimi sağlamak amacıyla kullanılan bir API kontrolcüsüdür. IGenreRepo aracılığıyla tür veritabanı işlemleri gerçekleştirilir. Kontrolcü; türlerin listelenmesi, yeni tür ekleme, var olan türleri güncelleme ve tür silme işlemlerini sağlar. GetAllGenres metodu tüm türleri getirir ve GenreVm tipine dönüştürerek döner. Create metodu yeni bir tür eklemek için gerekli doğrulamaları yapar ve kaydeder. Update metodu belirli bir türün özelliklerini günceller, Delete metodu ise türü veritabanından siler. Her bir metod, uygun HTTP yanıtlarını (200 OK, 201 Created, 204 No Content, 400 Bad Request ve 404 Not Found gibi) döndürür.
 */