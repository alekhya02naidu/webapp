using System;

namespace EMS.DAL.DTO
{
    public class Employee
    {
        public int Id { get; set; }
        public string EmpNo { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime Dob { get; set; }
        public string? Email { get; set; }
        public string MobileNumber { get; set; }
        public DateTime JoiningDate { get; set; }
        public int LocationId { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
        public bool IsManager { get; set; }
        public int ManagerId { get; set; }
        public int ProjectId { get; set; }
    }
}


