using Booking.Core.Intefaces;
using Booking.Infrasturcture.Data;

namespace Booking.Infrasturcture.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookingDbContext _context;
        public ICategoryRepositry Categories { get; private set; }
        public IEventRepositry Events { get; private set; }
        public IBookingRepositry Books { get; private set; }
        public IAuthenticationRepositry Authentication { get; private set; }

        public UnitOfWork(BookingDbContext context)
        {
            _context = context;
            Categories = new CategoryRepositry(context);
            Events = new EventRepositry(context);
            Books = new BookingRepositry(context);
        }
        public async Task<int> CompleteAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
