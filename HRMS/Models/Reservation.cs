using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public int GuestID { get; set; }
        public DateTime Check_InDate { get; set; }
        public DateTime Check_OutDate { get; set; }
        public int NumAdult { get; set; }
        public int NumChild { get; set; }
        public string SpecialRequest { get; set; }
        public string ReservationStatus { get; set; }
        public int RoomID { get; set; }
        public int RoomTypeID { get; set; }
        
        // Navigation properties for display
        public string GuestName { get; set; }
        public string RoomNumber { get; set; }
        public string RoomTypeName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Common behavior
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Reservation {ReservationID}: {GuestName} - Room {RoomNumber} ({Check_InDate:yyyy-MM-dd} to {Check_OutDate:yyyy-MM-dd}) - {ReservationStatus}");
        }

        // Calculated properties
        public int TotalDays => (Check_OutDate - Check_InDate).Days;
        public int TotalGuests => NumAdult + NumChild;
        public decimal BalanceAmount => TotalAmount - PaidAmount;
    }
}
