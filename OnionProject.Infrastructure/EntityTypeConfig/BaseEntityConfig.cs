using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Bu kod, Entity Framework Core kullanılarak bir entity'yi (varlık) yapılandırmak için kullanılan bir sınıf tanımını içeriyor. 

namespace OnionProject.Infrastructure.EntityTypeConfig
{
    /*
        BaseEntityConfig<T>: Bu sınıf bir generic class'tır. T tipi, konfigüre edilmek istenen varlık (entity) tipi olabilir. Bu sınıf, IEntityTypeConfiguration<T> arayüzünü uyguluyor.

        IEntityTypeConfiguration<T> arayüzü, EF Core ile bir varlığın nasıl yapılandırılacağını belirlemek için kullanılır. Bu sınıfı generic yaparak, farklı varlıklar için yapılandırmaları tekrar tekrar yazmak yerine ortak yapılandırmayı kullanmayı sağlar.
        
        where T : class, IBaseEntity: Bu, T tipinin bir sınıf ve aynı zamanda IBaseEntity arayüzünden türemesi gerektiğini belirtir. Yani, sadece IBaseEntity arayüzünü uygulayan varlıklar bu sınıf tarafından yapılandırılabilir.
     */
    public abstract class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : class, IBaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.UpdatedDate).IsRequired(false);
            builder.Property(x => x.DeletedDate).IsRequired(false);
            builder.Property(x => x.Status).IsRequired(true);
        }
    }

    /*
     3. Configure Metodu
        Bu metod, ilgili varlığın veritabanında nasıl yapılandırılacağını belirtir:

        builder.Property(x => x.CreatedDate).IsRequired(true);

        CreatedDate özelliğinin zorunlu bir alan olduğunu belirtir.
        builder.Property(x => x.UpdatedDate).IsRequired(false);
        
        UpdatedDate opsiyoneldir, yani null olabilir.
        builder.Property(x => x.DeletedDate).IsRequired(false);
        
        DeletedDate de opsiyoneldir.
        builder.Property(x => x.Status).IsRequired(true);
        
        Status zorunlu bir alandır.
        Bu yapılandırmalar, veritabanında ilgili varlıkların kolonlarının gerekliliklerini belirtir.      Örneğin, CreatedDate ve Status değerleri her zaman bir değer almak zorundayken,     UpdatedDate ve     DeletedDate boş olabilir.
        
        Genel Amacı
        Bu sınıf, bir base (temel) konfigürasyon sınıfı olarak hizmet eder. Başka varlık sınıfları bu sınıftan türeyerek, ortak özelliklerin (CreatedDate, UpdatedDate, DeletedDate, Status) konfigürasyonunu alır. Bu, her entity (varlık) için aynı özellikleri tekrar tekrar yapılandırmak yerine, tek bir yerde yapılandırma yapmayı sağlar ve kod tekrarını önler.
     */
}
