using Blog.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.DTos
{
    public class PostDTo
    {
       
        public int Id { get; set; } // PK
        [Required, StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        [Required, StringLength(300, MinimumLength = 10)]
        public string Content { get; set; }
        // Relations
        // 1. User  1 user -> M Posts
        public int UserId { get; set; }
       
        // 2. Category  [ 1 category  -> M Posts]
        public int CategoryId { get; set; }
        

    }
}
