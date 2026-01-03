using System;
using System.Collections.Generic;
using System.Text;

namespace HRMS.Models
{
    namespace HotelManagementSystem.Models
    {
        public class HotelManager : User
        {
            public string Department { get; set; } = "Management";

            // Full system access (conceptual placeholder)
            public void AccessSystem()
            {
                Console.WriteLine($"Manager {Username} has full system access.");
            }

            // User management
            public void ManageUsers()
            {
                Console.WriteLine($"Manager {Username} is managing user accounts.");
                // Here you would call IUserService methods like AddUser, UpdateUser, DeleteUser
            }

            // Configure room rates and policies
            public void ConfigureRoomRates(decimal newRate)
            {
                Console.WriteLine($"Manager {Username} set new room rate: {newRate}");
                // Logic to update room rate in DB
            }

            public void ConfigurePolicies(string policy)
            {
                Console.WriteLine($"Manager {Username} updated policy: {policy}");
                // Logic to update hotel policies in DB
            }

            // View all reports
            public void ViewReport(string reportType)
            {
                Console.WriteLine($"Manager {Username} is viewing {reportType} report.");
                // Logic to generate and display reports
            }

            // System configuration
            public void ConfigureSystem(string setting, string value)
            {
                Console.WriteLine($"Manager {Username} updated system setting '{setting}' to '{value}'.");
                // Logic to update system configuration
            }
        }
    }

}
