using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Models
{
    public class Guest
    {
        public int GuestID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public string Classification { get; set; }
     
       
       

        // Full name property for display
        public string FullName => $"{FirstName} {LastName}";
    }
}
