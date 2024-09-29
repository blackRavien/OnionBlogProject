using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Domain.Entities;
using OnionProject.Domain.AbstractRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnionProject.Domain.Enum;

[Route("api/[controller]")]
[ApiController]
public class AdminGenreController : ControllerBase
{
    private readonly IGenreRepo _genreRepo;
    
    public AdminGenreController(IGenreRepo genreRepo)
    {
        _genreRepo = genreRepo;
    }

    [HttpGet]
    public async Task<ActionResult<List<GenreVm>>> GetAllGenres()
    {
        var genres = await _genreRepo.GetAll();
        var genreVms = genres.Select(g => new GenreVm { Id = g.Id, Name = g.Name }).ToList();
        return Ok(genreVms);
    }

   

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGenreDTO genreDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var genre = new Genre
        {
            Name = genreDto.Name,
            CreatedDate = genreDto.CreatedDate,
            Status = genreDto.Status
        };

        await _genreRepo.Add(genre);
        return CreatedAtAction(nameof(GetAllGenres), new { id = genre.Id }, genre);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGenreDTO genreDto)
    {
        if (id != genreDto.Id) return BadRequest();

        var genre = await _genreRepo.GetById(id);
        if (genre == null) return NotFound();

        genre.Name = genreDto.Name;
        genre.Status = genreDto.Status;
        genre.UpdatedDate = DateTime.Now;

        await _genreRepo.Update(genre);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _genreRepo.GetById(id);
        if (genre == null) return NotFound();

        await _genreRepo.Delete(genre);
        return NoContent();
    }
}
