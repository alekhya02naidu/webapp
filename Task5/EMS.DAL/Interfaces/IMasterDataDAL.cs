using System.Collections.Generic;
using EMS.DAL.DTO;
using EMS.DAL.DBO;

namespace EMS.DAL.Interfaces;

public interface IMasterDataDal
{
    List<Location> GetAllLocations();
    List<Department> GetAllDepartments();
    List<Employee> GetAllManagers();
    List<Project> GetAllProjects();
    Location GetLocationFromName(string locationName);
    Department GetDepartmentFromName(string departmentName);
    Employee GetManagerFromName(string managerName);
    Project GetProjectFromName(string projectName);
    Location GetLocationById(int locationId);
    Department GetDepartmentById(int departmentId);
    string GetManagerNameById(int managerId);
    Project GetProjectById(int projectId);
}