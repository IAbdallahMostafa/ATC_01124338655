namespace Booking.Core.Enities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;   
        public List<Event> Events { get; set; } = new List<Event>();
    }
}
