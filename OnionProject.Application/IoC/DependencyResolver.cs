using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac; // Dependency Injection (DI) için kullanılan IoC (Inversion of Control) kütüphanesi
using AutoMapper; // Nesne eşlemelerini (mapping) kolaylaştıran kütüphane
using OnionProject.Application.AutoMapper; // Projede oluşturulan AutoMapper profillerine erişim sağlar
using OnionProject.Application.Services.AbstractServices; // Servislerin interface'lerinin yer aldığı katman
using OnionProject.Application.Services.ConcreteManagers; // Servislerin somut implementasyonlarının (managerlar) yer aldığı katman
using OnionProject.Domain.AbstractRepositories; // Repository interface'lerinin bulunduğu domain katmanı
using OnionProject.Infrastructure.ConcreteRepositories; // Repository implementasyonlarının bulunduğu katman
using Module = Autofac.Module; // IoC için Autofac'in sunduğu module sınıfı

namespace OnionProject.Application.IoC // IoC (Dependency Injection) işlemlerini gerçekleştiren katman
{
    // Dependency injection işlemlerinin yapılandırıldığı sınıf
    public class DependencyResolver : Module
    {
        // IoC container'ına gerekli servis ve repository'lerin eklenmesini sağlar
        protected override void Load(ContainerBuilder builder)
        {
            // Repository'lerin Dependency Injection'a kaydedilmesi
            // Her repository interface'i için, ilgili somut sınıfın yaşam döngüsü scope seviyesinde olacak şekilde kaydediliyor
            builder.RegisterType<AuthorRepo>().As<IAuthorRepo>().InstancePerLifetimeScope(); // Yazar repository'si
            builder.RegisterType<GenreRepo>().As<IGenreRepo>().InstancePerLifetimeScope(); // Tür repository'si
            builder.RegisterType<AppUserRepo>().As<IAppUserRepo>().InstancePerLifetimeScope(); // Kullanıcı repository'si
            builder.RegisterType<PostRepo>().As<IPostRepo>().InstancePerLifetimeScope(); // Post repository'si
            builder.RegisterType<CommentRepo>().As<ICommentRepo>().InstancePerLifetimeScope(); // Yorum repository'si

            // Servislerin Dependency Injection'a kaydedilmesi
            // Her service interface'i için, ilgili somut sınıfın yaşam döngüsü scope seviyesinde olacak şekilde kaydediliyor
            builder.RegisterType<AuthorManager>().As<IAuthorService>().InstancePerLifetimeScope(); // Yazar servisi
            builder.RegisterType<PostManager>().As<IPostService>().InstancePerLifetimeScope(); // Post servisi
            builder.RegisterType<GenreManager>().As<IGenreService>().InstancePerLifetimeScope(); // Tür servisi
            builder.RegisterType<CommentManager>().As<ICommentService>().InstancePerLifetimeScope(); // Yorum servisi
            builder.RegisterType<ContactMessageRepo>().As<IContactMessageRepo>().InstancePerLifetimeScope(); // İletişim mesajları repository'si
            builder.RegisterType<ContactMessageManager>().As<IContactMessageService>().InstancePerLifetimeScope(); // İletişim mesajları servisi
            builder.RegisterType<AppUserManager>().As<IAppUserService>().InstancePerLifetimeScope(); // Kullanıcı servisi

            // AutoMapper yapılandırmasının Dependency Injection'a kaydedilmesi
            builder.Register(context => new MapperConfiguration(config =>
                // Mapper profilleri kaydediliyor
                config.AddProfile<Mapping>() // Mapping sınıfı kullanılarak tüm map işlemleri yükleniyor
            )).AsSelf().SingleInstance(); // Bu yapılandırma tek bir instance olarak çalışacak

            // AutoMapper için IMapper interface'inin Dependency Injection'a eklenmesi
            builder.Register(c =>
            {
                var context = c.Resolve<IComponentContext>(); // Component context'i çözülüyor
                var config = c.Resolve<MapperConfiguration>(); // Mapper configuration çözülüyor

                return config.CreateMapper(context.Resolve); // Mapper nesnesi oluşturuluyor
            }).As<IMapper>().InstancePerLifetimeScope(); // Mapper yaşam döngüsü scope seviyesinde olacak şekilde kaydediliyor

            // Diğer servis ve managerlar burada kaydedilebilir...
            // Eğer eklenmesi gereken başka manager ya da servis varsa, buraya eklenir

            // En altta kalması gerektiği belirtilen base.Load(builder) çağrısı, mevcut Autofac yapılandırmalarını tamamlar
            base.Load(builder); // Autofac'in varsayılan `Load` işlemi gerçekleştirilir
        }
    }
}

/*
    Genel Özet:
DependencyResolver.cs dosyası, Autofac kullanarak dependency injection (bağımlılıkların otomatik çözülmesi) işlemlerini yönetir. Bu sınıf, proje içerisindeki servisler, repository'ler ve AutoMapper konfigürasyonlarını IoC (Inversion of Control) konteynerine ekleyerek, projenin belirli bölümlerinde otomatik olarak çözülmesini sağlar. Bu, bağımlılıkları manuel olarak çözmek yerine Autofac tarafından otomatik olarak yönetilmesini sağlar ve projenin modüler, test edilebilir ve sürdürülebilir olmasına katkı sağlar.
 */