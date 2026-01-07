using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // store securely
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleID { get; set; }  
        public string RoleName { get; set; }

        public string UserStatus { get; set; }   // Active, Inactive, Suspended
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Common behavior
     

    }
}
