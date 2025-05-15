using Booking.Core.Intefaces;
using Booking.Infrasturcture.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Booking.Infrasturcture.Services
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : class
    {
        private readonly BookingDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepositry(BookingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string[]? includeWords = null)
        {
            return await GetQuery(filter, includeWords)
                .ToListAsync();
        }
        public async Task<T> GetOneAsync(Expression<Func<T, bool>>? filter = null, string[]? includeWords = null)
        {
            return await GetQuery(filter, includeWords).FirstOrDefaultAsync();
        }

        private IQueryable<T> GetQuery(Expression<Func<T, bool>>? filter = null, string[]? includeWords = null)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
                query = query.Where(filter);
            if (includeWords != null)
                foreach (var word in includeWords)
                    query = query.Include(word);
            return query;
        }

        public async Task AddAsync(T item)
        {
            await _dbSet.AddAsync(item);
        }

        public void Update(T updatedItem)
        {
            _dbSet.Update(updatedItem);
        }

        public void Delete(T item)
        {
            _dbSet.Remove(item);    
        }
    }
}
