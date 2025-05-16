namespace Booking.Core.DTOs.Event
{
    public class EventDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; } = string.Empty;
        public decimal Price { get; set; }

    }
}
