using Blog.Core.DTos;
using Blog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interfaces
{
    public interface ICategoryService
    {
        // get all  
        Task<IEnumerable<Category>> GetAllAsync();
        // get by id
        Task<Category> GetByIdAsync(int id);
        // add
        Task<Category> CreateAsync(CategoryDTo category); // change category model
        // update
        Task<bool> UpdateAsync(int id,CategoryDTo category);
        Task<bool> UpdateAsync(CategoryDTo category);
        // delete
        Task<bool> DeleteAsync(int id);
    }
}
