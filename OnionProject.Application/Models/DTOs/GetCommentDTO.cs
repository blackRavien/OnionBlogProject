using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.DTOs
{
    public class GetCommentDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }

}
