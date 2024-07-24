using System.Collections.Generic;
using EMS.DAL.DBO;
using EMS.DAL.DTO;

namespace EMS.DAL.Interfaces;

public interface IEmployeeDAL
{
    int Insert(Employee employee);
    int Update(Employee employee);
    int Delete(int id);
    List<EmployeeDetail> Filter(EmployeeFilter filters);
    List<EmployeeDetail> GetAll();
}