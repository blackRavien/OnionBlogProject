using Microsoft.EntityFrameworkCore;  // Entity Framework Core kütüphanesi için gerekli namespace
using Microsoft.EntityFrameworkCore.Query;  // IQueryable ve IIncludableQueryable için gerekli namespace
using OnionProject.Domain.AbstractRepositories;  // Projenizin abstract repository arayüzlerini içeren namespace
using OnionProject.Domain.Entities;  // Domain katmanındaki entity sınıflarını içeren namespace
using OnionProject.Infrastructure.Context;  // Projenizin veri bağlamını içeren namespace
using System;  // Genel .NET türleri ve işlevleri için namespace
using System.Collections.Generic;  // Koleksiyonlar için namespace
using System.Linq;  // LINQ sorguları için namespace
using System.Linq.Expressions;  // LINQ ifadeleri için namespace
using System.Threading.Tasks;  // Asenkron işlemler için namespace

namespace OnionProject.Infrastructure.ConcreteRepositories
{
    // BaseRepo sınıfı, genel repository işlevlerini uygulayan bir sınıf
    public class BaseRepo<T> : IBaseRepo<T> where T : class, IBaseEntity
    {
        protected readonly AppDbContext _context;  // Veri bağlamını tutan özel alan
        private readonly DbSet<T> _dbSet;  // DbSet<T> nesnesi, entity'leri temsil eder

        // Constructor, AppDbContext nesnesini alır ve DbSet<T> nesnesini başlatır
        public BaseRepo(AppDbContext context)
        {
            _context = context;  // Veri bağlamını ayarla
            _dbSet = _context.Set<T>();  // İlgili DbSet<T> nesnesini al
        }

        // Asenkron Any metodu, verilen ifade ile herhangi bir kayıt olup olmadığını kontrol eder
        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            // _dbSet üzerinden belirtilen ifade ile herhangi bir kayıt olup olmadığını kontrol et
            return await _dbSet.AnyAsync(expression);
        }

        // Create metodu, yeni bir entity oluşturur
        public virtual async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);  // Entity'yi DbSet'e ekle
            await _context.SaveChangesAsync();  // Değişiklikleri veritabanına kaydet
        }

        // Update metodu, var olan bir entity'yi günceller
        public virtual async Task Update(T entity)
        {
            _dbSet.Update(entity);  // Entity'yi DbSet'te güncelle
            await _context.SaveChangesAsync();  // Değişiklikleri veritabanına kaydet
        }

        // Delete metodu, var olan bir entity'yi siler
        public virtual async Task Delete(T entity)
        {
            _dbSet.Remove(entity);  // Entity'yi DbSet'ten kaldır
            await _context.SaveChangesAsync();  // Değişiklikleri veritabanına kaydet
        }

        // GetDefault metodu, verilen ifade ile eşleşen ilk entity'yi getirir
        public async Task<T> GetDefault(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);  // İlk eşleşeni getir
        }

        // GetDefaults metodu, verilen ifade ile eşleşen tüm entity'leri getirir
        public async Task<List<T>> GetDefaults(Expression<Func<T, bool>> expression)
        {
            // Verilen ifade ile filtrelenmiş tüm entity'leri asenkron olarak liste şeklinde getir
            return await _dbSet.Where(expression).ToListAsync();
        }


        // GetFilteredFirstOrDefault metodu, filtrelenmiş ve sıralanmış ilk sonucu getirir
        public async Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> select,
                                                Expression<Func<T, bool>> where,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet;  // Sorgu için başlangıç noktasını al

            if (include != null)
                query = include(query);  // İlişkili verileri dahil et

            if (where != null)
                query = query.Where(where);  // Filtre uygula

            if (orderBy != null)
                return await orderBy(query).Select(select).FirstOrDefaultAsync();  // Sıralama uygula ve ilk sonucu getir
            else
                return await query.Select(select).FirstOrDefaultAsync();  // Sıralama olmadan ilk sonucu getir
        }

        // GetFilteredList metodu, filtrelenmiş ve sıralanmış tüm sonuçları getirir
        public async Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select,
                                                Expression<Func<T, bool>> where,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet;  // Sorgu için başlangıç noktasını al

            if (include != null)
                query = include(query);  // İlişkili verileri dahil et

            if (where != null)
                query = query.Where(where);  // Filtre uygula

            if (orderBy != null)
                return await orderBy(query).Select(select).ToListAsync();  // Sıralama uygula ve tüm sonuçları getir
            else
                return await query.Select(select).ToListAsync();  // Sıralama olmadan tüm sonuçları getir
        }
    }
}


