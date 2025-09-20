using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        // DB DI
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CategoryService = new CategoryService(context);
            Categories = new GenaricReposatory<Category>(context);
            Posts = new GenaricReposatory<Post>(context);
        }

        public ICategoryService CategoryService { get;private set; }
        public IGenaricReposatory<Category> Categories { get;private set; }
        public IGenaricReposatory<Post> Posts { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
