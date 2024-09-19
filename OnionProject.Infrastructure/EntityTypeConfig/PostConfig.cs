using Microsoft.EntityFrameworkCore;  // Entity Framework Core ana kütüphanesi
using Microsoft.EntityFrameworkCore.Metadata.Builders;  // Entity Framework Core yapılandırma araçları
using OnionProject.Domain.Entities;  // Domain katmanındaki Post sınıfını kullanmak için gerekli
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.EntityTypeConfig
{
    // Post varlığını yapılandırmak için kullanılan konfigürasyon sınıfı
    public class PostConfig : BaseEntityConfig<Post>
    {
        // Post varlığı için özel yapılandırmalar burada yapılır
        public override void Configure(EntityTypeBuilder<Post> builder)
        {
            // Id alanını birincil anahtar (Primary Key) olarak belirler.
            builder.HasKey(x => x.Id); // PK

            // Title ve Content alanlarının zorunlu olduğunu belirtir.
            builder.Property(x => x.Title).IsRequired(true);
            builder.Property(x => x.Content).IsRequired(true);

            // ImagePath alanının da zorunlu olduğunu belirtir.
            builder.Property(x => x.ImagePath).IsRequired(true);

            // Post sınıfının alanları:
            // public int Id { get; set; }
            // public string Title { get; set; }
            // public string Content { get; set; }
            // public string ImagePath { get; set; } 

            // Navigasyon özellikleri (ilişkiler)

            // Bir Post'un bir Author'a (Yazar) ait olduğunu, ancak bir Author'un birden fazla Post'u olabileceğini belirtir.
            builder.HasOne(x => x.Author) // Bir Post'un bir Yazarı olur
                .WithMany(x => x.Posts)   // Bir Yazarın birden fazla Post'u olabilir
                .HasForeignKey(x => x.AuthorId)  // Post tablosundaki foreign key, AuthorId'dir
                .OnDelete(DeleteBehavior.Restrict);  // Yazar silinirse, ona bağlı Post'lar silinmez.

            // Bir Post'un bir Genre'ye (Tür) ait olduğunu, ancak bir Genre'nin birden fazla Post'u olabileceğini belirtir.
            builder.HasOne(x => x.Genre)  // Bir Post'un bir Türü olur
                .WithMany(x => x.Posts)   // Bir Türün birden fazla Post'u olabilir
                .HasForeignKey(x => x.GenreId)  // Post tablosundaki foreign key, GenreId'dir
                .OnDelete(DeleteBehavior.Restrict);  // Tür silinirse, ona bağlı Post'lar silinmez.

            // BaseEntityConfig sınıfındaki ortak yapılandırmaları çağırır (CreatedDate, UpdatedDate vb.).
            base.Configure(builder);
        }
    }
}



//Bir yazarın
//birden fazla Postu olur 
//fk de post daki AuthorId dir
//Parent table dan silme işlemi yapıldığında child da bağlı veri varsa silme işlemi yapmaz.

