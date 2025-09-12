using Blog.Core.DTos;
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
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
            //return await _context.Categories.FirstAsync(c => c.CatId == id);
            //return await _context.Categories.LastAsync(c => c.CatId == id);
            //return await _context.Categories.FirstOrDefaultAsync(c => c.CatId == id);
            //return await _context.Categories.LastOrDefaultAsync(c => c.CatId == id);
            //return await _context.Categories.SingleAsync(c => c.CatId == id);
            //return await _context.Categories.SingleOrDefaultAsync(c => c.CatId == id);

        }
        public async Task<Category> CreateAsync(CategoryDTo category)
        {
            var Category = new Category
            {
                Name = category.Name,
            };
            await _context.Categories.AddAsync(Category);
            await _context.SaveChangesAsync();
            return Category;
        }
        public async Task<bool> UpdateAsync(int id, CategoryDTo category)
        {
            var OldCategory = await GetByIdAsync(id);
            if (OldCategory is null)
                return false;
            OldCategory.Name = category.Name;
            _context.Categories.Update(OldCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(CategoryDTo category)
        {
            var OldCategory = await GetByIdAsync(category.CatId);
            if (OldCategory is null)
                return false;
            OldCategory.Name = category.Name;
            _context.Categories.Update(OldCategory);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var Category = await GetByIdAsync(id);
            if (Category is null)
                return false;
            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();
            return true;
        }


    }
}
