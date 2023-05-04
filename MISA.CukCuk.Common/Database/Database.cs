using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.Common.Database
{
    public class Database : IDatabase
    {
        private readonly IConfiguration _configuration;

        private readonly string _connectionString;

        public Database(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }

        /// <summary>
        /// Hàm khởi tạo kết nối
        /// </summary>
        /// <returns>Kết quả kết nối với DB</returns>
        public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
    }
}
