using Microsoft.EntityFrameworkCore.Metadata.Builders;  // Entity Framework Core'un yapılandırma araçlarını kullanmak için gerekli
using OnionProject.Domain.Entities;  // Projedeki domain katmanında bulunan AppUser sınıfını kullanmak için gerekli
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Infrastructure.EntityTypeConfig
{
    // AppUser varlığını yapılandırmak için kullanılan konfigürasyon sınıfı
    public class AppUserConfig : BaseEntityConfig<AppUser>
    {
        // AppUser varlığı için özel yapılandırmalar burada yapılır
        public override void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // AppUser IdentityUser'dan kalıtım aldığı için burada o sınıfın özelliklerini de configure edebiliriz.
            // Id alanını birincil anahtar olarak belirler.
            builder.HasKey(e => e.Id);

            // NormalizedUserName özelliğinin zorunlu olduğunu belirtir (null olamaz).
            builder.Property(x => x.NormalizedUserName).IsRequired(true);

            // UserName özelliğinin de zorunlu olduğunu belirtir.
            builder.Property(x => x.UserName).IsRequired(true);

            // ImagePath özelliği opsiyoneldir, yani null olabilir.
            builder.Property(x => x.ImagePath).IsRequired(false);

            // BaseEntityConfig sınıfındaki ortak yapılandırmaları çağırır.
            base.Configure(builder);
        }
    }
}
