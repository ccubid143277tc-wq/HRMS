using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using HRMS.Models;

namespace HRMS.Interfaces
{
    public interface IReservationService
    {
        int AddReservation(Reservation reservation);
        void UpdateReservation(Reservation reservation);
        void DeleteReservation(int reservationId);
        void CancelReservation(int reservationId);
        
        IEnumerable<Reservation> SearchReservation(string keyword);
        Reservation GetReservationById(int reservationId);
        IEnumerable<Reservation> GetAllReservations();
        IEnumerable<Reservation> GetReservationGridData();
        DataTable GetExpectedArrivalsGridData(DateTime date);
        DataTable GetExpectedDeparturesGridData(DateTime date);
        Dictionary<string, int> GetReservationStatusCounts();
        bool CheckRoomAvailability(int roomId, DateTime checkIn, DateTime checkOut);
        decimal CalculateReservationAmount(int roomId, DateTime checkIn, DateTime checkOut, int numAdults, int numChildren);
        
        // New methods for room availability by type
        IEnumerable<Room> GetAvailableRoomsByType(int roomTypeId, DateTime checkIn, DateTime checkOut);
        IEnumerable<RoomType> GetAllRoomTypes();

        void AddReservationRooms(int reservationId, List<int> roomIds);
        List<int> GetRoomIdsByReservation(int reservationId);
    }
}
