using System;

namespace OnionProject.Application.Models.VMs
{
    // Yorum bilgilerini içeren ViewModel sınıfı
    public class CommentVm
    {
        public int Id { get; set; } // Yorumun benzersiz kimliği
        public string Content { get; set; } // Yorumun içeriği
        public string UserId { get; set; } // Yorumu yapan kullanıcının kimliği
        public DateTime CreatedAt { get; set; } // Yorumun oluşturulma tarihi
        //public string UserName { get; set; } // Kullanıcı adı, sonradan eklendi
    }
}

/*
    Genel Özet:
CommentVm sınıfı, yorum bilgilerini görüntülemek için kullanılan bir ViewModel'dir. Sınıfın içerdiği alanlar ve açıklamaları aşağıda yer almaktadır:

Id: Yorumun sistemdeki benzersiz kimliğini belirtir.
Content: Yorumun metin içeriğini tutar.
UserId: Yorumu yapan kullanıcının benzersiz kimliğini belirtir. Bu, kullanıcının yorumun sahibi olduğunu belirlemek için gereklidir.
CreatedAt: Yorumun oluşturulma tarihini belirtir. Bu, yorumların zaman damgasını tutarak ne zaman yapıldığını gösterir.
 */