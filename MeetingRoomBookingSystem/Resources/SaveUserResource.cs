using MRBS.Core.Models;

namespace MeetingRoomBookingSystem.Resources
{
    public class SaveUserResource
    {
        public string Name { get; set; } 

       // public DateTime DateOfBirth { get; set; }

        public string Email { get; set; } 

        public string Role { get; set; }

        public string? PhoneNumber { get; set; }

       // public int CompanyId { get; set; }

    }
}
