using AutoMapper; // AutoMapper kütüphanesini içe aktarıyor.
using OnionProject.Application.Models.DTOs; // DTO sınıflarını içe aktarıyor.
using OnionProject.Application.Models.VMs; // ViewModel sınıflarını içe aktarıyor.
using OnionProject.Application.Services.AbstractServices; // Servis arayüzünü içe aktarıyor.
using OnionProject.Domain.AbstractRepositories; // Repository arayüzlerini içe aktarıyor.
using OnionProject.Domain.Entities; // Domain varlıklarını içe aktarıyor.
using SixLabors.ImageSharp; // Resim işleme kütüphanesini içe aktarıyor.
using SixLabors.ImageSharp.Processing; // Resim işleme işlemleri için gerekli olan namespace.

namespace OnionProject.Application.Services.ConcreteManagers
{
    // IAuthorService arayüzünü uygulayan AuthorManager sınıfı
    public class AuthorManager : IAuthorService
    {
        private readonly IMapper _mapper; // AutoMapper nesnesi
        private readonly IAuthorRepo _authorRepo; // Yazar repository nesnesi

        // Constructor
        public AuthorManager(IMapper mapper, IAuthorRepo authorRepo)
        {
            _mapper = mapper; // Mapper'ı atama
            _authorRepo = authorRepo; // Repository'yi atama
        }

        // Yeni bir yazar oluşturur
        public async Task Create(CreateAuthorDTO model)
        {
            // DTO'dan Author entity'sine dönüştürme
            var author = _mapper.Map<Author>(model);

            // Yazarın fotoğrafı varsa
            if (model.Image is not null)
            {
                // Fotoğrafı yükleme ve işleme
                using var image = Image.Load(model.Image.OpenReadStream()); // Resmi yükle
                image.Mutate(x => x.Resize(600, 500)); // Resmi boyutlandır
                Guid guid = Guid.NewGuid(); // Yeni isim için GUID oluşturma
                image.Save($"wwwroot/images/{guid}.jpg"); // Resmi kaydetme
                author.ImagePath = $"/images/{guid}.jpg"; // Yazarın fotoğraf yolunu ayarlama

                // Yazar verisini repo'ya kaydetme
                await _authorRepo.Create(author);
            }
            else // Fotoğraf yoksa
            {
                // Varsayılan fotoğraf yolunu ayarlama
                author.ImagePath = $"/images/defaultPhoto.jpg";
                await _authorRepo.Create(author);
            }
        }

        // Belirtilen ID'ye göre yazarı siler
        public async Task Delete(int id)
        {
            // Yazar bilgilerini al
            var author = await _authorRepo.GetById(id);
            if (author != null) // Yazar varsa
            {
                await _authorRepo.Delete(author); // Yazarı sil
            }
        }

        // Tüm yazarları listeler
        public async Task<List<AuthorVm>> GetAuthors()
        {
            // Tüm yazarları al
            var authors = await _authorRepo.GetAll();
            // DTO'ya dönüştürüp döndür
            return _mapper.Map<List<AuthorVm>>(authors);
        }

        // ID'ye göre yazarı güncelleme için DTO alır
        public async Task<UpdateAuthorDTO> GetById(int id)
        {
            // Yazar bilgilerini al
            var author = await _authorRepo.GetById(id);
            // DTO'ya dönüştürüp döndür
            return _mapper.Map<UpdateAuthorDTO>(author);
        }

        // ID'ye göre yazarın detaylarını alır
        public async Task<AuthorDetailVm> GetDetail(int id)
        {
            // Yazar bilgilerini al
            var author = await _authorRepo.GetById(id);
            // ViewModel'e dönüştürüp döndür
            return _mapper.Map<AuthorDetailVm>(author);
        }

        // Ad ve soyadı ile yazarın detaylarını alır
        public async Task<AuthorDetailVm> GetFullName(string firstName, string lastName)
        {
            // Yazar bilgilerini al
            var author = await _authorRepo.GetByFullName(firstName, lastName);
            // ViewModel'e dönüştürüp döndür
            return _mapper.Map<AuthorDetailVm>(author);
        }

        // Ad ve soyadı ile yazarın mevcut olup olmadığını kontrol eder
        public async Task<bool> IsAuthorExists(string firstName, string lastName)
        {
            // Yazar bilgilerini al
            var author = await _authorRepo.GetByFullName(firstName, lastName);
            // Yazar varsa true, yoksa false döndür
            return author != null;
        }

        // Yazar bilgilerini günceller
        public async Task Update(UpdateAuthorDTO model)
        {
            // Mevcut yazarı al
            var existingAuthor = await _authorRepo.GetById(model.Id);
            if (existingAuthor == null)
            {
                throw new Exception("Yazar bulunamadı."); // Yazar bulunamazsa hata fırlat
            }

            // Mevcut yazarı DTO'dan gelen verilere göre güncelle
            existingAuthor.FirstName = model.FirstName;
            existingAuthor.LastName = model.LastName;
            existingAuthor.Biography = model.Biography;
            existingAuthor.PhoneNumber = model.PhoneNumber;
            existingAuthor.Email = model.Email;

            // Yazarın yeni fotoğrafı varsa
            if (model.Image is not null)
            {
                // Fotoğrafı yükleme ve işleme
                using var image = Image.Load(model.Image.OpenReadStream());
                image.Mutate(x => x.Resize(600, 500)); // Resmi boyutlandır
                Guid guid = Guid.NewGuid(); // Yeni isim için GUID oluşturma
                image.Save($"wwwroot/images/{guid}.jpg"); // Resmi kaydetme
                existingAuthor.ImagePath = $"/images/{guid}.jpg"; // Yazarın fotoğraf yolunu ayarlama
            }

            // Güncellenmiş yazarı repo'ya kaydet
            await _authorRepo.Update(existingAuthor);
        }
    }
}
/*
    Detaylı Açıklama:
Namespace İçe Aktarma:

Gerekli olan kütüphaneler ve sınıflar içe aktarılır.
AuthorManager Sınıfı:

IAuthorService arayüzünü uygulayan bir sınıf.
Constructor:

IMapper ve IAuthorRepo nesneleri ile sınıfın bağımlılıkları atanır.
Metod Tanımları:

Create: Yeni bir yazar oluşturur. Yazarın fotoğrafı varsa, fotoğrafı yükleyip boyutlandırır, GUID ile kaydeder ve yazar nesnesinin ImagePath özelliğini ayarlar.
Delete: Belirli bir ID'ye sahip yazarı siler.
GetAuthors: Tüm yazarları alır ve AuthorVm nesnelerine dönüştürerek döner.
GetById: Belirli bir ID'ye sahip yazarı alır ve UpdateAuthorDTO nesnesine dönüştürerek döner.
GetDetail: Belirli bir ID'ye sahip yazarın detaylarını alır ve AuthorDetailVm nesnesine dönüştürerek döner.
GetFullName: Belirli bir ad ve soyadı ile yazarın detaylarını alır.
IsAuthorExists: Belirli bir ad ve soyadı ile yazarın mevcut olup olmadığını kontrol eder.
Update: Mevcut yazarın bilgilerini günceller. Eğer yazarın yeni bir fotoğrafı varsa, fotoğrafı yükleyip kaydeder.
 */