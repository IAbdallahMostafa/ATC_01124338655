using Booking.Core.Enities;

namespace Booking.Core.Intefaces
{
    public interface IEventRepositry : IGenericRepositry<Event> 
    {
        public Task<bool> IsValid(int id);
    }
}
