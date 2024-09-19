using Microsoft.EntityFrameworkCore.Metadata.Builders;  // Entity Framework Core'un yapılandırma araçlarını kullanmak için gerekli
using OnionProject.Domain.Entities;  // Projedeki domain katmanında bulunan Genre sınıfını kullanmak için gerekli
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.EntityTypeConfig
{
    // Genre varlığını yapılandırmak için kullanılan konfigürasyon sınıfı
    public class GenreConfig : BaseEntityConfig<Genre>
    {
        // Genre varlığı için özel yapılandırmalar burada yapılır
        public override void Configure(EntityTypeBuilder<Genre> builder)
        {
            // Id alanını birincil anahtar (Primary Key) olarak belirler.
            builder.HasKey(x => x.Id); // PK

            // Name alanının zorunlu olduğunu belirtir.
            builder.Property(x => x.Name).IsRequired(true);

            // Genre sınıfının alanları
            // public int Id { get; set; }
            // public string Name { get; set; }

            // BaseEntityConfig sınıfındaki ortak yapılandırmaları çağırır (CreatedDate, UpdatedDate vb.).
            base.Configure(builder);
        }
    }
}
