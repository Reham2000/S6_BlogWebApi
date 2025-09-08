using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        // DI
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            //var Categories = await _context.Categories.ToListAsync();
            var Categories =  _context.Categories.Select(cat => new Category
            {
                Name = cat.Name
            });
            return Categories;
        }
        public async Task<IEnumerable<Object>> GetAllAsync2()
        {
            var Categories = _context.Categories.Select(cat => new 
            {
                Name = cat.Name,
                SayHi = "Hello"
            });
            return Categories;
        }
        public Task<Category> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<Category> CreateAsync(Category category)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateAsync(int id, Category category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Category category)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
}
