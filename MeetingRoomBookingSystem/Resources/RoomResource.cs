namespace MeetingRoomBookingSystem.Resources
{
    public class RoomResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; } 

        public int Capacity { get; set; }

        public string RoomDescription { get; set; } 

        public int? CompanyId { get; set; }

    }
}
