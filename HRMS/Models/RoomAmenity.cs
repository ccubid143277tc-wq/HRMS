using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Models
{
    public class RoomAmenity
    {
        public int RoomAmenityID { get; set; }
        public int RoomID { get; set; }
        public int AmenitiesID { get; set; }
    }
}
