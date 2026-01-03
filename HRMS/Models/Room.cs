using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; }
        public int RoomType { get; set; }
        public string BedConfiguration { get; set; }
        public int MaximumOccupancy { get; set; }
        public int RoomFloor { get; set; }
        public int RoomStatusID { get; set; }
        public string ViewType { get; set; }
        public decimal RoomRate { get; set; }

        public List<string> Amenities { get; set; } = new List<string>();
        public string RoomTypeName { get; set; }
        public string RoomStatusName { get; set; }
        public string AmenitiesString { get; set; }


    }
}
