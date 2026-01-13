using HRMS.Interfaces;
using HRMS.Models;
using System;
using System.Collections.Generic;

namespace HRMS.Services
{
    public class UserManager
    {
        private readonly IUserService _repo;

        public UserManager(IUserService repo)
        {
            _repo = repo;
        }

        public int AddUser(User user)
        {
            return _repo.AddUser(user);
        }

        public void UpdateUser(User user)
        {
            _repo.UpdateUser(user);
        }

        public void DeleteUser(int userId)
        {
            _repo.DeleteUser(userId);
        }

        public IEnumerable<User> SearchUser(string keyword)
        {
            return _repo.SearchUser(keyword);
        }

        public User GetUserById(int userId)
        {
            return _repo.GetUserById(userId);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _repo.GetAllUsers();
        }

        public IEnumerable<User> GetUserGridData()
        {
            return _repo.GetUserGridData();
        }

        public Dictionary<string, int> GetUserStatusCounts()
        {
            return _repo.GetUserStatusCounts();
        }

        public User AuthenticateUser(string username, string password)
        {
            return _repo.AuthenticateUser(username, password);
        }
    }
}
