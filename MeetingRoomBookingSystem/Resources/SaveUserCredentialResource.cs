using System.ComponentModel.DataAnnotations;

namespace MeetingRoomBookingSystem.Resources
{
    public class SaveUserCredentialResource
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; } 

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string role { get; set; } = string.Empty;
        [Required]
        public string phoneNumber { get; set; } = string.Empty;
        [Required]
        public int companyId { get; set; }
        [Required]
        public string gender { get; set; }

    }
}
