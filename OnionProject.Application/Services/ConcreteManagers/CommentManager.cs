using Microsoft.EntityFrameworkCore; // Entity Framework Core ile çalışmak için gerekli olan isim alanı
using OnionProject.Application.Models.DTOs; // DTO sınıflarını kullanmak için gerekli olan isim alanı
using OnionProject.Application.Services.AbstractServices; // Yorum hizmetinin arayüzünü kullanmak için gerekli olan isim alanı
using OnionProject.Domain.AbstractRepositories; // Repository arayüzlerini kullanmak için gerekli olan isim alanı
using OnionProject.Domain.Entities; // Domain varlıklarını kullanmak için gerekli olan isim alanı
using OnionProject.Infrastructure.Context; // Uygulamanın veritabanı bağlamını kullanmak için gerekli olan isim alanı
using System; // Temel sistem işlevleri için gerekli olan isim alanı
using System.Collections.Generic; // Koleksiyonlar için gerekli olan isim alanı
using System.Linq; // LINQ sorguları için gerekli olan isim alanı
using System.Text; // String işleme için gerekli olan isim alanı
using System.Threading.Tasks; // Asenkron programlama için gerekli olan isim alanı

namespace OnionProject.Application.Services.ConcreteManagers
{
    // Yorum hizmetinin uygulanması
    public class CommentManager : ICommentService
    {
        private readonly ICommentRepo _commentRepository; // Yorum veritabanı işlemleri için repository
        private readonly AppDbContext _context; // Uygulamanın veritabanı bağlamı

        // Yapıcı metod, bağımlılık enjeksiyonu ile gerekli bileşenleri alır
        public CommentManager(ICommentRepo commentRepository, AppDbContext context)
        {
            _commentRepository = commentRepository; // Yorum repository'sini atama
            _context = context; // Veritabanı bağlamını atama
        }

        // Yeni bir yorum ekler
        public async Task AddCommentAsync(CreateCommentDTO commentDto)
        {
            var comment = new Comment // Yeni bir Comment nesnesi oluşturma
            {
                Content = commentDto.Content, // İçeriği ayarlama
                PostId = commentDto.PostId, // İlgili gönderi ID'sini ayarlama
                UserId = commentDto.UserId, // Kullanıcı ID'sini ayarlama
            };
            await _commentRepository.AddAsync(comment); // Repository üzerinden yeni yorumu ekleme
        }

        // Belirli bir yorumu siler
        public async Task DeleteCommentAsync(int commentId)
        {
            // Yorumu silmek için repository üzerinden silme işlemini gerçekleştir
            await _commentRepository.DeleteAsync(commentId); // Belirtilen ID'ye sahip yorumu silme
        }

        // Belirli bir gönderi için tüm yorumları alır
        public async Task<List<GetCommentDTO>> GetCommentsByPostIdAsync(int postId)
        {
            var comments = await _commentRepository.GetByPostIdAsync(postId); // Belirtilen gönderi için yorumları al

            // Dönüşüm işlemi
            return comments.Select(c => new GetCommentDTO // Yorumları DTO'ya dönüştürme
            {
                Id = c.Id, // Yorum ID'sini ayarlama
                Content = c.Content, // Yorum içeriğini ayarlama
                PostId = c.PostId, // İlgili gönderi ID'sini ayarlama
                UserId = c.UserId, // Kullanıcı ID'sini ayarlama
                CreatedAt = c.CreatedAt, // Yorumun oluşturulma tarihini ayarlama
                UserName = _context.Users // Kullanıcının ismini almak için sorgu
                .Where(u => u.Id == c.UserId) // Kullanıcı ID'sine göre filtreleme
                .Select(u => u.UserName) // Kullanıcının ismini seçme
                .FirstOrDefault() // Kullanıcının UserName'ini getiriyoruz
            }).ToList(); // Sonuçları listeye dönüştürme
        }
    }
}

/*
    Açıklamalar
Namespace Kullanımı:

Microsoft.EntityFrameworkCore: Entity Framework Core ile etkileşim kurmak için gerekli olan isim alanıdır. Veritabanı işlemleri için kullanılır.
OnionProject.Application.Models.DTOs: DTO sınıflarını içeren isim alanıdır. Verilerin taşınması ve yönetilmesi için kullanılır.
OnionProject.Application.Services.AbstractServices: Yorum hizmetinin arayüzünü içeren isim alanıdır.
OnionProject.Domain.AbstractRepositories: Repository arayüzlerini içeren isim alanıdır.
OnionProject.Domain.Entities: Uygulamanın domain varlıklarını içeren isim alanıdır.
OnionProject.Infrastructure.Context: Uygulamanın veritabanı bağlamını içeren isim alanıdır.
İşlevsellik:

CommentManager Sınıfı: ICommentService arayüzünü uygulayan sınıf. Yorum işlemleriyle ilgili işlevselliği sağlar.

Yapıcı Metod:

ICommentRepo commentRepository: Yorumları yönetmek için kullanılan repository.
AppDbContext context: Uygulamanın veritabanı bağlamı. Veritabanı işlemlerini gerçekleştirmek için kullanılır.
AddCommentAsync Metodu:

CreateCommentDTO commentDto: Yorum eklemek için gereken verileri içeren DTO nesnesi.
Yorum nesnesi oluşturulur ve repository üzerinden eklenir.
DeleteCommentAsync Metodu:

int commentId: Silinmesi istenen yorumun ID'sini alır.
Repository aracılığıyla belirtilen ID'ye sahip yorum silinir.
GetCommentsByPostIdAsync Metodu:

int postId: Yorumlarını almak istediğimiz gönderinin ID'sini alır.
Belirli bir gönderiye ait tüm yorumları alır ve bunları GetCommentDTO nesnelerine dönüştürerek döndürür.
Kullanıcı isimleri, _context.Users üzerinden sorgulanarak elde edilir.
Genel Değerlendirme
Bu sınıf, yorumlarla ilgili temel işlemleri gerçekleştirir: yeni yorum eklemek, yorum silmek ve belirli bir gönderiye ait yorumları almak. CommentManager, bağımlılık enjeksiyonu ile repository ve veritabanı bağlamı gibi bileşenleri alarak bu işlemleri gerçekleştirecek işlevselliği sağlar. Asenkron metodlar kullanarak uygulamanın performansını artırır ve kullanıcı arayüzünde daha akıcı bir deneyim sunar.
 */