using Booking.Core.DTOs.Authentication;
using Booking.Core.Enities.Authentication;
    
namespace Booking.Core.Intefaces
{
    public interface IAuthenticationRepositry 
    {
        Task<AuthenticationModel> Register(RegisterDTO registerDTO);
        Task<AuthenticationModel> Login(LoginDTO loginDTO);
        Task<AuthenticationModel> GetNewRefreshToken(string currentRefreshToken);
        Task<string> RevokeRefreshToken(string refreshToken);
    }
}
