using MRBS.Core.Models;

namespace MeetingRoomBookingSystem.Resources
{
    public class CompanyResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public string EmailAddress { get; set; }
        public bool Active { get; set; }

    }
}
