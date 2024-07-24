using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using EMS.BAL.Interfaces;
using EMS.DAL.Interfaces;
using EMS.DAL.DBO;
using EMS.DAL.DTO;

namespace EMS.BAL.BAL;
public class EmployeeBAL : IEmployeeBAL
{
    private readonly string _connectionString;
    private readonly IEmployeeDAL _employeeDal;
     private readonly IMasterDataBal _masterDataBAL;
    private readonly IRolesBAL _rolesBAL;

    public EmployeeBAL(string connectionString, IEmployeeDAL employeeDAL,IMasterDataBal masterDataBal, IRolesBAL rolesBAL)
    {
        _connectionString = connectionString;
        _employeeDal = employeeDAL;
        _masterDataBAL = masterDataBal;
        _rolesBAL = rolesBAL;
    }

    public int Add(Employee employee)
    {
        if (!(ValidateEmployeeInputData(employee)))
        {
            return -1;
        }
        return _employeeDal.Insert(employee);
    }

    public int Update(Employee updatedEmployee)
    {
        int updatedEmployeeCount = 0;
        List<EmployeeDetail> existingEmployees = _employeeDal.GetAll();
        EmployeeDetail existingEmployee = existingEmployees.Find(emp => emp.Id == updatedEmployee.Id);
        if (!(ValidateEmployeeInputData(updatedEmployee)))
        {
            return -1;
        }
        if (existingEmployee != null)
        {
            updatedEmployeeCount += _employeeDal.Update(updatedEmployee);
        }
        return updatedEmployeeCount;
    }

    public int Delete(IEnumerable<int> ids)
    {
        try
        {
            int count = 0;
            foreach (var id in ids)
            {
                count += _employeeDal.Delete(id);
            }
            return count;
        }
        catch
        {
            return -1;
        }
    }

    private bool ValidateEmployeeInputData(Employee employee)
    {
        if(employee == null || string.Equals(employee.EmpNo,"0"))   
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(employee.FirstName) || string.IsNullOrWhiteSpace(employee.LastName))
        {
            return false; 
        }
        if(string.IsNullOrWhiteSpace(employee.Email) || !(employee.Email.Contains("@") && employee.Email.Contains(".")))
        {
            return false;
        }
        if(employee.MobileNumber.Length > 1)
        {
            if(employee.MobileNumber.Length != 10)
            {
               return false;
            }
        }
        if(employee.LocationId != 0)
        {
            if (employee.LocationId < 1 || employee.LocationId > 3)
            {
                return false;
            }
        }
        if(_rolesBAL == null)
        {
            return false;
        }
        var departments = _masterDataBAL.GetAllDepartments();
        var department = departments.FirstOrDefault(d => d.Id == employee.DepartmentId);
        if (department == null)
        {
            return false; 
        }
        var roles = _rolesBAL.GetAllRoles().Where(r => r.DepartmentId == employee.DepartmentId);
        if (!roles.Any())
        {
            return false; 
        }

        if(employee.ProjectId != 0)
        {
            if(employee.ProjectId != 1 && employee.ProjectId != 2)
            {
                return false;
            }
        }
        return true;
    }
    
    public List<EmployeeDetail> Filter(EmployeeFilter filters)
    {
        return _employeeDal.Filter(filters);
    }

    public List<EmployeeDetail> GetAllEmployees()
    {
        List<EmployeeDetail> employees = _employeeDal.GetAll();
        if (employees == null)
        {
            employees = new List<EmployeeDetail>(); 
        }
        return employees;
    }
}