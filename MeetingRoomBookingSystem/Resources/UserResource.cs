using MRBS.Core.Models;

namespace MeetingRoomBookingSystem.Resources
{
    public class UserResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Email { get; set; }

     //   public string Password { get; set; }

        public string Role { get; set; }

        public string? PhoneNumber { get; set; }

        public int CompanyId { get; set; }
 //       public CompanyResource Company { get; set; }

//        public  ICollection<Reservation> Reservations { get; set; }
    }
}
