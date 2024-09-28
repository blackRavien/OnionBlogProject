using OnionProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.VMs
{
    public class AuthorVm
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Status Status { get; set; }

        public string ImagePath { get; set; }// Eğer ImagePath bu alana karşılık geliyorsa, UploadPath ile ilişkili olmalı

        public string UploadPath { get; set; } // Eğer UploadPath gerekiyorsa buraya eklenmeli
    }
}
