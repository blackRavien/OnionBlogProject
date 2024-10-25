using AutoMapper; // AutoMapper kütüphanesi, nesne dönüşümlerini kolaylaştırmak için kullanılır.
using Microsoft.AspNetCore.Http; // ASP.NET Core'da HTTP istek ve yanıtlarını yönetmek için kullanılır.
using Microsoft.EntityFrameworkCore; // Entity Framework Core ile çalışmak için gerekli.
using OnionProject.Application.Models.DTOs; // DTO sınıflarının tanımlı olduğu alan.
using OnionProject.Application.Models.VMs; // View Model sınıflarının tanımlı olduğu alan.
using OnionProject.Application.Services.AbstractServices; // Servis arayüzlerinin tanımlı olduğu alan.
using OnionProject.Domain.AbstractRepositories; // Repository arayüzlerinin tanımlı olduğu alan.
using OnionProject.Domain.Entities; // Varlıkların tanımlı olduğu alan.
using OnionProject.Infrastructure.Context; // Uygulama veritabanı bağlamının tanımlı olduğu alan.
using SixLabors.ImageSharp; // Resim işleme kütüphanesi.
using SixLabors.ImageSharp.Processing; // Resim işleme işlemleri için gerekli uzantılar.
using System; // Temel sistem sınıfları.
using System.Collections.Generic; // Koleksiyonlar için gerekli.
using System.Threading.Tasks; // Asenkron programlama için gerekli.

namespace OnionProject.Application.Services.ConcreteManagers
{
    // IPostService arayüzünü uygulayan PostManager sınıfı
    public class PostManager : IPostService
    {
        // Bağımlılıkların tanımlanması
        private readonly IMapper _mapper; // AutoMapper nesnesi
        private readonly IPostRepo _postRepo; // Post veri erişim nesnesi
        private readonly IAuthorService _authorService; // Yazar ile ilgili işlemler için servis

        // Constructor ile bağımlılıkların enjekte edilmesi
        public PostManager(IMapper mapper, IPostRepo postRepo, IAuthorService authorService)
        {
            _mapper = mapper;
            _postRepo = postRepo;
            _authorService = authorService;
        }

        // Yeni bir post oluşturma metodu
        public async Task Create(CreatePostDTO model)
        {
            // DTO'dan Post nesnesine dönüşüm
            var post = _mapper.Map<Post>(model);

            // Eğer bir resim yüklenmişse işleme al
            if (model.UploadPath is not null)
            {
                using var image = Image.Load(model.UploadPath.OpenReadStream()); // Resmi yükle
                image.Mutate(x => x.Resize(600, 500)); // Resmi boyutlandır
                Guid guid = Guid.NewGuid(); // Yeni bir GUID oluştur
                image.Save($"wwwroot/images/{guid}.jpg"); // Resmi kaydet
                post.ImagePath = $"/images/{guid}.jpg"; // Postun resim yolunu ayarla
            }
            else
            {
                // Varsayılan bir resim kullan
                post.ImagePath = $"/images/defaultPhoto.jpg";
            }

            // Postu veritabanına ekle
            await _postRepo.Create(post);
        }

        // Mevcut bir postu güncelleme metodu
        public async Task Update(UpdatePostDTO model)
        {
            // Mevcut postu bul
            var post = await _postRepo.GetById(model.Id);
            if (post == null)
            {
                throw new Exception("Post bulunamadı."); // Eğer post yoksa hata fırlat
            }

            // Modelden gelen verileri mevcut post nesnesine aktar
            _mapper.Map(model, post);

            // Eğer yeni bir resim yüklenmişse güncelle
            if (model.UploadPath != null)
            {
                using var stream = model.UploadPath.OpenReadStream(); // Resmi akış olarak aç
                using var image = Image.Load(stream); // Resmi yükle
                image.Mutate(x => x.Resize(600, 500)); // Resmi boyutlandır
                Guid guid = Guid.NewGuid(); // Yeni bir GUID oluştur
                image.Save($"wwwroot/images/{guid}.jpg"); // Resmi kaydet
                post.ImagePath = $"/images/{guid}.jpg"; // Postun yeni resim yolunu ayarla
            }

            // Güncellenmiş postu veritabanında güncelle
            await _postRepo.Update(post);
        }

        // Postu silme metodu
        public async Task Delete(int id)
        {
            var post = await _postRepo.GetById(id); // Postu bul
            if (post != null)
            {
                await _postRepo.Delete(post); // Post varsa sil
            }
        }

        // Tüm postları alma metodu
        public async Task<List<PostVm>> GetPosts()
        {
            var posts = await _postRepo.GetAll(); // Tüm postları al
            return _mapper.Map<List<PostVm>>(posts); // Postları View Model'e dönüştür
        }

        // Belirli bir postun detaylarını alma metodu
        public async Task<PostDetailsVm> GetDetail(int id)
        {
            var post = await _postRepo.GetById(id); // Postu bul
            if (post == null)
            {
                throw new Exception("Post bulunamadı."); // Eğer post yoksa hata fırlat
            }

            // Yazar bilgilerini almak için Yazar servisini kullan
            var author = await _authorService.GetDetail(post.AuthorId);
            if (author == null)
            {
                throw new Exception($"Yazar bulunamadı: {post.AuthorId}");
            }

            // PostDetailsVm oluştur
            var postDetailsVm = _mapper.Map<PostDetailsVm>(post);

            // Post resmi için tam URL oluştur
            postDetailsVm.ImagePath = $"https://localhost:7296/{post.ImagePath.TrimStart('/')}";

            // Yazar bilgilerini ve resmini ekle
            postDetailsVm.AuthorDetailVm = author;
            postDetailsVm.AuthorDetailVm.ImagePath = $"https://localhost:7296/{author.ImagePath.TrimStart('/')}";

            return postDetailsVm; // Post detaylarını döndür
        }

        // Belirli bir postu ID ile alma metodu
        public async Task<UpdatePostDTO> GetById(int id)
        {
            var post = await _postRepo.GetById(id); // Postu bul
            return _mapper.Map<UpdatePostDTO>(post); // Postu DTO'ya dönüştür
        }

        // Başlık ile postları alma metodu
        public async Task<List<PostVm>> GetPostsByTitle(string title)
        {
            var posts = await _postRepo.GetPostsByTitle(title); // Başlığa göre postları al
            return _mapper.Map<List<PostVm>>(posts); // Postları View Model'e dönüştür
        }

        // Yazar ID'sine göre postları alma metodu
        public async Task<List<PostVm>> GetPostsByAuthor(int authorId)
        {
            var posts = await _postRepo.GetPostsByAuthor(authorId); // Yazar ID'sine göre postları al
            return _mapper.Map<List<PostVm>>(posts); // Postları View Model'e dönüştür
        }

        // Tür ID'sine göre postları alma metodu
        public async Task<List<PostVm>> GetPostsByGenre(int genreId)
        {
            var posts = await _postRepo.GetPostsByGenre(genreId); // Tür ID'sine göre postları al
            return _mapper.Map<List<PostVm>>(posts); // Postları View Model'e dönüştür
        }

        // Tarih aralığına göre postları alma metodu
        public async Task<List<PostVm>> GetPostsByDateRange(DateTime startDate, DateTime endDate)
        {
            var posts = await _postRepo.GetPostsByDateRange(startDate, endDate); // Tarih aralığına göre postları al
            return _mapper.Map<List<PostVm>>(posts); // Postları View Model'e dönüştür
        }

        // Yazar ve tür bilgileri ile birlikte postları alma metodu
        public async Task<List<PostVm>> GetPostsWithAuthorAndGenre()
        {
            var posts = await _postRepo.GetPostsWithAuthorAndGenre(); // Yazar ve tür bilgileri ile postları al
            return _mapper.Map<List<PostVm>>(posts); // Postları View Model'e dönüştür
        }
    }
}

/*
    Açıklama
Yapıcı Metot (Constructor): PostManager sınıfının yapıcı metodu, AutoMapper, post repository'si ve yazar servisi gibi bağımlılıkları alır ve sınıfın içinde kullanılmak üzere atar.
Create Metodu: Yeni bir post oluşturmak için DTO'dan (Data Transfer Object) bir Post nesnesi oluşturur. Eğer bir resim yüklenmişse, resmi işleyerek belirtilen boyutlara getirir ve kaydeder.
Update Metodu: Mevcut bir postu güncelleyebilmek için önce mevcut postu alır, ardından modelden gelen verilerle günceller. Yine, yeni bir resim yüklenmişse resmi işleyerek güncellenmiş yolunu atar.
Delete Metodu: Belirtilen ID'ye sahip bir postu bulur ve eğer varsa siler.
GetPosts Metodu: Tüm postları alır ve bunları PostVm (View Model) nesnelerine dönüştürür.
GetDetail Metodu: Belirli bir postun detaylarını alır. Ayrıca yazarın bilgilerini almak için IAuthorService kullanılır.
GetById Metodu: Belirtilen ID'ye sahip postu alır ve UpdatePostDTO nesnesine dönüştürür.
GetPostsByTitle, GetPostsByAuthor, GetPostsByGenre, GetPostsByDateRange Metodları: Belirli filtrelere göre postları almak için kullanılır.
GetPostsWithAuthorAndGenre Metodu: Yazar ve tür bilgileri ile birlikte tüm postları alır.
Bu sınıf, post yönetimi ile ilgili tüm iş mantığını kapsar ve uygulama genelinde post işlemleri için merkezi bir yapı sağlar.
 */