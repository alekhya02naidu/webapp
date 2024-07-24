using System;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using EMS.DAL.DBO;
using EMS.DAL.DTO;
using EMS.DAL.Interfaces;

namespace EMS.DAL.DAL;
public class EmployeeDAL : IEmployeeDAL
{
    private readonly string _connectionString;
    
    public EmployeeDAL(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public int Insert(Employee employee)
    {
        int EmployeeId = 0;
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(_connectionString);
            connection.Open();

            string query;
            if (employee.IsManager)
            {
                query = "INSERT INTO Employee (Uid, FirstName, LastName, Dob, Email, MobileNumber, JoiningDate, LocationId, DepartmentId, RoleId, IsManager, ProjectId) " +
                        "VALUES (@Uid, @FirstName, @LastName, @Dob, @Email, @MobileNumber, @JoiningDate, @LocationId, @DepartmentId, @RoleId, @IsManager, @ProjectId)";
            }
            else
            {
                query = "INSERT INTO Employee (Uid, FirstName, LastName, Dob, Email, MobileNumber, JoiningDate, LocationId, DepartmentId, RoleId, IsManager, ManagerId, ProjectId) " +
                        "VALUES (@Uid, @FirstName, @LastName, @Dob, @Email, @MobileNumber, @JoiningDate, @LocationId, @DepartmentId, @RoleId, @IsManager, @ManagerId, @ProjectId)";
            }
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.Add(new SqlParameter("@Uid", SqlDbType.VarChar) { Value = employee.EmpNo });
            command.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.VarChar) { Value = employee.FirstName });   
            command.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar) { Value = employee.LastName });
            command.Parameters.Add(new SqlParameter("@DOB", SqlDbType.Date) { Value = employee.Dob });
            command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar) { Value = employee.Email });
            command.Parameters.Add(new SqlParameter("@MobileNumber", SqlDbType.VarChar) { Value = employee.MobileNumber });
            command.Parameters.Add(new SqlParameter("@JoiningDate", SqlDbType.Date) { Value = employee.JoiningDate });
            command.Parameters.Add(new SqlParameter("@LocationId", SqlDbType.Int) { Value = employee.LocationId });
            command.Parameters.Add(new SqlParameter("@DepartmentId", SqlDbType.Int) { Value = employee.DepartmentId });
            command.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.Int) { Value = employee.RoleId });
            command.Parameters.Add(new SqlParameter("@IsManager", SqlDbType.Bit) { Value = employee.IsManager });
            command.Parameters.Add(new SqlParameter("@ProjectId", SqlDbType.Int) { Value = employee.ProjectId });
            if (!employee.IsManager)
            {
                command.Parameters.AddWithValue("@ManagerId", employee.ManagerId);
            }
            EmployeeId = Convert.ToInt32(command.ExecuteScalar());
        }
        catch
        {
            EmployeeId = 0;
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
        return EmployeeId;
    }
    
    public int Update(Employee employee)
    {
        int rowsAffected = 0;
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(_connectionString);
            connection.Open();
            string updateQuery = "UPDATE Employee SET FirstName = @FirstName, LastName = @LastName, Dob = @Dob, Email = @Email, " +
                "MobileNumber = @MobileNumber, JoiningDate = @JoiningDate, LocationId = @LocationId, DepartmentId = @DepartmentId, " +
                "RoleId = @RoleId, IsManager = @IsManager, ManagerId = @ManagerId, ProjectId = @ProjectId WHERE Uid = @Uid";

            SqlCommand command = new SqlCommand(updateQuery, connection);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Dob", employee.Dob);
            command.Parameters.AddWithValue("@Email", employee.Email);
            command.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
            command.Parameters.AddWithValue("@JoiningDate", employee.JoiningDate);
            command.Parameters.AddWithValue("@LocationId", employee.LocationId);
            command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
            command.Parameters.AddWithValue("@RoleId", employee.RoleId);
            command.Parameters.AddWithValue("@IsManager", employee.IsManager);
            command.Parameters.AddWithValue("@ManagerId", employee.ManagerId);
            command.Parameters.AddWithValue("@ProjectId", employee.ProjectId);
            command.Parameters.AddWithValue("@Uid", employee.EmpNo);
            rowsAffected = command.ExecuteNonQuery();
        }
        catch
        {
            rowsAffected = -1;
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
        return rowsAffected;
    }

    public int Delete(int Id)
    {
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(_connectionString);
            connection.Open();
            string deleteQuery = "DELETE FROM Employee WHERE Id = @Id";
            SqlCommand command = new SqlCommand(deleteQuery, connection);
            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = Id });
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected;
        }
        catch
        {
            return -1;
        }
        finally
        {
            if(connection != null)
            {
                connection.Close();
            }
        }
    }

    private EmployeeDetail AssignEmployeeFromDataReader(SqlDataReader reader)
    {
        EmployeeDetail employee = new EmployeeDetail
        {
            Id = Convert.ToInt32(reader["Id"]),
            EmpNo = reader["Uid"].ToString(),
            FirstName = reader["FirstName"].ToString(),
            LastName = reader["LastName"].ToString(),
            Dob = reader["Dob"] != DBNull.Value ? Convert.ToDateTime(reader["Dob"]) : DateTime.MinValue,
            Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
            MobileNumber = reader["MobileNumber"].ToString(),
            JoiningDate = reader["JoiningDate"] != DBNull.Value ? Convert.ToDateTime(reader["JoiningDate"]) : DateTime.MinValue,
            LocationName = reader["LocationName"] != DBNull.Value ? reader["LocationName"].ToString() : "",
            DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : "",
            RoleName = reader["RoleName"] != DBNull.Value ? reader["RoleName"].ToString() : "",
            IsManager = reader["IsManager"] != DBNull.Value && Convert.ToBoolean(reader["IsManager"]),
            ManagerName = reader["IsManager"] != DBNull.Value && Convert.ToBoolean(reader["IsManager"]) ? "Null" : reader["ManagerName"].ToString(),
            ProjectName = reader["ProjectName"] != DBNull.Value ? reader["ProjectName"].ToString() : ""
        };
        return employee;
    }

    public List<EmployeeDetail> Filter(EmployeeFilter filters)
    {
        List<EmployeeDetail> filteredEmps = new List<EmployeeDetail>();
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(_connectionString);
            connection.Open();
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("SELECT e.Id, e.Uid, e.FirstName, e.LastName, e.Dob, e.Email, e.MobileNumber, e.JoiningDate, ");
            queryBuilder.Append("l.Name AS LocationName, d.Name AS DepartmentName, r.Name AS RoleName, ");
            queryBuilder.Append("e.IsManager, m.FirstName AS ManagerName, p.Name AS ProjectName ");
            queryBuilder.Append("FROM Employee e ");
            queryBuilder.Append("JOIN Location l ON e.LocationId = l.Id ");
            queryBuilder.Append("JOIN Department d ON e.DepartmentId = d.Id ");
            queryBuilder.Append("JOIN Role r ON e.RoleId = r.Id ");
            queryBuilder.Append("LEFT JOIN Employee m ON e.ManagerId = m.Id ");
            queryBuilder.Append("JOIN Project p ON e.ProjectId = p.Id ");

            bool hasFilters = false; 
            if (!string.IsNullOrEmpty(filters.FirstName) || filters.LocationId.HasValue || filters.DepartmentId.HasValue || !string.IsNullOrEmpty(filters.EmpNo))
            {
                queryBuilder.Append("WHERE ");
                hasFilters = true;
            }
            bool isFirstFilter = true; 
            if (!string.IsNullOrEmpty(filters.FirstName))
            {
                if (!isFirstFilter) queryBuilder.Append("AND ");
                queryBuilder.Append("e.FirstName LIKE @FirstName ");
                isFirstFilter = false;
            }
            if (filters.LocationId.HasValue)
            {
                if (!isFirstFilter) queryBuilder.Append("AND ");
                queryBuilder.Append("e.LocationId = @LocationId ");
                isFirstFilter = false;
            }
            if (filters.DepartmentId.HasValue)
            {
                if (!isFirstFilter) queryBuilder.Append("AND ");
                queryBuilder.Append("e.DepartmentId = @DepartmentId ");
                isFirstFilter = false;
            }
            if (!string.IsNullOrEmpty(filters.EmpNo))
            {
                if (!isFirstFilter) queryBuilder.Append("AND ");
                queryBuilder.Append("e.Uid = @Uid ");
                isFirstFilter = false;
            }
            using (SqlCommand command = new SqlCommand(queryBuilder.ToString(), connection))
            {
                if (!string.IsNullOrEmpty(filters.FirstName))
                {
                    command.Parameters.AddWithValue("@FirstName" , filters.FirstName + "%");
                }
                if (filters.LocationId.HasValue)
                {
                    command.Parameters.AddWithValue("@LocationId", filters.LocationId.Value);
                }
                if (filters.DepartmentId.HasValue)
                {
                    command.Parameters.AddWithValue("@DepartmentId", filters.DepartmentId.Value);
                }
                if (!string.IsNullOrEmpty(filters.EmpNo))
                {
                    command.Parameters.AddWithValue("@Uid", filters.EmpNo);
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        filteredEmps.Add(AssignEmployeeFromDataReader(reader));
                    }
                }
            }
            if (!hasFilters)
            {
                return filteredEmps;
            }
            if (filteredEmps.Count == 0)
            {
                return null;
            }
        }
        catch 
        {
            return null;
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
        return filteredEmps;
    }

    public List<EmployeeDetail> GetAll()
    {
        List<EmployeeDetail> employeeDetails = new List<EmployeeDetail>();
        SqlConnection connection = null;
        try
        {
            connection = new SqlConnection(_connectionString);
            connection.Open();
            string query = "SELECT e.Id, e.Uid, e.FirstName, e.LastName, e.Dob, e.Email, e.MobileNumber, e.JoiningDate, " +
                            "l.Name AS LocationName, d.Name AS DepartmentName, r.Name AS RoleName, " +
                            "e.IsManager, m.FirstName AS ManagerName, p.Name AS ProjectName " +
                            "FROM Employee e " +
                            "JOIN Location l ON e.LocationId = l.Id " +
                            "JOIN Department d ON e.DepartmentId = d.Id " +
                            "JOIN Role r ON e.RoleId = r.Id " +
                            "LEFT JOIN Employee m ON e.ManagerId = m.Id " +
                            "JOIN Project p ON e.ProjectId = p.Id";
            SqlCommand command = new SqlCommand(query, connection);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    employeeDetails.Add(AssignEmployeeFromDataReader(reader));
                }
            }
        }
        catch
        {
            return null;
        }
        finally
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
        return employeeDetails;
    }
}

