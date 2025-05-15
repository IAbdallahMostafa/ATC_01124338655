using Microsoft.AspNetCore.Http;

namespace Booking.Core.DTOs.Event
{
    public class UpdateEventDTO : EventDTO
    {
        public IFormFile? Image { get; set; }
    }
}
