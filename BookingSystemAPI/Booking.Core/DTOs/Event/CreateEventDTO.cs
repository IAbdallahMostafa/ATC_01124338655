using Microsoft.AspNetCore.Http;

namespace Booking.Core.DTOs.Event
{
    public class CreateEventDTO : EventDTO
    {
        public IFormFile Image { get; set; }
    }
}
