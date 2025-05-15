namespace Booking.Core.Enities.Authentication
{
    public class AuthenticationModel
    {
        public string Message { get; set; } 
        public string UserName { get; set; }
        public string Email { get; set;} 
        public List<string> Roles { get; set; } = new List<string>();
        public bool isAuthenticated { get; set; }
        public string AccessToken { get; set; } 
        public DateTime AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; } 
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
