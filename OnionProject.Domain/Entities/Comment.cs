using OnionProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Domain.Entities
{
    public class Comment : IBaseEntity
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; } // Hangi post'a ait olduğunu belirlemek için
        public int? AuthorId { get; set; } // Yorum yapabilen Admin kullanıcısı
        public string? UserId { get; set; } // Yorum yapan standart kullanıcı (ileride dahil edilecek)
        //public string UserName { get; set; } //kullanıcı adı sonradan eklendi
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Post Post { get; set; } // İlişkilendirme
        public virtual Author? Author { get; set; } // Yeni ilişki
        public virtual AppUser? User { get; set; } //User ile İlişkilendirme (ileride)
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }
    }

}
