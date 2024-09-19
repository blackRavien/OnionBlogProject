using AutoMapper;
using OnionProject.Application.Models.DTOs;
using OnionProject.Application.Models.VMs;
using OnionProject.Application.Services.AbstractServices;
using OnionProject.Domain.AbstractRepositories;
using OnionProject.Domain.Entities;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.ConcreteManagers
{
    public class AuthorManager : IAuthorService
    {
        private readonly IMapper _mapper; // AutoMapper'ı enjekte etmek için
        private readonly IAuthorRepo _authorRepo; // Author repo'yu enjekte etmek için

        public AuthorManager(IMapper mapper, IAuthorRepo authorRepo)
        {
            _mapper = mapper;
            _authorRepo = authorRepo;
        }

        // Yeni bir yazar oluşturur
        public async Task Create(CreateAuthorDTO model)
        {
            // DTO'dan Author entity'sine dönüştürme
            var author = _mapper.Map<Author>(model);

            if (author.UploadPath is not null) // Yazar fotoğrafı varsa
            {
                // Fotoğrafı yükleme ve işleme
                using var image = Image.Load(model.UploadPath.OpenReadStream());
                image.Mutate(x => x.Resize(600, 500));
                Guid guid = Guid.NewGuid(); // Yeni isim için GUID oluşturma
                image.Save($"wwwroot/images/{guid}.jpg"); // Resmi kaydetme
                author.ImagePath = $"/images/{guid}.jpg"; // Yazarın fotoğraf yolunu ayarlama

                // Yazar verisini repo'ya kaydetme
                await _authorRepo.Create(author);
            }
            else
            {
                // Fotoğraf yoksa varsayılan fotoğraf yolunu ayarlama
                author.ImagePath = $"/images/defaultPhoto.jpg";
                await _authorRepo.Create(author);
            }
        }

        // ID'ye göre yazarı siler
        public async Task Delete(int id)
        {
            var author = await _authorRepo.GetById(id); // Yazar bilgilerini al
            if (author != null) // Yazar varsa
            {
                await _authorRepo.Delete(author); // Yazarı sil
            }
        }

        // Tüm yazarları listeler
        public async Task<List<AuthorVm>> GetAuthors()
        {
            var authors = await _authorRepo.GetAll(); // Tüm yazarları al
            return _mapper.Map<List<AuthorVm>>(authors); // DTO'ya dönüştürüp döndür
        }

        // ID'ye göre yazarı güncelleme için DTO alır
        public async Task<UpdateAuthorDTO> GetById(int id)
        {
            var author = await _authorRepo.GetById(id); // Yazar bilgilerini al
            return _mapper.Map<UpdateAuthorDTO>(author); // DTO'ya dönüştürüp döndür
        }

        // ID'ye göre yazarın detaylarını alır
        public async Task<AuthorDetailVm> GetDetail(int id)
        {
            var author = await _authorRepo.GetById(id); // Yazar bilgilerini al
            return _mapper.Map<AuthorDetailVm>(author); // ViewModel'e dönüştürüp döndür
        }

        // Ad ve soyadı ile yazarın detaylarını alır
        public async Task<AuthorDetailVm> GetFullName(string firstName, string lastName)
        {
            var author = await _authorRepo.GetByFullName(firstName, lastName); // Yazar bilgilerini al
            return _mapper.Map<AuthorDetailVm>(author); // ViewModel'e dönüştürüp döndür
        }

        // Ad ve soyadı ile yazarın mevcut olup olmadığını kontrol eder
        public async Task<bool> IsAuthorExists(string firstName, string lastName)
        {
            var author = await _authorRepo.GetByFullName(firstName, lastName); // Yazar bilgilerini al
            return author != null; // Yazar varsa true, yoksa false döndür
        }

        // Yazar bilgilerini günceller
        public async Task Update(UpdateAuthorDTO model)
        {
            var author = _mapper.Map<Author>(model); // DTO'dan Author entity'sine dönüştürme

            if (model.UploadPath is not null) // Fotoğraf varsa
            {
                using var image = Image.Load(model.UploadPath.OpenReadStream()); // Fotoğrafı yükle
                image.Mutate(x => x.Resize(600, 500)); // Resmi boyutlandır
                Guid guid = Guid.NewGuid(); // Yeni isim için GUID oluşturma
                image.Save($"wwwroot/images/{guid}.jpg"); // Resmi kaydet
                author.ImagePath = $"/images/{guid}.jpg"; // Yazarın fotoğraf yolunu ayarla
            }

            await _authorRepo.Update(author); // Güncellenmiş yazarı repo'ya kaydet
        }
    }
}
