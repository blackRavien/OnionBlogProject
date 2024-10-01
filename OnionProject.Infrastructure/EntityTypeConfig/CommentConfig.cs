using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnionProject.Domain.Entities;

namespace OnionProject.Infrastructure.EntityTypeConfig
{
    // Comment varlığını yapılandırmak için kullanılan konfigürasyon sınıfı
    public class CommentConfig : BaseEntityConfig<Comment>
    {
        public override void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Content)
                .IsRequired(true)
                .HasMaxLength(500);

            builder.Property(x => x.PostId)
                .IsRequired(true);

            // UserId için foreign key tanımı
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId) // string tipli UserId olarak ayarladık
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            base.Configure(builder);
        }
    }

}
