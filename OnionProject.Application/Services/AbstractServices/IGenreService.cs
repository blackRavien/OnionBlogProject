using OnionProject.Application.Models.DTOs; // DTO sınıflarını kullanmak için gerekli olan isim alanı
using OnionProject.Application.Models.VMs; // ViewModel sınıflarını kullanmak için gerekli olan isim alanı
using System.Collections.Generic; // Koleksiyonlar için gerekli olan isim alanı
using System.Threading.Tasks; // Asenkron programlama için gerekli olan isim alanı

namespace OnionProject.Application.Services.AbstractServices
{
    // Türler ile ilgili hizmetlerin arayüzü
    public interface IGenreService
    {
        // Yeni bir tür oluşturur ve oluşturulan türün ViewModel'ini döndürür
        Task<GenreVm> CreateGenre(CreateGenreDTO createGenreDTO);

        // Var olan bir türü günceller ve güncellenen türün ViewModel'ini döndürür
        Task<GenreVm> UpdateGenre(UpdateGenreDTO updateGenreDTO);

        // Belirtilen ID'ye göre bir türü siler
        Task DeleteGenre(int id);

        // Belirtilen ID'ye göre tür bilgilerini döndürür
        Task<GenreVm> GetGenreById(int id);

        // Tüm türleri listeler ve ViewModel olarak döndürür
        Task<List<GenreVm>> GetAllGenres();

        // Belirtilen isimdeki türü döndürür
        Task<GenreVm> GetGenreByName(string name);

        // Belirtilen tür ID'sine göre ilgili postları döndürür
        Task<List<PostVm>> GetPostsByGenre(int genreId);
    }
}
/*
    Açıklamalar
Namespace Kullanımı:

OnionProject.Application.Models.DTOs: Veri taşıma nesnelerini (DTO) içeren isim alanıdır.
OnionProject.Application.Models.VMs: Görünüm modellerini (ViewModel) içeren isim alanıdır.
System.Collections.Generic: Genel koleksiyon türleri için gerekli olan isim alanıdır.
System.Threading.Tasks: Asenkron programlama işlevleri için gerekli olan isim alanıdır.
Arayüz (Interface):

IGenreService: Bu arayüz, türler (genre) ile ilgili hizmetlerin uygulama sözleşmesini tanımlar. Uygulamanın farklı katmanları arasında türlerle ilgili işlemleri gerçekleştirmek için gereken işlevleri belirler.
Metod Tanımları:

CreateGenre(CreateGenreDTO createGenreDTO):

Yeni bir tür oluşturmak için kullanılır. Bu metod, CreateGenreDTO türünde bir nesne alır ve oluşturulan türün GenreVm (ViewModel) nesnesini döndürür.
Task<GenreVm>: Asenkron bir işlem döndürdüğünü ve GenreVm türünde bir nesne döndüreceğini belirtir.
UpdateGenre(UpdateGenreDTO updateGenreDTO):

Var olan bir türü güncellemek için kullanılır. UpdateGenreDTO türünde bir nesne alır ve güncellenen türün GenreVm nesnesini döndürür.
Task<GenreVm>: Asenkron bir işlem döndüğünü belirtir.
DeleteGenre(int id):

Belirtilen ID'ye göre bir türü silmek için kullanılır.
Task: İşlemin asenkron olarak gerçekleştirileceğini belirtir.
GetGenreById(int id):

Belirtilen ID'ye göre tür bilgilerini döndürmek için kullanılır. GenreVm nesnesi döndürür.
Task<GenreVm>: Asenkron bir işlem döndüğünü belirtir.
GetAllGenres():

Tüm türleri listelemek için kullanılır. Tüm türlerin GenreVm nesneleri olarak döndürülmesini sağlar.
Task<List<GenreVm>>: Asenkron bir işlem döndüğünü belirtir.
GetGenreByName(string name):

Belirtilen isimdeki türü döndürmek için kullanılır. GenreVm nesnesi döndürür.
Task<GenreVm>: Asenkron bir işlem döndüğünü belirtir.
GetPostsByGenre(int genreId):

Belirtilen tür ID'sine göre ilgili postları döndürmek için kullanılır. Postları PostVm nesneleri olarak döndürür.
Task<List<PostVm>>: Asenkron bir işlem döndüğünü belirtir.
Genel Değerlendirme
IGenreService arayüzü, türler ile ilgili temel işlevlerin uygulanabilirliğini sağlayan bir sözleşmedir. Bu yapı, türleri yönetmek için gerekli olan işlemleri tanımlar ve uygulamanın modülerliğini artırır. Arayüzdeki her bir metod, belirli bir işlevi yerine getirir ve türler üzerinde CRUD (Create, Read, Update, Delete) işlemleri ile ilgili işlemleri kolaylaştırır. Bağımlılık enjeksiyonu sayesinde, bu arayüzün uygulanması ve yönetimi daha esnek hale gelir.
 */