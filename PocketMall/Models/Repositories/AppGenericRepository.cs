using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PocketMall.Models.IRepositories;
using System.ComponentModel.DataAnnotations;

namespace PocketMall.Models.Repositories
{
    public class AppGenericRepository<T> : IAppGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public AppGenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<int?> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            if (result >= 1)
            {
                return result;
            }
            return null;
        }

        public async Task<int?> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            if (result >= 1)
            {
                return result;
            }
            return null;
        }

        public async Task<int?> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            var result = await _context.SaveChangesAsync();
            if (result >= 1)
            {
                return result;
            }
            return null;
        }
        public async Task<T> GetIfExistsAsync(T entity)
        {
            var existingEntity = await _context.Set<T>().Where(e => e.Equals(entity)).FirstOrDefaultAsync();
            return existingEntity;
        }

    }
}
