using System;
using System.Collections.Generic;
using EMS.BAL.Interfaces;
using EMS.DAL.Interfaces;
using EMS.DAL.DTO;
using EMS.DAL.DBO;

namespace EMS.BAL.BAL;

public class MasterDataBAL : IMasterDataBal
{
    private readonly IMasterDataDal _masterDataDAL;

    public MasterDataBAL(IMasterDataDal masterDataDal)
    {
        _masterDataDAL = masterDataDal;
    }
    
    public List<Location> GetAllLocations()
    {
        return _masterDataDAL.GetAllLocations();
    }
    
    public List<Department> GetAllDepartments()
    {
        return _masterDataDAL.GetAllDepartments();
    }
    
    public List<Employee> GetAllManagers()
    {
        return _masterDataDAL.GetAllManagers();
    }
    
    public List<Project> GetAllProjects()
    {
        return _masterDataDAL.GetAllProjects();
    }
    
    public Location GetLocationFromName(string locationName)
    {
        return _masterDataDAL.GetLocationFromName(locationName);
    }

    public Department GetDepartmentFromName(string departmentName)
    {
        return _masterDataDAL.GetDepartmentFromName(departmentName);
    }

    public Employee GetManagerFromName(string managerName)
    {
        return _masterDataDAL.GetManagerFromName(managerName);
    }

    public Project GetProjectFromName(string projectName)
    {
        return _masterDataDAL.GetProjectFromName(projectName);
    }
    
    public Location GetLocationById(int locationId)
    {
        return _masterDataDAL.GetLocationById(locationId);
    }

    public Department GetDepartmentById(int departmentId)
    {
        return _masterDataDAL.GetDepartmentById(departmentId);
    }

    public string GetManagerNameById(int managerId)
    {
        return _masterDataDAL.GetManagerNameById(managerId);
    }

    public Project GetProjectById(int projectId)
    {
        return _masterDataDAL.GetProjectById(projectId);
    }
}