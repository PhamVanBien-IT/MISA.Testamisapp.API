using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.Common.Database
{
    public interface IDatabase
    {
        /// <summary>
        /// Hàm khởi tạo kết nối với DB
        /// </summary>
        /// <returns>Kết nối với DB</returns>
        public IDbConnection CreateConnection();
    }
}
