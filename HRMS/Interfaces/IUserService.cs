using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Models;

namespace HRMS.Interfaces
{
    public interface IUserService
    {

        int AddUser(User user);
        void UpdateUser(User user);

        void DeleteUser(int userId);

        IEnumerable<User> SearchUser(string keyword);
        User GetUserById(int userId);
        IEnumerable<User> GetAllUsers();
        IEnumerable<User> GetUserGridData();
        Dictionary<string, int> GetUserStatusCounts();
        
    }
}
