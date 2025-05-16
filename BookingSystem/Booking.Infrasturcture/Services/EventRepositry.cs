using Booking.Core.Enities;
using Booking.Core.Intefaces;
using Booking.Infrasturcture.Data;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrasturcture.Services
{
    public class EventRepositry : GenericRepositry<Event>, IEventRepositry
    {
        private BookingDbContext _context;
        private DbSet<Event> _dbSet;
        
        public EventRepositry(BookingDbContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<Event>();
        }

        public async Task<bool> IsValid(int id)
        {
            return await _dbSet.AnyAsync(c => c.Id == id);
        }
    }
}
