using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using EMS.DAL.DBO;
using EMS.DAL.Interfaces;  

namespace EMS.DAL.DAL;

public class RolesDAL : IRolesDAL
{
    private readonly string _connectionString;
    
    public RolesDAL(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<Role> GetAllRoles()
    {
        List<Role> roles = new List<Role>();
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Role";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Role role = new Role
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                        Name = reader["Name"].ToString()
                    };
                    roles.Add(role);
                }
                reader.Close();
            }
        }
        catch
        {
            return null;
        }
        return roles;
    }

    public Role GetRoleFromName(string roleName)
    {
        Role role = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Role WHERE Name = @RoleName";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoleName", roleName);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    role = new Role
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString()
                    };
                }
                reader.Close();
            }
        }
        catch
        {
            return null;
        }
        return role;
    }

    public Role GetRoleById(int roleId)
    {
        Role role = null;
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Role WHERE Id = @RoleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoleId", roleId);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    role = new Role
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString()
                    };
                }
                reader.Close();
            }
        }
        catch
        {
            return null;
        }
        return role;
    }
    
    public bool AddRole(Role role)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Role (Name) VALUES (@RoleName)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoleName", role.Name);
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        catch
        {
            return false;
        }
    }

    public bool UpdateRoles(List<Role> roles)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                foreach (var role in roles)
                {
                    string query = "UPDATE Role SET Name = @RoleName WHERE Id = @RoleId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@RoleName", role.Name);
                    command.Parameters.AddWithValue("@RoleId", role.Id);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
        catch 
        {
            return false;
        }
    }

}
