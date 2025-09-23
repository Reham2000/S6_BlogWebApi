using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blog.Core.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; } // PK
        [Required,StringLength(50,MinimumLength =3)]
        public string Title { get; set; }
        [Required,StringLength(300,MinimumLength =10)]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Relations
        // 1. User  1 user -> M Posts
        public string UserId { get; set; }
       
        public virtual User User { get; set; }
        // 2. Category  [ 1 category  -> M Posts]
        public int CategoryId { get; set; }
        
        public virtual Category Category { get; set; }
        

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
