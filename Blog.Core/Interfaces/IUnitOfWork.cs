using Blog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Access Tools
        ICategoryService CategoryService { get; }
        IGenaricReposatory<Category> Categories { get; }
        IGenaricReposatory<Post> Posts { get; }
        // Save Method
        Task<int> SaveAsync();
    }
}
