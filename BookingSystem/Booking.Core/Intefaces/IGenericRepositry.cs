using System.Linq.Expressions;

namespace Booking.Core.Intefaces
{
    public interface IGenericRepositry<T> where T : class
    {
        Task<T> GetOneAsync(Expression<Func<T, bool>>? filter = null, string[]? includeWords = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string[]? includeWords = null);
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
    }
}
