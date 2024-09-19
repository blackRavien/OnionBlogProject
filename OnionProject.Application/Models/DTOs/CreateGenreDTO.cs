using OnionProject.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnionProject.Application.Models.DTOs
{
    public class CreateGenreDTO
    {
        [Required(ErrorMessage = "Tür ismi gereklidir.")]
        [MinLength(2, ErrorMessage = "Tür ismi en az 2 karakterden oluşmalıdır.")]
        [Display(Name = "Tür İsmi")]
        public string Name { get; set; } // Tür ismini alacak olan property

        public DateTime CreatedDate => DateTime.Now; // Oluşturulma tarihi otomatik olarak güncel tarih olacak.

        public Status Status => Status.Active; // Status, yeni oluşturulan türün varsayılan olarak aktif olmasını sağlar.
    }
}
