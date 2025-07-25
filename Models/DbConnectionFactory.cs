// 创建文件 Data/DbConnectionFactory.cs
using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace LibraryManagementSystem.Data
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;
        
        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        
        public IDbConnection CreateConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}