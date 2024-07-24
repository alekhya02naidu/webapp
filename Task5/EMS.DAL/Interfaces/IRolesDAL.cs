using System.Collections.Generic;
using EMS.DAL.DBO;

namespace EMS.DAL.Interfaces;

public interface IRolesDAL
{
    List<Role> GetAllRoles();
    Role GetRoleFromName(string roleName);
    Role GetRoleById(int roleId);
    bool AddRole(Role role);
    bool UpdateRoles(List<Role> roles);
}
