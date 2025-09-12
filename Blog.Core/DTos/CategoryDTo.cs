using Blog.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.DTos
{
    public class CategoryDTo
    {
        
        public int CatId { get; set; }
        [Required(ErrorMessage = "Category Name Is Required")]
        [StringLength(100, MinimumLength = 3)] // 3: 100 Char
        //[MaxLength(100),MinLength(3)]
        public string Name { get; set; }


        
    }
}
