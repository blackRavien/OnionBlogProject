using System;

namespace OnionProject.Application.Models.VMs
{
    // Tür bilgilerini içeren ViewModel sınıfı
    public class GenreVm
    {
        public int Id { get; set; } // Türün benzersiz kimliği
        public string Name { get; set; } // Türün adı
    }
}

/*
    Genel Özet:
GenreVm sınıfı, bir türün (genre) bilgilerini görüntülemek için kullanılan bir ViewModel'dir. Sınıfın içerdiği alanlar ve açıklamaları aşağıda yer almaktadır:

Id: Türün sistemdeki benzersiz kimliğini belirtir. Her tür için eşsiz bir değer olmalıdır.
Name: Türün adını tutar. Bu, türün ne olduğunu tanımlamak için kullanılır (örneğin, "Fantezi", "Bilim Kurgu", "Roman" gibi).
 */