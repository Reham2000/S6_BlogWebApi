using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interfaces
{
    public interface IGenaricReposatory<T> where T : class
    {
        // get all
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(
                Expression<Func<T,bool>> predicate = null,
                IEnumerable<Expression<Func<T, object>>> includes = null
            );
        // get By Id
        Task<T?> GetByIdAsync(int id);
        // Find / Search
        Task<IEnumerable<T>> FindAsync(Expression<Func<T,bool>> predicate);
        // create
        Task CreateAsync(T entity);
        // update
        void Update(T entity);
        // Delete
        void Delete(T entity);
        //Task SaveAsync();
    }
}
