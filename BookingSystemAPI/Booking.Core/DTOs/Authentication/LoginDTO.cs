using System.ComponentModel.DataAnnotations;

namespace Booking.Core.DTOs.Authentication
{
    public class LoginDTO
    {
        [MaxLength(25)]
        public string UserName { get; set; } = string.Empty;
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
