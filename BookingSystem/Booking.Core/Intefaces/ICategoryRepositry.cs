using Booking.Core.Enities;

namespace Booking.Core.Intefaces
{
    public interface ICategoryRepositry : IGenericRepositry<Category> 
    {
        Task<bool> IsValid(int  id);
    }
}
