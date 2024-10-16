﻿using OnionProject.Application.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionProject.Application.Models.VMs
{
    public class PostDetailsWithCommentVm
    {
        public PostDetailsVm? PostDetails { get; set; }
        public CreateCommentDTO NewComment { get; set; }
        public List<GetCommentDTO>? Comments { get; set; }  // Yorumları listeleme için ekledik.
        public AuthorDetailVm? AuthorDetail { get; set; }
        
    }
}
