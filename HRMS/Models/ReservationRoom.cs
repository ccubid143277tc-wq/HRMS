using System;

namespace HRMS.Models
{
    public class ReservationRoom
    {
        public int ReservationRoomID { get; set; }
        public int ReservationID { get; set; }
        public int RoomID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
