using Microsoft.AspNetCore.Mvc;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Area("Admin")]
public class AdminCommentController : Controller
{
    private readonly ICommentRepo _commentRepo;
    private readonly string uri = "https://localhost:7296"; // API URL

    public AdminCommentController(ICommentRepo commentRepo)
    {
        _commentRepo = commentRepo;
    }

    // Yorumları listeleme
    public async Task<IActionResult> Index()
    {
        var comments = await _commentRepo.GetAllCommentsAsync(); // API'den yorumları al
        var commentVms = comments.Select(c => new CommentVm
        {
            Id = c.Id,
            Content = c.Content,
            CreatedAt = c.CreatedAt
        }).ToList(); // Yorumları ViewModel'e dönüştür
        return View(commentVms);
    }

    // Yorum ekleme sayfası
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Yorum ekleme işlemi
    [HttpPost]
    public async Task<IActionResult> Create(CreateCommentDTO createCommentDto)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Girdiğiniz verileri kontrol ediniz!";
            return View(createCommentDto); // Hata varsa, formu tekrar göster
        }

        var comment = new Comment
        {
            Content = createCommentDto.Content,
            UserId = createCommentDto.UserId,
            PostId = createCommentDto.PostId,
            CreatedAt = DateTime.UtcNow
        };

        await _commentRepo.AddAsync(comment); // API'ye ekleme isteği
        TempData["Success"] = "Yorum başarıyla eklendi!";
        return RedirectToAction(nameof(Index));
    }

    // Yorum silme işlemi
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _commentRepo.GetCommentByIdAsync(id);
        if (comment == null)
        {
            return NotFound(); // Yorum bulunamazsa 404 döner
        }

        await _commentRepo.DeleteAsync(id); // API'ye silme isteği
        TempData["Success"] = "Yorum başarıyla silindi!";
        return RedirectToAction(nameof(Index));
    }
}
