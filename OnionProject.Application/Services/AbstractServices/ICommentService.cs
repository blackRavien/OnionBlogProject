using OnionProject.Application.Models.DTOs; // DTO sınıflarını kullanmak için gerekli olan isim alanı
using System; // Temel sistem işlevleri için gerekli olan isim alanı
using System.Collections.Generic; // Koleksiyonlar için gerekli olan isim alanı
using System.Linq; // LINQ sorguları için gerekli olan isim alanı
using System.Text; // String işleme için gerekli olan isim alanı
using System.Threading.Tasks; // Asenkron programlama için gerekli olan isim alanı

namespace OnionProject.Application.Services.AbstractServices
{
    // Yorum hizmeti için bir arayüz
    public interface ICommentService
    {
        // Yorum ekleme işlemi için metot
        Task AddCommentAsync(CreateCommentDTO commentDto);

        // Yorum silme işlemi için metot
        Task DeleteCommentAsync(int commentId);

        // Belirli bir gönderi (post) için yorumları almak için metot
        Task<List<GetCommentDTO>> GetCommentsByPostIdAsync(int postId);
    }
}
/*
    Açıklamalar
Namespace Kullanımı:
OnionProject.Application.Models.DTOs: DTO (Data Transfer Object) sınıflarını içeren isim alanıdır. Bu alan, uygulama içindeki veri taşımayı kolaylaştırmak için kullanılır.
İşlevsellik:
ICommentService Arayüzü: Yorum hizmeti ile ilgili temel işlevleri tanımlar. Bu, bağımlılık enjeksiyonu ve test edilebilirliği artırmak için kullanılan bir tasarım desenidir.

AddCommentAsync Metodu:

CreateCommentDTO commentDto: Yeni bir yorum eklemek için gerekli olan veriyi içeren DTO nesnesini alır.
Task: Bu metodun asenkron bir görev olduğunu belirtir, yani işlemin tamamlanmasını beklemeden diğer işlemlere devam edilebilir.
DeleteCommentAsync Metodu:

int commentId: Silinmesi istenen yorumun kimliğini (ID'sini) alır.
Bu metod, belirli bir yorumu silmek için kullanılır.
GetCommentsByPostIdAsync Metodu:

int postId: Yorumlarını almak istediğimiz gönderinin (post'un) kimliğini alır.
Task<List<GetCommentDTO>>: Bu metodun, belirli bir gönderiye ait yorumların listesini döndürdüğünü belirtir. Bu, yorumların kullanıcı arayüzünde görüntülenmesi için gerekli veriyi sağlar.
 */