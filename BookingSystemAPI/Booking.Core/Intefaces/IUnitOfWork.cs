namespace Booking.Core.Intefaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepositry Categories { get; }
        IEventRepositry Events { get; }
        IAuthenticationRepositry Authentication { get; }
        Task<int> CompleteAsync();

    }
}
