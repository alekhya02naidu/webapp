using System;
using CommandLine;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using EMS.BAL.BAL;
using EMS.DAL.DAL;
using EMS.DAL.DBO;
using EMS.DAL.DTO;
using EMS.BAL.Interfaces;
using EMS.Common.Helpers;
using EMS.Common.Logging;

namespace EmployeeManagement;

public static class Program
{   
    private static readonly IEmployeeBAL _employeeBal;
    private static readonly IMasterDataBal _masterDataBAL;
    private static readonly IRolesBAL _rolesBAL;
    private static IConfiguration _configuration;
    private static readonly ILogger _logger;
    private static readonly IWriter _console;
    
    static Program()
    {
        BuildConfiguration();
        string connectionString = _configuration.GetConnectionString("DBConnection");
        _console = new ConsoleWriter();
        _masterDataBAL = new MasterDataBAL(new MasterDataDAl(connectionString));
        _rolesBAL = new RolesBAL(new RolesDAL(connectionString));    
        _employeeBal = new EmployeeBAL(connectionString, new EmployeeDAL(connectionString), _masterDataBAL, _rolesBAL);
    }
    
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
        .WithParsed(option =>
        {
            if(option.Add)
            {
                AddEmployee();
            }
            else if(option.Filter)
            {
                FilterAndDisplay();
            }
            else if(option.Edit > 0)
            {
                Update();
            }
            else if (option.Delete.Count() > 0 && option.Delete.Any())
            {
                DeleteEmployee(option);
            }
            else if(option.Display)
            {
                List<EmployeeDetail> employees = _employeeBal.GetAllEmployees();
                DisplayEmployee(employees);
            }
            else if (option.AddRole)
            {
                AddRole(option);
            }
            else if(option.DisplayRoles)
            {
                DisplayAllRoles();
            }
            else
            {
                _console.PrintError("Invalid Command");
            }
        });
    }

    public static void BuildConfiguration()
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string appSettingsPath = Path.Combine(currentDirectory, "appSettings.json");
        _configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath, optional: true, reloadOnChange: true)
            .Build();
    }

    private static void AddEmployee()
    {
        try
        {
            if(_employeeBal.Add(GetEmployeeInputs()) != -1)
            {
                _console.PrintSuccess("Succesfully Added ");
            }
            else
            {
                _console.PrintError("Invalid");
            }
        }
        catch (Exception ex)    
        {
            _console.PrintError($"Error occured while adding employee : {ex.Message}");
        }
    }
    
    private static void AddRole(Options option)
    {
        try
        {
            if (option.RoleName == null || option.DepartmentId == 0)
            {
                _console.PrintError("Role ID and Department ID are required");
            }
            else
            {
                AddOrUpdateRole(option.DepartmentId, option.RoleName);
                _console.PrintSuccess("Added role "+option.RoleName+" to department");
            }
        }
        catch (Exception ex)
        {
            _console.PrintError($"Error found while adding role : {ex.Message}");
        }
    }
    
    private static void UpdateEmployeeDetails(Employee employee)
    {
        try
        {
            if(_employeeBal.Update(employee) > 0)
            {
                _console.PrintSuccess("Updated Successfully");
            }
            else
            {
                _console.PrintError("Cannot update");
            }
        }
        catch(Exception ex)
        {
            _console.PrintError($"Error found while updating : {ex.Message}");
        }
    }
    
    private static void DeleteEmployee(Options option)
    {
        try
        {
            int deletedEmployees = _employeeBal.Delete(option.Delete);
            if (deletedEmployees > 0)
            {
                _console.PrintSuccess("Deleted Successfully");
            }
            else
            {
                _console.PrintSuccess("Cannot find Employee");
            }
        }
        catch (Exception ex)
        {
            _console.PrintError($"Error found while deleting : {ex.Message}");
        }
    }

    public static Employee GetEmployeeInputs()
    {
        _console.PrintMsg("Enter Employee Details:");

        int id = 0;
 
        _console.PrintMsg("Employee Number: ");
        string empNo = Console.ReadLine() ?? "0";

        _console.PrintMsg("First Name: ");
        string firstName = Console.ReadLine() ?? "";

        _console.PrintMsg("Last Name: ");
        string lastName = Console.ReadLine() ?? "";

        _console.PrintMsg("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.PrintMsg("Email: ");
        string email = Console.ReadLine() ?? "";

        _console.PrintMsg("Mobile Number: ");
        string mobileNumber = Console.ReadLine() ?? "0";

        _console.PrintMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.PrintMsg("Select Location:");
        List<Location> locations = _masterDataBAL.GetAllLocations();
        foreach (var loc in locations)
        {
            _console.PrintMsg(loc.Name);
        }
        string locationInput = Console.ReadLine()?.Trim();
        Location location = _masterDataBAL.GetLocationFromName(locationInput);
        if (location == null)
        {
            _console.PrintError("Invalid location selected.");
            return null;
        }

        _console.PrintMsg("Select Department:");
        List<Department> departments = _masterDataBAL.GetAllDepartments();
        foreach (var dept in departments)
        {
            _console.PrintMsg(dept.Name);
        }
        string departmentInput = Console.ReadLine()?.Trim();
        Department department = _masterDataBAL.GetDepartmentFromName(departmentInput);
        if (department == null)
        {
            _console.PrintError("Invalid department selected.");
            return null;
        }

        _console.PrintMsg("Select Role:");
        List<Role> roles = _rolesBAL.GetAllRoles().Where(r => r.DepartmentId == department.Id).ToList();
        foreach (var r in roles)
        {
            _console.PrintMsg(r.Name);
        }
        string roleInput = Console.ReadLine()?.Trim();
        Role role = roles.FirstOrDefault(r => r.Name.ToLower() == roleInput);
        if (role == null)
        {
            _console.PrintError("Invalid role selected.");
            return null;
        }

        bool isManager = false;
        int managerId = 0;
        _console.PrintMsg("Are you a Manager? (Y/N): ");
        char isManagerInp = char.ToLower(Console.ReadLine()[0]);
        if (isManagerInp == 'y')
        {
            isManager = true;
        }
        else
        {
            _console.PrintMsg("Select Manager:");
            List<Employee> managers = _masterDataBAL.GetAllManagers();
            foreach (var mgr in managers)
            {
                _console.PrintMsg($"{mgr.FirstName}");
            }
            string managerName = Console.ReadLine()?.Trim();
            Employee manager = managers.FirstOrDefault(m => $"{m.FirstName}".Equals(managerName, StringComparison.OrdinalIgnoreCase));
            if (manager != null)
            {
                managerId = manager.Id;
            }
            else
            {
                _console.PrintError("Invalid manager selected.");
                return null;
            }
        }

        _console.PrintMsg("Select Project:");
        List<Project> projects = _masterDataBAL.GetAllProjects();
        foreach (var proj in projects)
        {
            _console.PrintMsg(proj.Name);
        }
        string projectInput = Console.ReadLine()?.Trim();
        Project project = projects.FirstOrDefault(p => p.Name.Equals(projectInput, StringComparison.OrdinalIgnoreCase));
        if (project == null)
        {
            _console.PrintError("Invalid project selected.");
            return null;
        }

        Employee employee = new Employee
        {
            Id = id,
            EmpNo = empNo,
            FirstName = firstName,
            LastName = lastName,
            Dob = dob,
            Email = email,
            MobileNumber = mobileNumber,
            JoiningDate = joiningDate,
            LocationId = location.Id,
            DepartmentId = department.Id,
            RoleId = role.Id,
            IsManager = isManager,
            ManagerId = managerId,
            ProjectId = project.Id,
        };
        return employee;
    }

    public static void DisplayEmployee(List<EmployeeDetail> employees)
    {
        if (employees == null && employees.Count == 0)
        {
            _console.PrintError("No employees found");
            return;
        }
        try
        {
            foreach(var employee in employees)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Id : " + (employee.Id));
                stringBuilder.AppendLine("EmpNo : " + (employee.EmpNo ?? "N/A"));
                stringBuilder.AppendLine("Name : " + (employee.FirstName ?? "N/A") + " " + (employee.LastName ?? "N/A"));
                stringBuilder.AppendLine("DOB : " + employee.Dob.ToString("dd-MM-yyyy")); 
                stringBuilder.AppendLine("Email : " + (employee.Email ?? "N/A"));
                stringBuilder.AppendLine("MobileNumber : " + (employee.MobileNumber ?? "N/A"));
                stringBuilder.AppendLine("JoiningDate : " + employee.JoiningDate.ToString("dd-MM-yyyy")); 
                stringBuilder.AppendLine("Location: " + (employee.LocationName ?? "N/A"));
                stringBuilder.AppendLine("Department: " + (employee.DepartmentName ?? "N/A"));
                stringBuilder.AppendLine("Role: " + (employee.RoleName ?? "N/A"));
                stringBuilder.AppendLine("Manager: " + (employee.ManagerName ?? "N/A"));
                stringBuilder.AppendLine("Project: " + (employee.ProjectName ?? "N/A"));
                _console.PrintMsg(stringBuilder.ToString());
            }
        }
        catch (Exception ex)
        {
            _console.PrintError($"Error occurred while displaying employee details: {ex.Message}");
        }
    } 
        
    public static void FilterAndDisplay()
    {
        _console.PrintMsg("Enter Alphabet");
        string alphabetFilter = Console.ReadLine();

        _console.PrintMsg("Enter Location");
        string locationFilter = Console.ReadLine();
        var location = _masterDataBAL.GetLocationFromName(locationFilter);
        int? locationId = location?.Id;

        _console.PrintMsg("Enter Department");
        string departmentFilter = Console.ReadLine();
        var department = _masterDataBAL.GetDepartmentFromName(departmentFilter);
        int? departmentId = department?.Id;

        _console.PrintMsg("Enter EmpNo to search");
        string empNoFilterString = Console.ReadLine(); 
        string empNoFilter = null;
        if (!string.IsNullOrEmpty(empNoFilterString))
        {
            empNoFilter = empNoFilterString;
        }
        
        EmployeeFilter filters = new EmployeeFilter
        {
            FirstName = alphabetFilter,
            LocationId = locationId,
            DepartmentId = departmentId,
            EmpNo = empNoFilter 
        };
        List<EmployeeDetail> filteredEmployees = _employeeBal.Filter(filters);
        if (filteredEmployees != null && filteredEmployees.Count > 0)
        {
            _console.PrintMsg("Employees found:");
            DisplayEmployee(filteredEmployees);
            _console.PrintSuccess("Filtered Successfully");
        }
        else
        {
            _console.PrintError("No employees found based on the provided filters.");
        }
    }

    public static void Update()
    {
        _console.PrintMsg("Employee Number: ");
        string empNo = Console.ReadLine()?? "0";

        EmployeeDetail existingEmployee = _employeeBal.GetAllEmployees().FirstOrDefault(emp => emp.EmpNo == empNo);
        if (existingEmployee == null)
        {
            _console.PrintError("Employee not found.");
            return;
        }

        _console.PrintMsg("First Name: ");
        string firstName = (Console.ReadLine()?? "");

        _console.PrintMsg("Last Name: ");
        string lastName = (Console.ReadLine()?? "");

        _console.PrintMsg("Date of Birth (MM-DD-YYYY): ");
        DateTime dob = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.PrintMsg("Email: ");
        string email = (Console.ReadLine()?? "");

        _console.PrintMsg("Mobile Number: ");
        string mobileNumber = (Console.ReadLine() ?? "0");

        _console.PrintMsg("Joining Date (MM-DD-YYYY): ");
        DateTime joiningDate = DateTime.Parse(Console.ReadLine() ?? "0000-00-00");

        _console.PrintMsg("Select Location:");
        List<Location> locations = _masterDataBAL.GetAllLocations();
        foreach (var loc in locations)
        {
            _console.PrintMsg(loc.Name);
        }
        string locationInput = Console.ReadLine()?.Trim();
        Location location = _masterDataBAL.GetLocationFromName(locationInput);
        if (location == null)
        {
            _console.PrintError("Invalid location selected.");
        }

        _console.PrintMsg("Select Department:");
        List<Department> departments = _masterDataBAL.GetAllDepartments();
        foreach (var dept in departments)
        {
            _console.PrintMsg(dept.Name);
        }
        string departmentInput = Console.ReadLine()?.Trim();
        Department department = _masterDataBAL.GetDepartmentFromName(departmentInput);
        if (department == null)
        {
            _console.PrintError("Invalid department selected.");
        }

        _console.PrintMsg("Select Role:");
        List<Role> roles = _rolesBAL.GetAllRoles().Where(r => r.DepartmentId == department.Id).ToList();
        foreach (var r in roles)
        {
            _console.PrintMsg(r.Name);
        }
        string roleInput = Console.ReadLine()?.Trim();
        Role role = roles.FirstOrDefault(r => r.Name.ToLower() == roleInput);
        if (role == null)
        {
            _console.PrintError("Invalid role selected.");
        }

        bool isManager = false;
        int managerId = 0;
        _console.PrintMsg("Are you a Manager? (Y/N): ");
        char isManagerInp = char.ToLower(Console.ReadLine()[0]);
        if (isManagerInp == 'y')
        {
            isManager = true;
        }
        else
        {
            _console.PrintMsg("Select Manager:");
            List<Employee> managers = _masterDataBAL.GetAllManagers();
            foreach (var mgr in managers)
            {
                _console.PrintMsg($"{mgr.FirstName}");
            }
            string managerName = Console.ReadLine()?.Trim();
            Employee manager = managers.FirstOrDefault(m => $"{m.FirstName}".Equals(managerName, StringComparison.OrdinalIgnoreCase));
            if (manager != null)
            {
                managerId = manager.Id;
            }
            else
            {
                _console.PrintError("Invalid manager selected.");
            }
        }

        _console.PrintMsg("Select Project:");
        List<Project> projects = _masterDataBAL.GetAllProjects();
        foreach (var proj in projects)
        {
            _console.PrintMsg(proj.Name);
        }
        string projectInput = Console.ReadLine()?.Trim();
        Project project = projects.FirstOrDefault(p => p.Name.Equals(projectInput, StringComparison.OrdinalIgnoreCase));
        if (project == null)
        {
            _console.PrintError("Invalid project selected.");
        }

        Employee employee = new Employee
        {
            EmpNo = empNo,
            FirstName = firstName,
            LastName = lastName,
            Dob = dob,
            Email = email,
            MobileNumber = mobileNumber,
            JoiningDate = joiningDate,
            LocationId = location?.Id ?? 0,
            DepartmentId = department?.Id ?? 0,
            RoleId = role?.Id ?? 0,
            IsManager = isManager,
            ManagerId = managerId,
            ProjectId = project?.Id ?? 0,
        };
        employee.Id = existingEmployee.Id;
        UpdateEmployeeDetails(employee);
    }

    private static void DisplayAllRoles()
    {
        StringBuilder roleString = new StringBuilder();
        List<Role> roles = _rolesBAL.GetAllRoles();
        
        foreach (var role in roles)
        {
            roleString.AppendLine($"Dept ID: {role.DepartmentId}\tRole ID: {role.Id}\tName: {role.Name}");
        }
        _console.PrintMsg(roleString.ToString());
    }

    public static bool AddOrUpdateRole(int departmentId, string roleName)
    {
        try
        {
            var department = _masterDataBAL.GetDepartmentById(departmentId); 
            if (department == null)
            {
                return false;
            }
            List<Role> roles = _rolesBAL.GetAllRoles();
            int roleId = roles.Count + 1; 
            Role role = new Role
            {
                Id = roleId,
                Name = roleName,
                DepartmentId = departmentId
            };
            roles.Add(role);
            _rolesBAL.UpdateRoles(roles);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class Options
{
    [Option('a', "add", Required = false, HelpText = "Add an employee")]
    public bool Add { get; set; }
    [Option('s', "display", Required = false, HelpText = "Display all employee info")]
    public bool Display { get; set; }
    [Option('r', "delete", Required = false, HelpText = "Delete employee data")]
    public IEnumerable<int>? Delete { get; set; }
    [Option('e', "edit", Required = false, HelpText = "Edit employee data")]
    public int Edit { get; set; }
    [Option('f', "filter", Required = false, HelpText = "Filter employee data")]
    public bool Filter { get; set; }
    [Option('p', "addrole", Required = false, HelpText = "Addition of roles to department")]
    public bool AddRole { get; set; }
    [Option('j', "displayroles", Required = false, HelpText = "Shows roles available to a department")]
    public bool DisplayRoles { get; set; }
    [Option('r', "roleId", Required = false, HelpText = "Role ID")]
    public int RoleId { get; set; }
    [Option('d', "departmentId", Required = false, HelpText = "Department ID")]
    public int DepartmentId { get; set; }
    [Option('n', "roleName", Required = false, HelpText = "Role Name")]
    public string RoleName { get; set; }
}
