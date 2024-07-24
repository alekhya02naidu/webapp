using System.Collections.Generic;
using EMS.DAL.DBO;
using EMS.DAL.DTO;

namespace EMS.BAL.Interfaces;

public interface IEmployeeBAL
{
    int Add(Employee employee) ;
    int Update(Employee updatedEmployee);
    int Delete(IEnumerable<int> ids);
    List<EmployeeDetail> Filter(EmployeeFilter filters);
    List<EmployeeDetail> GetAllEmployees();
}