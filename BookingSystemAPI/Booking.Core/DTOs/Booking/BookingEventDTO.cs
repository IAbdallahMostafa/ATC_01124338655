using System.ComponentModel.DataAnnotations;

namespace Booking.Core.DTOs.Booking
{
    public class BookingEventDTO
    {
        [Required]
        public string ApplicationUserId { get; set; }
        public int EventId { get; set; }
    }
}
