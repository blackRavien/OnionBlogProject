using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.VMs
{
    public class PostVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public string GenreName { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string AuthorFullName => $"{AuthorFirstName} {AuthorLastName}";
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int GenreId { get; set; }
        public int AuthorId { get; set; }

    }
}

//post 3 yazar 2 1 tür get posts yani