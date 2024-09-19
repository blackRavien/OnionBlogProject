using Microsoft.EntityFrameworkCore.Query;  // Entity Framework Core için sorgu özelliklerini içerir
using OnionProject.Domain.Entities;  // Domain entity'lerini içerir
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

//Bu kod parçası, Onion Architecture'da Domain katmanında yer alan bir Repository arayüzünü (IBaseRepo<T>) tanımlar. Repository deseninin amacı, veri erişim işlemlerini merkezi bir yerden yönetmek ve domain logic'ten ayırmaktır. Bu arayüz, genel veri erişim metodlarını tanımlar ve tüm entity'ler için ortak işlemleri sağlar.

namespace OnionProject.Domain.AbstractRepositories
{
    // Generic repository arayüzü
    public interface IBaseRepo<T> where T : IBaseEntity
    {
        // Yeni bir entity oluşturur
        Task Create(T entity);

        // Varolan bir entity'yi günceller
        Task Update(T entity);

        // Bir entity'yi siler
        Task Delete(T entity);

        // Belirli bir koşulu sağlayan tek bir entity'yi getirir
        Task<T> GetDefault(Expression<Func<T, bool>> expression);

        // Belirli bir koşulu sağlayan entity'lerin listesini getirir
        Task<List<T>> GetDefaults(Expression<Func<T, bool>> expression);

        // Koşula uyan herhangi bir entity olup olmadığını kontrol eder
        Task<bool> Any(Expression<Func<T, bool>> expression);

        // Filtreleme, sıralama ve ilişkili verileri dahil etme işlemleri ile tek bir sonuç getirir
        Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> select,
                                                Expression<Func<T, bool>> where,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);

        // Filtreleme, sıralama ve ilişkili verileri dahil etme işlemleri ile bir liste getirir
        Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select,
                                                Expression<Func<T, bool>> where,
                                                Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    }
}


/*
        Açıklamalar:
Genel Tanım:
IBaseRepo<T>: Bu, IBaseEntity arayüzünü implemente eden herhangi bir entity için kullanılabilecek genel bir repository arayüzüdür. Generic (şablon) yapıda olup, farklı entity türleri için kullanılabilir.
Metodlar:
Task Create(T entity):

Açıklama: Veritabanına yeni bir entity ekler.
Parametre: T entity - Veritabanına eklenmek istenen entity.
Dönüş Tipi: Task - Asenkron işlem tamamlandığında geri döner.
Task Update(T entity):

Açıklama: Veritabanındaki mevcut bir entity'yi günceller.
Parametre: T entity - Güncellenmesi gereken entity.
Dönüş Tipi: Task - Asenkron işlem tamamlandığında geri döner.
Task Delete(T entity):

Açıklama: Veritabanından bir entity'yi siler.
Parametre: T entity - Silinmesi gereken entity.
Dönüş Tipi: Task - Asenkron işlem tamamlandığında geri döner.
Task<T> GetDefault(Expression<Func<T, bool>> expression):

Açıklama: Belirli bir koşulu sağlayan tek bir entity'yi getirir.
Parametre: Expression<Func<T, bool>> expression - Filtreleme koşulu (LINQ ifadesi).
Dönüş Tipi: Task<T> - Asenkron işlem sonucunda bulunan entity.
Task<List<T>> GetDefaults(Expression<Func<T, bool>> expression):

Açıklama: Belirli bir koşulu sağlayan entity'lerin listesini getirir.
Parametre: Expression<Func<T, bool>> expression - Filtreleme koşulu (LINQ ifadesi).
Dönüş Tipi: Task<List<T>> - Asenkron işlem sonucunda bulunan entity listesi.
Task<bool> Any(Expression<Func<T, bool>> expression):

Açıklama: Veritabanında belirtilen koşula uyan herhangi bir entity olup olmadığını kontrol eder.
Parametre: Expression<Func<T, bool>> expression - Koşul (LINQ ifadesi).
Dönüş Tipi: Task<bool> - Asenkron işlem sonucunda true veya false döner.
Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null):

Açıklama: Filtreleme, sıralama ve ilişkili verileri dahil etme işlemlerini uygulayarak, bir TResult türünde tek bir sonuç getirir.
Parametreler:
Expression<Func<T, TResult>> select - Getirilecek sonuç türünü belirler (projection).
Expression<Func<T, bool>> where - Filtreleme koşulu (LINQ ifadesi).
Func<IQueryable<T>, IOrderedQueryable<T>> orderBy - (Opsiyonel) Sıralama fonksiyonu.
Func<IQueryable<T>, IIncludableQueryable<T, object>> include - (Opsiyonel) İlişkili verileri dahil etme fonksiyonu.
Dönüş Tipi: Task<TResult> - Asenkron işlem sonucunda elde edilen TResult türünde tek bir sonuç.
Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null):

Açıklama: Filtreleme, sıralama ve ilişkili verileri dahil etme işlemlerini uygulayarak, bir List<TResult> türünde sonuç listesi getirir.
Parametreler:
Expression<Func<T, TResult>> select - Getirilecek sonuç türünü belirler (projection).
Expression<Func<T, bool>> where - Filtreleme koşulu (LINQ ifadesi).
Func<IQueryable<T>, IOrderedQueryable<T>> orderBy - (Opsiyonel) Sıralama fonksiyonu.
Func<IQueryable<T>, IIncludableQueryable<T, object>> include - (Opsiyonel) İlişkili verileri dahil etme fonksiyonu.
Dönüş Tipi: Task<List<TResult>> - Asenkron işlem sonucunda elde edilen TResult türünde liste.
 */

