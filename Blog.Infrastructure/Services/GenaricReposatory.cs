using Blog.Core.Interfaces;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class GenaricReposatory<T> : IGenaricReposatory<T> where T : class
    {
        // DI
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenaricReposatory(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> predicate = null,
            IEnumerable<Expression<Func<T, object>>> includes = null)
        {
            IQueryable<T> query = _dbSet; // my Table
            // Select * from T
            if(predicate is not null)
                query = query.Where(predicate);
            // Select * from T Where (predicate)
            if (includes is not null)
                foreach (var include in includes)
                    query = query.Include(include);
            return await query.ToListAsync();
        }
        //public async Task SaveAsync()
        //{
        //    await _context.SaveChangesAsync();
        //}


    }
}
