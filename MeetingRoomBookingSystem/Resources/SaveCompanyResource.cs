using MRBS.Core.Models;

namespace MeetingRoomBookingSystem.Resources
{
    public class SaveCompanyResource
    {
        public string Name { get; set; }

        public string Description { get; set; } 

        public string EmailAddress { get; set; }

        //public byte[]? Logo { get; set; }

        public bool Active { get; set; } 
    }
}
