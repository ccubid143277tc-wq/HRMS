using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Models;

namespace HRMS.Interfaces
{
    public interface IRoomTypeService
    {
        IEnumerable<RoomType> GetAllRoomTypes();
        RoomType GetRoomTypeById(int roomTypeId);
    }
}
