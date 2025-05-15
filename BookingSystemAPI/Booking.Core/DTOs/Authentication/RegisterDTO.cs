using System.ComponentModel.DataAnnotations;

namespace Booking.Core.DTOs.Authentication
{
    public class RegisterDTO
    {
        [MaxLength(25)]
        public string FirstName { get; set; } = string.Empty;
        [MaxLength(25)]
        public string LastName { get; set; } = string.Empty;
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(25)]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [MaxLength(25)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
