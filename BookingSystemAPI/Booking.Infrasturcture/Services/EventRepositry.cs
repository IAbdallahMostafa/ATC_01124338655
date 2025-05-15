using Booking.Core.Enities;
using Booking.Core.Intefaces;
using Booking.Infrasturcture.Data;

namespace Booking.Infrasturcture.Services
{
    public class EventRepositry : GenericRepositry<Event>, IEventRepositry
    {
        public EventRepositry(BookingDbContext context) : base(context)
        {
        }
    }
}
