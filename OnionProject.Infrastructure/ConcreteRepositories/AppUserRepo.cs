using Microsoft.AspNetCore.Identity;  // ASP.NET Core Identity ile ilgili sınıfları içerir
using Microsoft.EntityFrameworkCore;  // Entity Framework Core ile ilgili sınıfları içerir
using Microsoft.EntityFrameworkCore.Query;  // EF Core sorgu işlemleri için gerekli sınıfları içerir
using OnionProject.Domain.AbstractRepositories;  // Generic repository arayüzünü içerir
using OnionProject.Domain.Entities;  // Domain entity'lerini içerir
using OnionProject.Infrastructure.Context;  // DbContext sınıfını içerir
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.ConcreteRepositories
{
    // AppUser entity'si için concrete repository sınıfı
    public class AppUserRepo : BaseRepo<AppUser>, IAppUserRepo
    {
        // UserManager<AppUser> sınıfını kullanarak identity işlemlerini yönetir
        private readonly UserManager<AppUser> _userManager;

        // Constructor, DbContext ve UserManager'i dependency injection ile alır
        public AppUserRepo(AppDbContext context, UserManager<AppUser> userManager) : base(context)
        {
            _userManager = userManager; // UserManager'ı sınıf içinde kullanılabilir hale getirir
        }

        // Create işlemini override eder ve UserManager kullanarak yeni bir kullanıcı oluşturur
        public override Task Create(AppUser entity)
        {
            _userManager.CreateAsync(entity); // UserManager ile kullanıcıyı asenkron olarak oluşturur
            return base.Create(entity); // Base sınıfın Create metodunu çağırır
        }

        // Update işlemini override eder ve UserManager kullanarak kullanıcıyı günceller
        public override Task Update(AppUser entity)
        {
            _userManager.UpdateAsync(entity); // UserManager ile kullanıcıyı asenkron olarak günceller
            return base.Update(entity); // Base sınıfın Update metodunu çağırır
        }

        // Delete işlemini override eder ve UserManager kullanarak kullanıcıyı siler
        public override Task Delete(AppUser entity)
        {
            _userManager.DeleteAsync(entity); // UserManager ile kullanıcıyı asenkron olarak siler
            return base.Delete(entity); // Base sınıfın Delete metodunu çağırır
        }

        // Belirli bir koşulu sağlayan ilk kullanıcıyı getirir
        public async Task<AppUser> GetDefault(Expression<Func<AppUser, bool>> expression)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(expression); // Koşulu sağlayan ilk kullanıcıyı getirir
        }

        // Belirli bir koşulu sağlayan kullanıcıların listesini getirir
        public List<Task<AppUser>> GetDefaults(Expression<Func<AppUser, bool>> expression)
        {
            return _context.AppUsers.Where(expression).Select(x => Task.FromResult(x)).ToList(); // Koşulu sağlayan kullanıcıları listeler
        }

        // Belirli bir koşulu sağlayan herhangi bir kullanıcı olup olmadığını kontrol eder
        public List<bool> Any(Expression<Func<AppUser, bool>> expression)
        {
            return _context.AppUsers.Any(expression) ? new List<bool> { true } : new List<bool> { false }; // Koşulu sağlayan kullanıcı varsa true, yoksa false döner
        }

        // Filtreleme, sıralama ve ilişkili verileri dahil etme işlemleri ile tek bir kullanıcı getirir
        public async Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<AppUser, TResult>> select,
                                            Expression<Func<AppUser, bool>> where,
                                            Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>> orderBy = null,
                                            Func<IQueryable<AppUser>, IIncludableQueryable<AppUser, object>> include = null)
        {
            IQueryable<AppUser> query = _context.AppUsers; // Başlangıç query'si

            if (include != null)
                query = include(query); // İlişkili verileri dahil etme

            if (where != null)
                query = query.Where(where); // Filtreleme

            if (orderBy != null)
                return await orderBy(query).Select(select).FirstOrDefaultAsync(); // Sıralama ve ilk sonucu getir
            else
                return await query.Select(select).FirstOrDefaultAsync(); // Sıralama olmadan ilk sonucu getir
        }

        // Filtreleme, sıralama ve ilişkili verileri dahil etme işlemleri ile bir liste getirir
        public async Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<AppUser, TResult>> select,
                                            Expression<Func<AppUser, bool>> where,
                                            Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>> orderBy = null,
                                            Func<IQueryable<AppUser>, IIncludableQueryable<AppUser, object>> include = null)
        {
            IQueryable<AppUser> query = _context.AppUsers; // Başlangıç query'si

            if (include != null)
                query = include(query); // İlişkili verileri dahil etme

            if (where != null)
                query = query.Where(where); // Filtreleme

            if (orderBy != null)
                return await orderBy(query).Select(select).ToListAsync(); // Sıralama ve listeyi getir
            else
                return await query.Select(select).ToListAsync(); // Sıralama olmadan listeyi getir
        }

        // Kullanıcı adı ile kullanıcıyı getiren metod
        public async Task<AppUser> GetByUsername(string username)
        {
            return await _context.AppUsers
                .FirstOrDefaultAsync(u => u.UserName == username); // Kullanıcı adını kullanarak kullanıcıyı getirir
        }

        // E-posta ile kullanıcıyı getiren metod
        public async Task<AppUser> GetByEmail(string email)
        {
            return await _context.AppUsers
                .FirstOrDefaultAsync(u => u.Email == email); // E-posta adresini kullanarak kullanıcıyı getirir
        }

        public async Task<AppUser> GetById(string userId)
        {
            // Kullanıcı ID'si ile veritabanından kullanıcıyı asenkron olarak getirir.
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception($"Kullanıcı ID'si '{userId}' ile eşleşen bir kullanıcı bulunamadı."); // Kullanıcı bulunamazsa hata fırlatır
            }

            return user; // Kullanıcıyı döndürür
        }

        public async Task<List<AppUser>> GetAll()
        {
            // Tüm kullanıcıları veritabanından asenkron olarak getirir.
            return await _context.AppUsers.ToListAsync(); // Tüm kullanıcıları liste olarak döndürür
        }
    }
}



/*
Genel Özet:
Yukarıda verilen kod, AppUser adlı bir kullanıcı entity'si için bir repository sınıfı olan AppUserRepo'yu tanımlar. Bu sınıf, kullanıcı işlemlerini gerçekleştirmek için UserManager<AppUser> sınıfını kullanır. Repository, temel CRUD (Create, Read, Update, Delete) işlemlerini override ederek kullanıcıların oluşturulması, güncellenmesi ve silinmesi gibi işlemleri asenkron olarak gerçekleştirir. Ayrıca, kullanıcıları filtreleme, sıralama ve ilişkili verileri dahil etme işlemleri için çeşitli metotlar sağlar.

Sınıfın Temel İşlevleri:
Kullanıcı Oluşturma, Güncelleme ve Silme: Create, Update, Delete metotları, kullanıcı işlemlerini UserManager aracılığıyla gerçekleştirir.
Kullanıcı Bulma: Kullanıcı adı, e-posta ve kullanıcı ID'si gibi farklı kriterlere göre kullanıcıları bulmak için çeşitli metotlar sunar (GetByUsername, GetByEmail, GetById).
Filtreleme ve Sıralama: Kullanıcıları belirli kriterlere göre filtrelemek ve sıralamak için GetFilteredFirstOrDefault ve GetFilteredList metotlarını kullanır.
Koşul Kontrolü: Kullanıcıların var olup olmadığını kontrol etmek için Any metodu vardır.
Bu yapılar, uygulamanın kullanıcı yönetimini ve veritabanı etkileşimlerini düzenli ve verimli bir şekilde yapabilmesini sağlar.
 */