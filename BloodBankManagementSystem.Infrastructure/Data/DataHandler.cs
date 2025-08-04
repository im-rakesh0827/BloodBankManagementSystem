using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Infrastructure.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.IO;
namespace BloodBankManagementSystem.Infrastructure.Data
{
    

public class DataHandler
{
    private readonly string _connectionString;

    public DataHandler(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
    {
        using var conn = CreateConnection();
        return await conn.QueryAsync<T>(sql, param);
    }

    public async Task<T> QuerySingleAsync<T>(string sql, object param = null)
    {
        using var conn = CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<T>(sql, param);
    }

    public async Task<int> ExecuteAsync(string sql, object param = null)
    {
        using var conn = CreateConnection();
        return await conn.ExecuteAsync(sql, param);
    }


    public async Task<DataSet> GetDataSetAsync(string commandText, SqlParameter[] parameters = null, CommandType commandType = CommandType.StoredProcedure)
{
    using var connection = new SqlConnection(_connectionString);
    using var command = new SqlCommand(commandText, connection)
    {
        CommandType = commandType
    };

    if (parameters != null)
        command.Parameters.AddRange(parameters);

    using var adapter = new SqlDataAdapter(command);
    var dataSet = new DataSet();
    await Task.Run(() => adapter.Fill(dataSet));
    return dataSet;
}

}

}