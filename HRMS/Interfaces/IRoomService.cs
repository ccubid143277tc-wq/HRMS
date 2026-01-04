using HRMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;


namespace HRMS.Interfaces
{
    //Interface Segregation Principle applied
    // focused only in Room operations
    public interface IRoomService
    {
        int AddRoom(Room room);
        void UpdateRoom(Room room);
        void DeleteRoom(int roomId);
        Room GetRoomById(int roomId);
        IEnumerable<Room> SearchRooms(string keyword);
        IEnumerable<Room> FilterRooms(int? roomTypeId, int? roomStatusId, decimal? minRate, decimal? maxRate);
        IEnumerable<Room> GetAllRooms();
        void AddRoomAmenity(int roomId, int amenityId);
        List<int> GetRoomAmenities(int roomId);
        DataTable GetRoomGridData();
        IEnumerable<RoomType> GetRoomTypes();
        IEnumerable<Room> GetAvailableRoomsByType(string roomTypeName);
        bool IsRoomAvailable(int roomId, DateTime checkInDate, DateTime checkOutDate);

    }
}
