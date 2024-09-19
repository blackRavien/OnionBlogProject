using OnionProject.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.DTOs
{
    public class UpdateGenreDTO
    {
        // Türün mevcut id'sini almak için gerekli. Bu yüzden zorunlu olarak işaretledik.
        [Required]
        public int Id { get; set; }

        // Türün adını güncellemek için gerekli olan alan.
        [Required(ErrorMessage = "Tür ismi zorunludur.")]
        [MinLength(3, ErrorMessage = "Tür ismi en az 3 karakterden oluşmalıdır.")]
        [Display(Name = "Tür")]
        public string Name { get; set; }

        // Türün durumunu (aktif, pasif vs.) güncellemek için gerekli alan.
        public Status Status { get; set; }

        // Türün güncelleme tarihini tutan alan. Güncelleme işleminde tarih otomatik olarak atanır.
        public DateTime UpdatedDate => DateTime.Now;
    }
}
