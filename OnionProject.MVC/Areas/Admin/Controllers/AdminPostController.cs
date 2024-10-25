using AutoMapper; // AutoMapper kütüphanesini ekliyoruz.
using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için gerekli.
using Microsoft.AspNetCore.Mvc; // MVC yapı taşlarını ekliyoruz.
using Newtonsoft.Json; // JSON işlemleri için kütüphane.
using OnionProject.Application.Models.DTOs; // DTO sınıflarının bulunduğu namespace.
using OnionProject.Application.Models.VMs; // ViewModel sınıflarının bulunduğu namespace.
using OnionProject.Application.Services.AbstractServices; // Uygulama servislerinin abstract sınıfları.
using OnionProject.Domain.AbstractRepositories; // Repository katmanının abstract sınıfları.
using OnionProject.Domain.Entities; // Uygulama entity sınıfları.
using System.Net.Http; // HTTP istemcisi için gerekli kütüphane.
using System.Security.Claims; // Kullanıcı taleplerini yönetmek için gerekli.
using System.Text; // Dize işlemleri için gerekli.
using System.Text.Json; // JSON işlemleri için alternatif kütüphane.


namespace OnionProject.MVC.Areas.Admin.Controllers
{
    
    // Admin alanı altında bulunan post işlemleri için controller.
    [Area("Admin")] // Bu controller'ın Admin alanında olduğunu belirtir.
    [Authorize(Roles = "Admin")] // Sadece Admin rolüne sahip kullanıcıların erişimine izin verir.
    public class AdminPostController : Controller
    {
        private readonly ICommentService _commentService; // Yorum işlemleri için servis.
        private readonly IPostService _postService; // Post işlemleri için servis.
        private readonly IMapper _mapper; // Mapper nesnesi.
        private readonly ICommentRepo _commentRepo; // Yorum repository'si.
        private readonly HttpClient _httpClient; // HTTP istemcisi.
        private readonly string uri = "https://localhost:7296"; // API URL'si.

        // Constructor'da dependency injection ile gerekli bağımlılıkları alıyoruz.
        public AdminPostController(ICommentRepo commentRepo, IMapper mapper, ICommentService commentService, IPostService postService, HttpClient httpClient)
        {
            _commentRepo = commentRepo; // Yorum repository'sini al.
            _mapper = mapper; // Mapper'ı al.
            _commentService = commentService; // Yorum servisini al.
            _postService = postService; // Post servisini al.
            _httpClient = httpClient; // HTTP istemcisini al.
        }

        // Tüm postları listeleme işlemi.
        public async Task<IActionResult> Index()
        {
            List<GetPostsVm> posts = new List<GetPostsVm>(); // Postları tutmak için liste.
            using (var httpClient = new HttpClient()) // Yeni bir HTTP istemcisi oluştur.
            {
                using (var response = await httpClient.GetAsync($"{uri}/api/AdminPostApi/Index")) // API'den postları al.
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Yanıtı string olarak oku.
                    posts = JsonConvert.DeserializeObject<List<GetPostsVm>>(apiResponse); // JSON'u deserialize et.
                }
            }
            return View(posts); // Postları view'e gönder.
        }

        // Yeni post oluşturma sayfasını getir.
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            List<GenreVm> genres = new List<GenreVm>(); // Türleri tutacak liste
            using (var httpClient = new HttpClient()) // Yeni bir HTTP istemcisi oluştur.
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminGenreApi");
                if (response.IsSuccessStatusCode) // İstek başarılıysa.
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Yanıtı oku.
                    genres = JsonConvert.DeserializeObject<List<GenreVm>>(apiResponse); // JSON'u deserialize et.
                }
            }
            ViewBag.Genres = genres;  // Türleri ViewBag'e ekle.


            List<AuthorVm> authors = new List<AuthorVm>(); // Yazarları tutacak liste.
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AuthorApi/Index");
                if (response.IsSuccessStatusCode) // İstek başarılıysa.
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    authors = JsonConvert.DeserializeObject<List<AuthorVm>>(apiResponse);
                }
            }
            ViewBag.Authors = authors; // Yazarları ViewBag'e ekle.


            return View();
        }

        // Yeni bir post oluşturma işlemi.
        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDTO post)
        {
            if (!ModelState.IsValid) // Model geçerli değilse.
            {
                TempData["Error"] = "Girdiğiniz verileri kontrol ediniz!";
                return View(post); // Geçersiz verileri döndür.
            }

            using (var httpClient = new HttpClient())
            {
                var form = new MultipartFormDataContent(); // Çok parçalı form verisi oluştur.
                form.Add(new StringContent(post.Title), "Title"); // Başlığı ekle.
                form.Add(new StringContent(post.Content), "Content"); // İçeriği ekle.
                form.Add(new StringContent(post.AuthorId.ToString()), "AuthorId"); // Yazar ID'sini ekle.
                form.Add(new StringContent(post.GenreId.ToString()), "GenreId"); // Tür ID'sini ekle.

                if (post.UploadPath != null) // Eğer dosya yüklenmişse.
                {
                    var fileStreamContent = new StreamContent(post.UploadPath.OpenReadStream()); // Dosya akışını oluştur.
                    form.Add(fileStreamContent, "UploadPath", post.UploadPath.FileName); // Dosyayı ekle.
                }

                var response = await httpClient.PostAsync($"{uri}/api/AdminPostApi/Create", form); // API'ye POST isteği gönder.
                if (response.IsSuccessStatusCode) // İstek başarılıysa.
                {
                    TempData["Success"] = "Post başarıyla oluşturuldu!"; // Başarı mesajı göster.
                    return Redirect("https://localhost:7225/Admin/AdminPost/Index"); // İndex sayfasına yönlendir.
                }
                TempData["Error"] = "Post oluşturulurken hata meydana geldi!"; // Hata mesajı göster.
            }
            return Redirect("https://localhost:7225/Admin/AdminPost/Index"); // İndex sayfasına yönlendir.
        }


        // Belirli bir postun detaylarını göster.
        public async Task<IActionResult> Details(int id)
        {
            PostDetailsVm postDetailsVm = new PostDetailsVm(); // Post detayları için model.
            using (var httpClient = new HttpClient()) // Yeni bir HTTP istemcisi oluştur.
            {
                // API isteği oluştur ve yanıtı al
                using (var response = await httpClient.GetAsync($"https://localhost:7296/api/AdminPostApi/Details/{id}")) // Belirli ID'ye göre post detaylarını al.
                {
                    if (response.IsSuccessStatusCode) // İstek başarılı olduysa.
                    {
                        // API'den gelen JSON'u string olarak oku
                        string apiResponse = await response.Content.ReadAsStringAsync(); // Yanıtı oku.

                        // JSON'u PostDetailsVm nesnesine deserialize et
                        postDetailsVm = JsonConvert.DeserializeObject<PostDetailsVm>(apiResponse); // JSON'u deserialize et.
                    }
                    else
                    {
                        return NotFound(); // Eğer istek başarısızsa NotFound döndür.
                    }
                }
            }

            // Yorumları al
            var comments = await _commentService.GetCommentsByPostIdAsync(id); // Post ID'sine göre yorumları al.

            // View'e göndermek için bir model oluştur
            var model = new PostDetailsWithCommentVm
            {
                PostDetails = postDetailsVm, // Post detayları (API'den gelen tam resim URL'si ile)
                Comments = comments, // Yorumlar
                NewComment = new CreateCommentDTO() // Yeni yorum için boş bir DTO
            };

            return View(model); // Modeli View'e gönder.
        }


        // Post güncelleme sayfasını getir.
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {

            //Genre'leri alma
            List<GenreVm> genres = new List<GenreVm>(); // Türleri tutacak liste.
            using (var httpClient = new HttpClient()) // Yeni bir HTTP istemcisi oluştur.
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminGenreApi"); // Türleri almak için API çağrısı yap.
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Yanıtı oku.
                    genres = JsonConvert.DeserializeObject<List<GenreVm>>(apiResponse); // JSON'u deserialize et.
                }
            }
            ViewBag.Genres = genres; // Türleri ViewBag'e ekle.

            //Author'ları alma
            List<AuthorVm> authors = new List<AuthorVm>(); // Yazarları tutacak liste.
            using (var httpClient = new HttpClient()) // Yeni bir HTTP istemcisi oluştur.
            {
                var response = await httpClient.GetAsync($"{uri}/api/AuthorApi/Index"); // Yazarları almak için API çağrısı yap.
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();    // Yanıtı oku.
                    authors = JsonConvert.DeserializeObject<List<AuthorVm>>(apiResponse); // JSON'u deserialize et.
                }
            }
            ViewBag.Authors = authors; // Yazarları ViewBag'e ekle.

            //Post'u alma
            UpdatePostDTO post = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{uri}/api/AdminPostApi/GetPost/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    post = JsonConvert.DeserializeObject<UpdatePostDTO>(apiResponse);
                }
            }

            //GÜNCELLEME bura eklendi
            if (post == null)
            {
                return NotFound(); // Eğer post bulunamazsa hata döner
            }


            return View(post); //GÜNCELLEME içine post yazdık

        }

        // Post güncelleme işlemi.
        [HttpPost]
        public async Task<IActionResult> Update(UpdatePostDTO post)
        {
            if (!ModelState.IsValid) return View(post);

            // Yükleme işlemi yapma
            using (var httpClient = new HttpClient()) // Yeni bir HTTP istemcisi oluştur.
            {
                var form = new MultipartFormDataContent(); // Çok parçalı form verisi oluştur.
                form.Add(new StringContent(post.Id.ToString()), "Id"); // Post ID'sini ekle.
                form.Add(new StringContent(post.Title), "Title"); // Başlığı ekle.
                form.Add(new StringContent(post.Content), "Content"); // İçeriği ekle.
                form.Add(new StringContent(post.AuthorId.ToString()), "AuthorId"); // Yazar ID'sini ekle.
                form.Add(new StringContent(post.GenreId.ToString()), "GenreId"); // Tür ID'sini ekle.
                


                if (post.UploadPath != null)
                {
                    var fileStreamContent = new StreamContent(post.UploadPath.OpenReadStream()); // Dosya akışını oluştur.
                    form.Add(fileStreamContent, "UploadPath", post.UploadPath.FileName); // Dosyayı ekle.
                }

                // API'ye PUT isteği gönder
                var response = await httpClient.PutAsync($"{uri}/api/AdminPostApi/Update", form); // API'ye PUT isteği gönder.
                if (response.IsSuccessStatusCode) // İstek başarılıysa.
                {
                    TempData["Success"] = "Post başarıyla güncellendi!"; // Başarı mesajı göster.
                    return Redirect("https://localhost:7225/Admin/AdminPost/Index"); // İndex sayfasına yönlendir.
                }
                TempData["Error"] = "Post güncellenirken hata meydana geldi!"; // Hata mesajı göster.
            }
            return Redirect("https://localhost:7225/Admin/AdminPost/Index");  // İndex sayfasına yönlendir.
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            using (var httpClient = new HttpClient()) // Yeni bir HTTP istemcisi oluştur.
            {
                // Belirtilen ID ile API'den post silme isteği gönder.
                var response = await httpClient.DeleteAsync($"{uri}/api/AdminPostApi/Delete/{id}");
                if (response.IsSuccessStatusCode) // İstek başarılıysa.
                {
                    TempData["Success"] = "Post başarıyla silindi!"; // Başarı mesajı göster.
                }
                else // İstek başarısızsa.
                {
                    TempData["Error"] = "Post silinirken hata meydana geldi!"; // Hata mesajı göster.
                }
            }
            // Silme işlemi tamamlandıktan sonra Index sayfasına yönlendir.
            return Redirect("https://localhost:7225/Admin/AdminPost/Index");
        }

        private async Task<UpdatePostDTO> GetPostById(int id)
        {
            UpdatePostDTO post = null; // Postu tutmak için değişken tanımla.
            using (var httpClient = new HttpClient()) // Yeni bir HTTP istemcisi oluştur.
            {
                // Belirtilen ID ile API'den post detaylarını al.
                var response = await httpClient.GetAsync($"{uri}/api/AdminPostApi/Details/{id}");
                if (response.IsSuccessStatusCode) // İstek başarılıysa.
                {
                    string apiResponse = await response.Content.ReadAsStringAsync(); // Yanıtı oku.
                    post = JsonConvert.DeserializeObject<UpdatePostDTO>(apiResponse); // JSON'u deserialize et.
                }
            }
            // Postu döndür.
            return post;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(PostDetailsWithCommentVm model)
        {
            if (ModelState.IsValid) // Model geçerliyse.
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcının ID'sini al.

                // Yeni bir yorum DTO oluştur.
                var createCommentDto = new CreateCommentDTO
                {
                    Content = model.NewComment.Content, // Yorum içeriğini al.
                    PostId = model.PostDetails.Id, // Post ID'sini al.
                    UserId = userId // Kullanıcı ID'sini ayarla.
                };

                // API üzerinden yorum oluştur.
                using (var httpClient = new HttpClient())
                {
                    var json = JsonConvert.SerializeObject(createCommentDto); // DTO'yu JSON formatına çevir.
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json"); // İçeriği ayarla.
                    var response = await httpClient.PostAsync($"{uri}/api/UserPostApi/CreateComment", content); // Yorum eklemek için API'ye POST isteği gönder.

                    if (!response.IsSuccessStatusCode) // İstek başarısızsa.
                    {
                        ModelState.AddModelError("", "Yorum eklenirken bir hata oluştu."); // Hata mesajı ekle.
                        return View("Details", model); // Detay sayfasını döndür.
                    }
                }

                // Yorum başarıyla eklenirse, post detay sayfasına yönlendir.
                return Redirect($"https://localhost:7225/Admin/AdminPost/Details/{createCommentDto.PostId}");
            }

            // Model geçerli değilse, detay sayfasına geri dön.
            return Redirect($"https://localhost:7225/Admin/AdminPost/Details/{model.PostDetails.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId, int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcının ID'sini al.

            // API'den yorum silme isteği gönder.
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync($"{uri}/api/UserPostApi/DeleteComment/DeleteComment/{commentId}");
                if (response.IsSuccessStatusCode) // İstek başarılıysa.
                {
                    return Redirect($"https://localhost:7225/Admin/AdminPost/Details/{postId}"); // Başarıyla silindiyse detay sayfasına yönlendir.
                }
            }

            // Hata olsa bile detaya geri dön.
            return Redirect($"https://localhost:7225/Admin/AdminPost/Details/{postId}"); // Detay sayfasına yönlendir.
        }

        public async Task<IActionResult> GetContactMessages()
        {
            // API'den iletişim mesajlarını alma isteği gönder.
            var response = await _httpClient.GetAsync($"{uri}/api/AdminPostApi/GetContactMessages/api/contact");
            if (response.IsSuccessStatusCode) // İstek başarılıysa.
            {
                var contactMessages = await response.Content.ReadFromJsonAsync<List<ContactMessage>>(); // Mesajları oku.
                return View(contactMessages); // Mesajları view'e gönder.
            }
            else // İstek başarısızsa.
            {
                // Hata yönetimi.
                TempData["ErrorMessage"] = "İletişim mesajları alınamadı."; // Hata mesajı göster.
                return View(new List<ContactMessage>()); // Boş bir liste döndür.
            }
        }

        public async Task<IActionResult> DeleteContactMessage(int id)
        {
            // API'den iletişim mesajı silme isteği gönder.
            var response = await _httpClient.DeleteAsync($"{uri}/api/AdminPostApi/DeleteContactMessage/api/contact/{id}");
            if (response.IsSuccessStatusCode) // İstek başarılıysa.
            {
                TempData["SuccessMessage"] = "Mesaj başarıyla silindi."; // Başarı mesajı göster.
                                                                         // Silme başarılıysa aynı sayfaya yönlendir.
                return Redirect("https://localhost:7225/Admin/AdminPost/GetContactMessages");
            }
            else // İstek başarısızsa.
            {
                TempData["ErrorMessage"] = "Mesaj silinirken bir hata oluştu."; // Hata mesajı göster.
                return Redirect("https://localhost:7225/Admin/AdminPost/GetContactMessages"); // Aynı sayfaya yönlendir.
            }
        }


    }
}
