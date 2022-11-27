using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;
using Bumbodium.Data.Interfaces;

namespace Bumbodium.Data
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _configuration;
        public string ConnectionStringName { get; set; } = "Default";
        public SqlDataAccess(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            string ConnectionString = _configuration.GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);

                return data.ToList();
            }
        }

        public async Task<T> LoadSingleRecord<T, U>(string sql, U parameters)
        {
            string ConnectionString = _configuration.GetConnectionString(ConnectionStringName);
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);

                return data.FirstOrDefault();
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            string ConnectionString = _configuration.GetConnectionString(ConnectionStringName);

            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
