using Microsoft.AspNetCore.Identity;  // Identity ile ilgili sınıflar için gerekli
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;  // IdentityDbContext sınıfını kullanmak için gerekli
using Microsoft.EntityFrameworkCore;  // Entity Framework Core sınıfları için gerekli
using OnionProject.Domain.Entities;  // Domain entity sınıflarını kullanmak için gerekli
using OnionProject.Infrastructure.EntityTypeConfig;  // Entity konfigürasyonlarını içeren sınıflar için gerekli
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.Context
{
    // AppDbContext, Entity Framework Core'un veritabanı bağlamı (DbContext) sınıfıdır.
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        // Varsayılan constructor
        public AppDbContext()
        {
        }

        // DbContextOptions ile yapılandırma yapılabilen constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Veritabanında kullanılacak entity'lerin DbSet'leri
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Post> Posts { get; set; }
        

        // DbContext'in yapılandırılması (örneğin, bağlantı dizesi ayarları)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-JCTOH8S;Database=YZL3171BlogDB;Uid=sa;Pwd=123;TrustServerCertificate=True;");
        }

        // Entity konfigürasyonlarının uygulanması ve veritabanı başlangıç verilerinin eklenmesi
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Fluent API konfigürasyonlarının uygulanması
            builder.ApplyConfiguration(new AppUserConfig());
            builder.ApplyConfiguration(new AuthorConfig());
            builder.ApplyConfiguration(new GenreConfig());
            builder.ApplyConfiguration(new PostConfig());

            // Seed data olarak rol ekleme (örneğin, Admin ve Member rolleri)
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = new Guid().ToString()
                },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Member",
                    NormalizedName = "MEMBER",
                    ConcurrencyStamp = new Guid().ToString()
                }
            );

            // Temel yapılandırmaları çağır en altta kalsın.
            base.OnModelCreating(builder);
        }
    }
}


/*
        Açıklamalar:
Constructor:
Varsayılan Constructor: Boş constructor, DbContext'in varsayılan şekilde başlatılmasını sağlar.

DbContextOptions ile Constructor: Bu constructor, dışarıdan DbContext konfigürasyonları (örneğin, bağlantı dizesi) geçirilmesini sağlar. Bu constructor genellikle Dependency Injection (DI) ile kullanılır.

DbSet'ler:
DbSet<AppUser> AppUsers: AppUser entity'si için veritabanında bir tablo sağlar.

DbSet<Author> Authors: Author entity'si için veritabanında bir tablo sağlar.

DbSet<Genre> Genres: Genre entity'si için veritabanında bir tablo sağlar.

DbSet<Post> Posts: Post entity'si için veritabanında bir tablo sağlar.

OnConfiguring:
OnConfiguring: Veritabanı bağlantı dizesini (connection string) ayarlamak için kullanılır. Bu metod, Entity Framework Core'un veritabanına nasıl bağlanacağını belirtir. Kodda SQL Server kullanıldığı belirtilmiş ve bağlantı dizesi verilmiştir.
OnModelCreating:
OnModelCreating: Bu metod, entity konfigürasyonlarını (Fluent API ile yapılan ayarları) ve veritabanı başlangıç verilerini (seed data) uygulamak için kullanılır.

builder.ApplyConfiguration(): Fluent API konfigürasyonlarını uygulamak için kullanılır. Bu metod, AppUserConfig, AuthorConfig, GenreConfig, ve PostConfig sınıflarının konfigürasyonlarını içerir ve entity'lerin nasıl yapılandırılacağını belirtir.

builder.Entity<IdentityRole>().HasData(): Veritabanına başlangıç verileri ekler. Bu örnekte, IdentityRole (rol) tablosuna Admin ve Member adında iki rol eklenir. Bu işlemler, uygulamanın ilk çalıştırıldığında veritabanına belirli rollerin eklenmesini sağlar.

base.OnModelCreating(builder): Taban sınıfın (IdentityDbContext'in) OnModelCreating metodunu çağırır ve var olan yapılandırmaları korur.
 */