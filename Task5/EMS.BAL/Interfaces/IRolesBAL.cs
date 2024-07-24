using System.Collections.Generic;
using EMS.DAL.DBO;

namespace EMS.BAL.Interfaces;

public interface IRolesBAL
{
    List<Role> GetAllRoles();
    Role GetRoleFromName(string roleInput);
    Role GetRoleById(int roleId);
    bool AddRole(Role role);
    bool UpdateRoles(List<Role> roles);
}