using Booking.Core.Enities.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace Booking.Core.Enities
{
    public class Book : BaseEntity
    {

        [ForeignKey(nameof(ApplicationUser))]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(Event))]
        public int EventId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public ApplicationUser ApplicationUser { get; set; }
        public Event Event { get; set; }
    }
}
