using OnionProject.Application.Models.DTOs;
using System;
using System.Collections.Generic;

namespace OnionProject.Application.Models.VMs
{
    // Gönderi detayları ve yorumları birlikte tutan ViewModel
    public class PostDetailsWithCommentVm
    {
        public PostDetailsVm? PostDetails { get; set; } // Gönderi detayları
        public CreateCommentDTO NewComment { get; set; } // Yeni yorum oluşturma için DTO
        public List<GetCommentDTO>? Comments { get; set; } // Yorumları listeleme için
        public AuthorDetailVm? AuthorDetail { get; set; } // Yazar detayları
    }
}

/*
    Genel Özet:
PostDetailsWithCommentVm sınıfı, bir gönderinin detayları ve onunla ilişkili yorumları aynı anda tutmak için kullanılan bir ViewModel'dir. Bu yapı, gönderi detaylarını görüntülemek ve yeni yorumlar eklemek için gereken bilgileri içerir.

Alanlar:
PostDetails: PostDetailsVm türünde bir nesne olup, gönderinin detaylarını tutar. Nullable olarak tanımlanmıştır, böylece gönderi detayları mevcut olmayabilir.

NewComment: Yeni bir yorum oluşturmak için kullanılan CreateCommentDTO türünde bir nesne. Bu alan, kullanıcının yeni bir yorum eklemesi için gereklidir.

Comments: GetCommentDTO türünde bir liste olup, gönderiye yapılmış yorumları tutar. Nullable olarak tanımlanmıştır, yani yorumlar mevcut olmayabilir.

AuthorDetail: Yazarın detaylarını tutan AuthorDetailVm türünde bir nesne. Nullable olarak tanımlanmıştır, bu yüzden yazar detayları sağlanmayabilir.

Kullanım:
Bu ViewModel, bir gönderi detay sayfasında gönderinin bilgilerini, yeni yorum ekleme formunu ve mevcut yorumları göstermek için kullanılır. Kullanıcı, sayfa üzerinde yeni bir yorum ekleyebilir ve var olan yorumları görebilir.
 */