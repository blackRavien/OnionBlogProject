using Microsoft.EntityFrameworkCore.Metadata.Builders;  // Entity Framework Core'un yapılandırma araçlarını kullanmak için gerekli
using OnionProject.Domain.Entities;  // Projedeki domain katmanında bulunan Author sınıfını kullanmak için gerekli
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.EntityTypeConfig
{
    // Author varlığını yapılandırmak için kullanılan konfigürasyon sınıfı
    public class AuthorConfig : BaseEntityConfig<Author>
    {
        // Author varlığı için özel yapılandırmalar burada yapılır
        public override void Configure(EntityTypeBuilder<Author> builder)
        {
            // Id alanını birincil anahtar (Primary Key) olarak belirler.
            builder.HasKey(x => x.Id); // PK olması için ve identity spesification yes olması için.

            // FirstName alanı zorunludur ve maksimum 50 karakter uzunluğundadır.
            builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);

            // LastName alanı zorunludur ve maksimum 75 karakter uzunluğundadır.
            builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(75);

            // ImagePath alanı zorunludur.
            builder.Property(x => x.ImagePath).IsRequired(true);

            // BaseEntityConfig sınıfındaki ortak yapılandırmaları çağırır (CreatedDate, UpdatedDate vb.).
            base.Configure(builder);
        }
    }
}
