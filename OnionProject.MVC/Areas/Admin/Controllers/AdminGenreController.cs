using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using OnionProject.Domain.Enum;
using System.Collections.Generic;
using System.Threading.Tasks;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class AdminGenreController : Controller
{
    private readonly IGenreRepo _genreRepo;
    private readonly string uri = "https://localhost:7296"; // API URL

    public AdminGenreController(IGenreRepo genreRepo)
    {
        _genreRepo = genreRepo;
    }

    public async Task<IActionResult> Index()
    {
        var genres = await _genreRepo.GetAll();
        var genreVms = genres.Select(g => new GenreVm { Id = g.Id, Name = g.Name }).ToList();
        return View(genreVms);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        List<GenreVm> genres = new List<GenreVm>();
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync($"{uri}/api/AdminGenreApi");
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                genres = JsonConvert.DeserializeObject<List<GenreVm>>(apiResponse);
            }
        }
        ViewBag.Genres = genres;
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreDTO genreDto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Girdiğiniz verileri kontrol ediniz!";
            return View(genreDto);
        }

        var genre = new Genre
        {
            Name = genreDto.Name,
            CreatedDate = DateTime.Now,
            Status = Status.Active
        };

        await _genreRepo.Add(genre);
        TempData["Success"] = "Tür başarıyla oluşturuldu!";
        return Redirect("https://localhost:7225/Admin/AdminGenre/Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var genre = await _genreRepo.GetById(id);
        if (genre == null) return NotFound();

        var genreDto = new UpdateGenreDTO
        {
            Id = genre.Id,
            Name = genre.Name,
            Status = genre.Status
        };
        return View(genreDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateGenreDTO genreDto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Girdiğiniz verileri kontrol ediniz!";
            return View(genreDto);
        }

        var genre = await _genreRepo.GetById(genreDto.Id);
        if (genre == null) return NotFound();

        genre.Name = genreDto.Name;
        genre.Status = genreDto.Status;
        genre.UpdatedDate = DateTime.Now;

        await _genreRepo.Update(genre);
        TempData["Success"] = "Tür başarıyla güncellendi!";
        return Redirect("https://localhost:7225/Admin/AdminGenre/Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _genreRepo.GetById(id);
        if (genre == null) return NotFound();

        await _genreRepo.Delete(genre);
        TempData["Success"] = "Tür başarıyla silindi!";
        return Redirect("https://localhost:7225/Admin/AdminGenre/Index");
    }
}
