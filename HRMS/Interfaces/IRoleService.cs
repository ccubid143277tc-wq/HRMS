using System;
using System.Collections.Generic;
using System.Text;
using HRMS.Models;

namespace HRMS.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAllRoles();
        Role GetRoleById(int roleId);
    }
}
