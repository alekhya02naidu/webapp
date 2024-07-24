using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using EMS.DAL.DBO;
using EMS.DAL.DTO;
using EMS.DAL.Interfaces;  

namespace EMS.DAL.DAL;

public class MasterDataDAl : IMasterDataDal
{
    private readonly string _connectionString;

    public MasterDataDAl(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Location> GetAllLocations()
    {
        List<Location> locations = new List<Location>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Location";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Location location = new Location
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
                locations.Add(location);
            }
            reader.Close();
        }
        return locations;
    }

    public List<Department> GetAllDepartments()
    {
        List<Department> departments = new List<Department>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Department";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Department department = new Department
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
                departments.Add(department);
            }
            reader.Close();
        }
        return departments;
    }

    public List<Employee> GetAllManagers()
    {
        List<Employee> managers = new List<Employee>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT Id, FirstName FROM Employee where IsManager = 1";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Employee manager = new Employee
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    FirstName = reader["FirstName"].ToString()
                };
                managers.Add(manager);
            }
            reader.Close();
        }
        return managers;
    }

    public List<Project> GetAllProjects()
    {
        List<Project> projects = new List<Project>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Project";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Project project = new Project
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
                projects.Add(project);
            }
            reader.Close();
        }
        return projects;
    }

    public Location GetLocationFromName(string locationName)
    {
        Location location = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT Id, Name FROM Location WHERE Name = @LocationName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocationName", locationName);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                location = new Location
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
            }
            reader.Close();
        }
        return location;
    }

    public Department GetDepartmentFromName(string departmentName)
    {
        Department department = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Department WHERE Name = @DepartmentName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentName", departmentName);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                department = new Department
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
            }
            reader.Close();
        }
        return department;
    }

    public Employee GetManagerFromName(string managerName)
    {
        Employee manager = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT Id, FirstName FROM Employee WHERE Name = @FirstName AND IsManager = 1";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", managerName);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                manager = new Employee
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    FirstName = reader["Name"].ToString()
                };
            }
            reader.Close();
        }
        return manager;
    }

    public Project GetProjectFromName(string projectName)
    {
        Project project = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Project WHERE Name = @ProjectName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProjectName", projectName);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                project = new Project
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
            }
            reader.Close();
        }
        return project;
    }

    public Location GetLocationById(int locationId)
    {
        Location location = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Location WHERE Id = @LocationId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocationId", locationId);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                location = new Location
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
            }
            reader.Close();
        }
        return location;
    }

    public Department GetDepartmentById(int departmentId)
    {
        Department department = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Department WHERE Id = @DepartmentId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentId", departmentId);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                department = new Department
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
            }
            reader.Close();
        }
        return department;
    }

    public string GetManagerNameById(int managerId)
    {
        string managerName = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT e.FirstName FROM Employee e " +
                            "INNER JOIN Employee mngr ON e.Id = mngr.ManagerId WHERE mngr.Id = @ManagerId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ManagerId", managerId);
            object result = command.ExecuteScalar();

            if (result != null)
            {
                managerName = result.ToString();
            }
        }
        return managerName;
    }

    public Project GetProjectById(int projectId)
    {
        Project project = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Project WHERE Id = @ProjectId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProjectId", projectId);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                project = new Project
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString()
                };
            }
            reader.Close();
        }
        return project;
    }
}