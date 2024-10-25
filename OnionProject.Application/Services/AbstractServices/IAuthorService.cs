using OnionProject.Application.Models.DTOs; // DTO sınıflarını içe aktarıyor.
using OnionProject.Application.Models.VMs; // ViewModel sınıflarını içe aktarıyor.
using System;
using System.Collections.Generic; // Liste koleksiyonları için gerekli.
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Services.AbstractServices
{
    // Yazar hizmetleri için bir arayüz tanımlıyor.
    public interface IAuthorService
    {
        // Yeni bir yazar oluşturmak için kullanılan metod.
        Task Create(CreateAuthorDTO model);

        // Mevcut bir yazarı güncellemek için kullanılan metod.
        Task Update(UpdateAuthorDTO model);

        // Belirtilen ID'ye sahip bir yazarı silmek için kullanılan metod.
        Task Delete(int id);

        // Tüm yazarları listelemek için kullanılan metod.
        Task<List<AuthorVm>> GetAuthors();

        // Belirtilen ID'ye sahip bir yazarın detaylarını almak için kullanılan metod.
        Task<AuthorDetailVm> GetDetail(int id);

        // Belirtilen ID'ye sahip bir yazarı almak için kullanılan metod.
        Task<UpdateAuthorDTO> GetById(int id);

        // Belirtilen ad ve soyadı olan bir yazarın mevcut olup olmadığını kontrol etmek için kullanılan metod.
        Task<bool> IsAuthorExists(string firstName, string lastName);

        // Belirtilen ad ve soyadı olan bir yazarın tam adını almak için kullanılan metod.
        Task<AuthorDetailVm> GetFullName(string firstName, string lastName);
    }
}
/*
 Detaylı Açıklama:
Namespace İçe Aktarma:

using OnionProject.Application.Models.DTOs;: Uygulama düzeyindeki DTO (Data Transfer Object) sınıflarını içe aktarıyor.
using OnionProject.Application.Models.VMs;: Uygulama düzeyindeki ViewModel (VM) sınıflarını içe aktarıyor.
using System.Collections.Generic;: Liste gibi koleksiyon türlerini kullanabilmek için gerekli olan namespace.
Arayüz Tanımı (IAuthorService):

public interface IAuthorService: IAuthorService adlı bir arayüz tanımlıyor. Bu arayüz, yazarlarla ilgili hizmetlerin (servislerin) uygulanması için gerekli metotları içeriyor.
Metod Tanımları:

Task Create(CreateAuthorDTO model);: Yeni bir yazar oluşturmak için bir metod. CreateAuthorDTO modelini parametre olarak alır ve bir görev (task) döner.
Task Update(UpdateAuthorDTO model);: Mevcut bir yazarı güncellemek için bir metod. UpdateAuthorDTO modelini alır ve bir görev döner.
Task Delete(int id);: Belirli bir yazarın ID'sine göre silinmesini sağlayan bir metod.
Task<List<AuthorVm>> GetAuthors();: Tüm yazarları listeleyen bir metod. AuthorVm (ViewModel) nesnelerinin bir listesini döner.
Task<AuthorDetailVm> GetDetail(int id);: Belirli bir yazarın detaylarını almak için bir metod. Yazar ID'sini alır ve AuthorDetailVm nesnesini döner.
Task<UpdateAuthorDTO> GetById(int id);: Belirli bir ID'ye sahip yazarı almak için bir metod. UpdateAuthorDTO modelini döner.
Task<bool> IsAuthorExists(string firstName, string lastName);: Belirli bir ad ve soyadı olan bir yazarın veritabanında mevcut olup olmadığını kontrol eden bir metod. Boolean bir değer döner.
Task<AuthorDetailVm> GetFullName(string firstName, string lastName);: Belirli bir ad ve soyadı ile yazarın tam adını almak için kullanılan bir metod. AuthorDetailVm nesnesini döner.
Bu arayüz, yazarlar üzerinde CRUD (Create, Read, Update, Delete) işlemleri yapmak için gerekli olan tüm metodları tanımlar. Arayüz, bağımsız bir yapı sunarak, uygulamanın ilerideki geliştirmelerine kolaylık sağlar. 
    
    */