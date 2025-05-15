namespace Booking.Core.Intefaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepositry Categories { get; }
        IEventRepositry Events { get; }
        IBookingRepositry Books { get; }
        IAuthenticationRepositry Authentication { get; }
        Task<int> CompleteAsync();

    }
}
