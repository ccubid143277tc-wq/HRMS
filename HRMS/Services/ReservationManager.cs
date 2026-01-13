using HRMS.Interfaces;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRMS.Services
{
    public class ReservationManager
    {
        private readonly IReservationService _repo;

        public ReservationManager(IReservationService repo)
        {
            _repo = repo;
        }

        public int AddReservation(Reservation reservation)
        {
            return _repo.AddReservation(reservation);
        }

        public void UpdateReservation(Reservation reservation)
        {
            _repo.UpdateReservation(reservation);
        }

        public void DeleteReservation(int reservationId)
        {
            _repo.DeleteReservation(reservationId);
        }

        public void CancelReservation(int reservationId)
        {
            _repo.CancelReservation(reservationId);
        }

        public IEnumerable<Reservation> SearchReservation(string keyword)
        {
            return _repo.SearchReservation(keyword);
        }

        public Reservation GetReservationById(int reservationId)
        {
            return _repo.GetReservationById(reservationId);
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return _repo.GetAllReservations();
        }

        public IEnumerable<Reservation> GetReservationGridData()
        {
            return _repo.GetReservationGridData();
        }

        public DataTable GetExpectedArrivalsGridData(DateTime date)
        {
            return _repo.GetExpectedArrivalsGridData(date);
        }

        public DataTable GetExpectedDeparturesGridData(DateTime date)
        {
            return _repo.GetExpectedDeparturesGridData(date);
        }

        public Dictionary<string, int> GetReservationStatusCounts()
        {
            return _repo.GetReservationStatusCounts();
        }

        public bool CheckRoomAvailability(int roomId, DateTime checkIn, DateTime checkOut)
        {
            return _repo.CheckRoomAvailability(roomId, checkIn, checkOut);
        }

        public decimal CalculateReservationAmount(int roomId, DateTime checkIn, DateTime checkOut, int numAdults, int numChildren)
        {
            return _repo.CalculateReservationAmount(roomId, checkIn, checkOut, numAdults, numChildren);
        }

        public IEnumerable<Room> GetAvailableRoomsByType(int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            return _repo.GetAvailableRoomsByType(roomTypeId, checkIn, checkOut);
        }

        public IEnumerable<RoomType> GetAllRoomTypes()
        {
            return _repo.GetAllRoomTypes();
        }

        public void AddReservationRooms(int reservationId, List<int> roomIds)
        {
            _repo.AddReservationRooms(reservationId, roomIds);
        }

        public List<int> GetRoomIdsByReservation(int reservationId)
        {
            return _repo.GetRoomIdsByReservation(reservationId);
        }
    }
}
