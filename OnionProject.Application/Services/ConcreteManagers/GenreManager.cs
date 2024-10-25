using AutoMapper; // AutoMapper kütüphanesini kullanmak için gerekli olan isim alanı
using OnionProject.Application.Models.DTOs; // DTO sınıflarını kullanmak için gerekli olan isim alanı
using OnionProject.Application.Models.VMs; // ViewModel sınıflarını kullanmak için gerekli olan isim alanı
using OnionProject.Application.Services.AbstractServices; // AbstractServices içindeki arayüzleri kullanmak için gerekli olan isim alanı
using OnionProject.Domain.AbstractRepositories; // Repository arayüzlerini kullanmak için gerekli olan isim alanı
using OnionProject.Domain.Entities; // Entity sınıflarını kullanmak için gerekli olan isim alanı
using System.Collections.Generic; // Koleksiyonlar için gerekli olan isim alanı
using System.Linq; // LINQ sorguları için gerekli olan isim alanı
using System.Threading.Tasks; // Asenkron programlama için gerekli olan isim alanı

namespace OnionProject.Application.Services.ConcreteManagers
{
    // Tür yönetimi ile ilgili işlevleri uygulayan sınıf
    public class GenreManager : IGenreService
    {
        private readonly IGenreRepo _genreRepo; // Türleri yöneten repository
        private readonly IMapper _mapper; // AutoMapper nesnesi

        // Constructor, dependency injection ile repository ve mapper nesnelerini alır
        public GenreManager(IGenreRepo genreRepo, IMapper mapper)
        {
            _genreRepo = genreRepo; // Tür repository'sini atama
            _mapper = mapper; // AutoMapper nesnesini atama
        }

        // Yeni bir tür oluşturur
        public async Task<GenreVm> CreateGenre(CreateGenreDTO createGenreDTO)
        {
            var genre = _mapper.Map<Genre>(createGenreDTO); // DTO'dan tür nesnesine dönüşüm
            await _genreRepo.Create(genre); // Repository üzerinden yeni türü ekler
            return _mapper.Map<GenreVm>(genre); // Eklenen türü ViewModel'e dönüştürerek döndürür
        }

        // Var olan bir türü günceller
        public async Task<GenreVm> UpdateGenre(UpdateGenreDTO updateGenreDTO)
        {
            var genre = await _genreRepo.GetById(updateGenreDTO.Id); // ID'ye göre türü al
            if (genre == null) return null; // Tür bulunamazsa null döndür
            _mapper.Map(updateGenreDTO, genre); // DTO'daki verileri tür nesnesine aktar
            await _genreRepo.Update(genre); // Repository üzerinden türü günceller
            return _mapper.Map<GenreVm>(genre); // Güncellenen türü ViewModel'e dönüştürerek döndür
        }

        // Belirtilen ID'ye göre türü siler
        public async Task DeleteGenre(int id)
        {
            var genre = await _genreRepo.GetById(id); // ID'ye göre türü al
            if (genre != null)
            {
                await _genreRepo.Delete(genre); // Tür varsa repository üzerinden sil
            }
        }

        // Belirtilen ID'ye göre tür bilgilerini döndürür
        public async Task<GenreVm> GetGenreById(int id)
        {
            var genre = await _genreRepo.GetById(id); // ID'ye göre türü al
            return _mapper.Map<GenreVm>(genre); // Türü ViewModel'e dönüştürerek döndür
        }

        // Tüm türleri listeler
        public async Task<List<GenreVm>> GetAllGenres()
        {
            var genres = await _genreRepo.GetDefaults(x => true); // Tüm türleri al
            return _mapper.Map<List<GenreVm>>(genres); // Türleri ViewModel'lere dönüştürerek döndür
        }

        // Belirtilen isimdeki türü döndürür
        public async Task<GenreVm> GetGenreByName(string name)
        {
            var genre = await _genreRepo.GetByName(name); // İsimle türü al
            return _mapper.Map<GenreVm>(genre); // Türü ViewModel'e dönüştürerek döndür
        }

        // Belirtilen tür ID'sine göre ilgili postları döndürür
        public async Task<List<PostVm>> GetPostsByGenre(int genreId)
        {
            var posts = await _genreRepo.GetPostsByGenre(genreId); // Tür ID'sine göre postları al
            return _mapper.Map<List<PostVm>>(posts); // Postları ViewModel'lere dönüştürerek döndür
        }
    }
}

/*
    Açıklamalar
Namespace Kullanımı:

AutoMapper: Nesneler arasında dönüşüm yapmak için kullanılan kütüphane.
OnionProject.Application.Models.DTOs: Uygulama veri taşıma nesneleri (DTO) için kullanılan isim alanı.
OnionProject.Application.Models.VMs: Uygulama görünüm modelleri (VM) için kullanılan isim alanı.
OnionProject.Application.Services.AbstractServices: Uygulama hizmetlerinin arayüzlerini içeren isim alanı.
OnionProject.Domain.AbstractRepositories: Veritabanı ile etkileşimde bulunan repository arayüzlerini içeren isim alanı.
OnionProject.Domain.Entities: Uygulamanın veri modellerini temsil eden sınıflar için kullanılan isim alanı.
System.Collections.Generic: Koleksiyon sınıfları için gerekli olan isim alanı.
System.Linq: LINQ sorgularını kullanmak için gerekli olan isim alanı.
System.Threading.Tasks: Asenkron programlama için gerekli olan isim alanı.
Sınıf (Class):

GenreManager: IGenreService arayüzünü uygulayan sınıf. Türlerle (genre) ilgili işlevleri gerçekleştiren bir yönetici sınıfıdır.
Üyeler (Members):

_genreRepo: Türleri yöneten repository nesnesi. Bu nesne ile veri erişim işlemleri gerçekleştirilir.
_mapper: AutoMapper nesnesi. DTO'lar ile entity'ler arasında dönüşüm işlemlerini gerçekleştirmek için kullanılır.
Constructor:

Constructor, IGenreRepo ve IMapper nesnelerini alır. Bu nesneler, sınıfın içinde kullanılmak üzere saklanır.
Metod Tanımları:

CreateGenre(CreateGenreDTO createGenreDTO):

Yeni bir tür oluşturur. CreateGenreDTO türündeki nesneyi Genre türüne dönüştürür ve veritabanına ekler. Eklenen tür, GenreVm (ViewModel) olarak döndürülür.
UpdateGenre(UpdateGenreDTO updateGenreDTO):

Var olan bir türü günceller. Önce türü veritabanından alır, ardından DTO'daki verileri mevcut tür nesnesine aktarır ve güncellenmiş türü döndürür.
DeleteGenre(int id):

Belirtilen ID'ye göre türü siler. Eğer tür bulunursa, repository üzerinden silme işlemi gerçekleştirilir.
GetGenreById(int id):

Belirtilen ID'ye göre türü alır ve GenreVm olarak döndürür.
GetAllGenres():

Tüm türleri alır ve GenreVm listesi olarak döndürür.
GetGenreByName(string name):

Belirtilen isimdeki türü alır ve GenreVm olarak döndürür.
GetPostsByGenre(int genreId):

Belirtilen tür ID'sine göre ilgili postları alır ve PostVm listesi olarak döndürür.
Genel Değerlendirme
GenreManager, türlerle ilgili temel işlevleri gerçekleştiren bir yöneticir. Bu yapı, türlerin oluşturulması, güncellenmesi, silinmesi ve sorgulanması gibi işlemleri yönetir. Ayrıca, AutoMapper kullanarak DTO ve ViewModel dönüşümlerini otomatikleştirir, böylece kodun daha temiz ve anlaşılır olmasını sağlar. Sınıfın tasarımı, uygulama katmanları arasındaki bağımlılıkları azaltarak esneklik ve test edilebilirlik sağlar.
 */