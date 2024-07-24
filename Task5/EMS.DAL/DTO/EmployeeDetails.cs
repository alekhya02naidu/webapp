using System;

namespace EMS.DAL.DTO
{
    public class EmployeeDetail
    {
        public int Id { get; set; }
        public string EmpNo { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Dob { get; set; }
        public string? Email { get; set; }
        public string MobileNumber { get; set; }
        public DateTime JoiningDate { get; set; }
        public string LocationName { get; set; }
        public string DepartmentName { get; set; }
        public string RoleName { get; set; }
        public bool IsManager { get; set; }
        public string ManagerName { get; set; }
        public string ProjectName { get; set; }
    }
}


