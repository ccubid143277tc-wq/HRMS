using HRMS.Interfaces;
using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRMS.Services
{
    public class RoomManager
    {
        private readonly IRoomService _repo;

        public RoomManager(IRoomService repo)
        {
            _repo = repo;
        }

        public int AddRoom(Room room)
        {
            return _repo.AddRoom(room);
        }

        public void UpdateRoom(Room room)
        {
            _repo.UpdateRoom(room);
        }

        public void DeleteRoom(int roomId)
        {
            _repo.DeleteRoom(roomId);
        }

        public Room GetRoomById(int roomId)
        {
            return _repo.GetRoomById(roomId);
        }

        public IEnumerable<Room> SearchRooms(string keyword)
        {
            return _repo.SearchRooms(keyword);
        }

        public IEnumerable<Room> FilterRooms(int? roomTypeId, int? roomStatusId, decimal? minRate, decimal? maxRate)
        {
            return _repo.FilterRooms(roomTypeId, roomStatusId, minRate, maxRate);
        }

        public IEnumerable<Room> GetAllRooms()
        {
            return _repo.GetAllRooms();
        }

        public void AddRoomAmenity(int roomId, int amenityId)
        {
            _repo.AddRoomAmenity(roomId, amenityId);
        }

        public List<int> GetRoomAmenities(int roomId)
        {
            return _repo.GetRoomAmenities(roomId);
        }

        public DataTable GetRoomGridData()
        {
            return _repo.GetRoomGridData();
        }

        public IEnumerable<RoomType> GetRoomTypes()
        {
            return _repo.GetRoomTypes();
        }

        public IEnumerable<Room> GetAvailableRoomsByType(string roomTypeName)
        {
            return _repo.GetAvailableRoomsByType(roomTypeName);
        }

        public bool IsRoomAvailable(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            return _repo.IsRoomAvailable(roomId, checkInDate, checkOutDate);
        }

        public bool UpdateRoomStatus(int roomId, string status)
        {
            return _repo.UpdateRoomStatus(roomId, status);
        }

        public Dictionary<string, int> GetRoomStatusCounts()
        {
            return _repo.GetRoomStatusCounts();
        }

        public int GetOccupiedRoomCountByDate(DateTime date)
        {
            return _repo.GetOccupiedRoomCountByDate(date);
        }

        public Dictionary<DateTime, int> GetWeeklyOccupiedRoomCounts(DateTime startDate, int days)
        {
            return _repo.GetWeeklyOccupiedRoomCounts(startDate, days);
        }

        public int GetExpectedArrivalsCount(DateTime date)
        {
            return _repo.GetExpectedArrivalsCount(date);
        }

        public int GetExpectedDeparturesCount(DateTime date)
        {
            return _repo.GetExpectedDeparturesCount(date);
        }
    }
}
