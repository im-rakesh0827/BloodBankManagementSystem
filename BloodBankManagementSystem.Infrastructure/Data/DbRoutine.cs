using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.Core.Interfaces;
using System.Data;

namespace BloodBankManagementSystem.Infrastructure.Data
{
    public class DbRoutine : IDbRoutine
{
    private readonly DataHandler _handler;

    public DbRoutine(DataHandler handler)
    {
        _handler = handler;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var query = "SELECT * FROM Users";
        return (await _handler.QueryAsync<User>(query)).ToList();
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var query = "SELECT * FROM Users WHERE Id = @UserId";
        var parameters = new { UserId = userId };
        return await _handler.QuerySingleAsync<User>(query, parameters);
    }

    public async Task<int> CreateUserAsync(User user)
    {
        var query = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)";
        return await _handler.ExecuteAsync(query, user);
    }


    public async Task<User> GetUserFromDataSetAsync(int userId)
    {
        var parameters = new Dictionary<string, object>
        {
            { "@UserId", userId }
        };
        var ds = await _handler.GetDataSetAsync("sp_GetData", null, CommandType.StoredProcedure);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            var row = ds.Tables[0].Rows[0];
            var user = new User
            {
                Id = Convert.ToInt32(row["Id"]),
                FirstName = row["FirstName"].ToString(),
                Email = row["Email"].ToString(),
                Role = row["Role"].ToString()
            };

            return user;
        }

        return null;
    }



    
}

    }
