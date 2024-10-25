using Microsoft.AspNetCore.Authorization; // Yetkilendirme için gerekli olan kütüphane
using Microsoft.AspNetCore.Mvc; // MVC yapıları için gerekli olan kütüphane
using Newtonsoft.Json; // JSON verilerini serileştirmek ve serileştirmek için gerekli kütüphane
using OnionProject.Application.Models.DTOs; // Veri Transfer Nesneleri (DTO'lar) için gerekli kütüphane
using OnionProject.Application.Models.VMs; // Görünüm Modelleri (VM'ler) için gerekli kütüphane
using OnionProject.Domain.AbstractRepositories; // Abstrakt repository arayüzleri için gerekli kütüphane
using OnionProject.Domain.Entities; // Domain varlıkları için gerekli kütüphane
using OnionProject.Domain.Enum; // Enum tanımları için gerekli kütüphane
using System.Collections.Generic; // Koleksiyonlar için gerekli kütüphane
using System.Threading.Tasks; // Asenkron programlama için gerekli kütüphane

// "Admin" alanını tanımlıyor, böylece bu controller Admin alanındaki yolları kullanıyor
[Area("Admin")]
// Yalnızca "Admin" rolüne sahip kullanıcıların bu controller'a erişmesine izin veriliyor
[Authorize(Roles = "Admin")]
public class AdminGenreController : Controller
{
    private readonly IGenreRepo _genreRepo; // Türler ile etkileşim için repository
    private readonly string uri = "https://localhost:7296"; // API URL'si

    // Constructor, tür repository'sini alıyor ve sınıf değişkenine atıyor
    public AdminGenreController(IGenreRepo genreRepo)
    {
        _genreRepo = genreRepo; // Geçerli repository nesnesini ayarlama
    }

    // Ana sayfa metodu, tüm türleri getirip görüntüler
    public async Task<IActionResult> Index()
    {
        var genres = await _genreRepo.GetAll(); // Repository'den tüm türleri al
        // Türleri, Görünüm Modeline (VM) dönüştür
        var genreVms = genres.Select(g => new GenreVm { Id = g.Id, Name = g.Name }).ToList();
        return View(genreVms); // Türleri görünümde döndür
    }

    // Yeni bir tür oluşturmak için GET isteği ile formu görüntüle
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        List<GenreVm> genres = new List<GenreVm>(); // Görünüm modeli listesi
        using (var httpClient = new HttpClient()) // HTTP istemcisi oluştur
        {
            // API'den türleri almak için GET isteği yap
            var response = await httpClient.GetAsync($"{uri}/api/AdminGenreApi");
            if (response.IsSuccessStatusCode) // İstek başarılıysa
            {
                string apiResponse = await response.Content.ReadAsStringAsync(); // API yanıtını oku
                genres = JsonConvert.DeserializeObject<List<GenreVm>>(apiResponse); // JSON'dan listeye dönüştür
            }
        }
        ViewBag.Genres = genres; // Türleri ViewBag'e ata
        return View(); // Oluşturma formunu döndür
    }

    // Yeni bir tür oluşturmak için POST isteği
    [HttpPost]
    public async Task<IActionResult> Create(CreateGenreDTO genreDto)
    {
        // Model doğrulaması yap
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Girdiğiniz verileri kontrol ediniz!"; // Hata mesajı göster
            return View(genreDto); // Hatalı model ile formu döndür
        }

        // Yeni tür oluştur
        var genre = new Genre
        {
            Name = genreDto.Name, // DTO'dan adı al
            CreatedDate = DateTime.Now, // Oluşturulma tarihini ayarla
            Status = Status.Active // Durumunu aktif olarak ayarla
        };

        await _genreRepo.Add(genre); // Repository üzerinden yeni türü ekle
        TempData["Success"] = "Tür başarıyla oluşturuldu!"; // Başarı mesajı göster
        return Redirect("https://localhost:7225/Admin/AdminGenre/Index"); // Ana sayfaya yönlendir
    }

    // Türü düzenlemek için GET isteği ile mevcut türü al
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var genre = await _genreRepo.GetById(id); // ID ile türü al
        if (genre == null) return NotFound(); // Eğer tür bulunamazsa 404 döndür

        // Güncelleme DTO'sunu oluştur
        var genreDto = new UpdateGenreDTO
        {
            Id = genre.Id, // ID'yi ata
            Name = genre.Name, // Adı ata
            Status = genre.Status // Durumu ata
        };
        return View(genreDto); // Düzenleme formunu döndür
    }

    // Düzenleme için POST isteği
    [HttpPost]
    public async Task<IActionResult> Edit(UpdateGenreDTO genreDto)
    {
        // Model doğrulaması yap
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Girdiğiniz verileri kontrol ediniz!"; // Hata mesajı göster
            return View(genreDto); // Hatalı model ile formu döndür
        }

        var genre = await _genreRepo.GetById(genreDto.Id); // Güncellenecek türü al
        if (genre == null) return NotFound(); // Eğer tür bulunamazsa 404 döndür

        // Türün alanlarını güncelle
        genre.Name = genreDto.Name; // Adı güncelle
        genre.Status = genreDto.Status; // Durumu güncelle
        genre.UpdatedDate = DateTime.Now; // Güncellenme tarihini ayarla

        await _genreRepo.Update(genre); // Repository üzerinden türü güncelle
        TempData["Success"] = "Tür başarıyla güncellendi!"; // Başarı mesajı göster
        return Redirect("https://localhost:7225/Admin/AdminGenre/Index"); // Ana sayfaya yönlendir
    }

    // Türü silme işlemi
    public async Task<IActionResult> Delete(int id)
    {
        var genre = await _genreRepo.GetById(id); // ID ile türü al
        if (genre == null) return NotFound(); // Eğer tür bulunamazsa 404 döndür

        await _genreRepo.Delete(genre); // Repository üzerinden türü sil
        TempData["Success"] = "Tür başarıyla silindi!"; // Başarı mesajı göster
        return Redirect("https://localhost:7225/Admin/AdminGenre/Index"); // Ana sayfaya yönlendir
    }
}
/*
    Sayfa Özeti:
AdminGenreController, admin kullanıcılarının türleri yönetmesine olanak tanıyan bir ASP.NET Core MVC controller'ıdır. Controller, türleri listeleme, oluşturma, düzenleme ve silme işlemlerini içerir. Her bir metod asenkron olarak çalışır ve kullanıcı etkileşimlerine uygun yanıtlar verir. Yalnızca "Admin" rolüne sahip kullanıcılar bu controller'ı kullanabilir. API üzerinden tür verilerini almak için HttpClient kullanılır ve JSON verileri, model nesnelerine dönüştürülerek işlemler gerçekleştirilir. Başarılı veya hatalı işlemlerde kullanıcıya bilgi vermek için TempData kullanılır.
 */