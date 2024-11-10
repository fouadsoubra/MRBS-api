namespace MeetingRoomBookingSystem.Resources
{
    public class ReservationResource
    {
        public int Id { get; set; }

        public DateTime DateOfMeeting { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int RoomId { get; set; }

        public int NumberAttendees { get; set; }

        public bool MeetingStatus { get; set; }

        public int UserId { get; set; }

        public bool ScreenNedded { get; set; }
    }
}
