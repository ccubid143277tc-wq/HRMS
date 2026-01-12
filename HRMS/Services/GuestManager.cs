using HRMS.Interfaces;
using HRMS.Models;
using System.Collections.Generic;

namespace HRMS.Services
{
    public class GuestManager
    {
        private readonly IGuestService _guestService;

        public GuestManager(IGuestService guestService)
        {
            _guestService = guestService;
        }

        public int AddGuest(Guest guest)
        {
            return _guestService.AddGuest(guest);
        }

        public void UpdateGuest(Guest guest)
        {
            _guestService.UpdateGuest(guest);
        }

        public void DeleteGuest(int guestId)
        {
            _guestService.DeleteGuest(guestId);
        }

        public bool HasReservationsForGuest(int guestId)
        {
            return _guestService.HasReservationsForGuest(guestId);
        }

        public IEnumerable<Guest> SearchGuest(string keyword)
        {
            return _guestService.SearchGuest(keyword);
        }

        public Guest GetGuestById(int guestId)
        {
            return _guestService.GetGuestById(guestId);
        }

        public IEnumerable<Guest> GetAllGuests()
        {
            return _guestService.GetAllGuests();
        }

        public IEnumerable<Guest> GetGuestGridData()
        {
            return _guestService.GetGuestGridData();
        }

        public Dictionary<string, int> GetGuestStatusCounts()
        {
            return _guestService.GetGuestStatusCounts();
        }
    }
}
