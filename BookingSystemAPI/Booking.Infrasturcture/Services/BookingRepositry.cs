using Booking.Core.Enities;
using Booking.Core.Intefaces;
using Booking.Infrasturcture.Data;


namespace Booking.Infrasturcture.Services
{
    public class BookingRepositry : GenericRepositry<Book>, IBookingRepositry
    {
        public BookingRepositry(BookingDbContext context) : base(context)
        {
        }
    }
}
