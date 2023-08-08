using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Exceptions;
using TodoApp.Core.Interfaces.Repositories;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DataContext _dbcontext;

        public BaseRepository(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbcontext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);
        }

        public async Task<T> Create(T entity)
        {
            try
            {
                await _dbcontext.Set<T>().AddAsync(entity);
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return entity;
        }

        public async Task<T> Update(T entity)
        {
            try
            {
                _dbcontext.Entry(entity).State = EntityState.Modified;
                await _dbcontext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }

            return entity;
        }

        public async Task Delete(T entity)
        {
            _dbcontext.Set<T>().Remove(entity);
            await _dbcontext.SaveChangesAsync();
        }

    }
}