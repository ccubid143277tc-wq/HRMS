using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Models;

namespace HRMS.Interfaces
{
    public interface IGuestService
    {
        int AddGuest(Guest guest);
        void UpdateGuest(Guest guest);
        void DeleteGuest(int guestId);
        
        IEnumerable<Guest> SearchGuest(string keyword);
        Guest GetGuestById(int guestId);
        IEnumerable<Guest> GetAllGuests();
        IEnumerable<Guest> GetGuestGridData();
        Dictionary<string, int> GetGuestStatusCounts();
    }
}
