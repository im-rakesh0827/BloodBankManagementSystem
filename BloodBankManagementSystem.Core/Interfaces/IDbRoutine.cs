using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;


namespace BloodBankManagementSystem.Core.Interfaces
{
    public interface IDbRoutine
{
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int userId);
    Task<int> CreateUserAsync(User user);
    // Task<DataSet> GetDataSetAsync(string storedProc, SqlParameter[] parameters);
}

}