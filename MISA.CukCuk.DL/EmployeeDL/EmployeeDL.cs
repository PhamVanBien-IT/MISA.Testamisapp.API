using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.API.Enums.DTO;
using MISA.Testamis.Common.Constants;
using MISA.Testamis.Common.Database;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.DL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.Testamis.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {

        public EmployeeDL(IDatabase database) : base(database)
        {
        }

        /// <summary>
        /// API Lấy tất cả danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: Bien (27/04/2023)
        public List<Employee> GetEmployees()
        {
            var dataStore = DataStore.Instance;

            // Khai tên class truyền vào
            var employeeName = typeof(Employee).Name;

            // Chuẩn bị tên stored procedure
            string storedProdureName = string.Format(ProcedureName.PROC_GET_ALL, employeeName);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                var multi = connection.QueryMultiple(storedProdureName, commandType: CommandType.StoredProcedure);
                var dataList = multi.Read<Employee>().ToList();

                for (int i = 0; i < dataList.Count; i++)
                {
                    dataStore.Set<Employee>(dataList[i].EmployeeId.ToString(), dataList[i]);
                }

                if(dataList.Count > 0)
                {
                    return dataList;
                }

                return null;
            }
        }
        /// <summary>
        /// API lấy danh sách nhân viên theo phân trang
        /// </summary>
        /// <param name="offset">Vị trí muốn lấy</param>
        /// <param name="limit">Số bản ghi muốn lấy</param>
        /// <param name="filter">Tìm kiếm theo tên và mã</param>
        /// <returns>Danh sách nhân viên</returns>
        public override PagingResult Filter(
           [FromQuery] int offset = 1,
           [FromQuery] int limit = 20,
           [FromQuery] string? filter = null
           )
        {
            // Khai tên class truyền vào
            var employeeName = typeof(Employee).Name;

            offset = (offset - 1) * limit;

            // Chuẩn bị tên stored procedure
            string storedProdureName = string.Format(ProcedureName.PROC_GET_BY_FILTER, employeeName);

            // Chuẩn bị thàm số đầu vào cho stored
            var parameters = new DynamicParameters();
            parameters.Add($"p_{employeeName}Filter", filter);
            parameters.Add("p_LiMit", limit);
            parameters.Add("p_OffSet", offset);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                var multi = connection.QueryMultiple(storedProdureName, parameters, commandType: CommandType.StoredProcedure);
                var dataList = multi.Read<Employee>().ToList();
                int sumPage = multi.ReadFirstOrDefault<int>();

                var data = new PagingResult
                {
                    Data = dataList,
                    TotalRecord = sumPage,
                    TotalPage = (sumPage / limit) == 0 ? 1 : sumPage / limit
                };
                return data;
            }
        }
        /// <summary>
        /// API sinh mới mã
        /// </summary>
        /// <returns>Mã mới được tạo</returns>
        /// CreatedBy: Bien (17/1/2023)
        public string GetMaxCode()
        {
            // Tính toán mã nhân viên mới
            string storedProdureName = $"Proc_Employee_GetMaxCode";

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                var multi = connection.QueryMultiple(storedProdureName, commandType: CommandType.StoredProcedure);
                var maxEmployeeCode = multi.ReadFirstOrDefault<string>();
                return maxEmployeeCode;
            }
        }

        /// <summary>
        /// API kiểm tra trùng mã nhân viên
        /// </summary>
        /// <returns>
        /// Id: Nếu nhân viên đã tồn tại
        /// Null: Nếu nhân viên hợp lệ
        /// </returns>
        /// CreatedBy: Bien (24/03/2023)
        public Guid CheckEmployeeCode(string employeeCode)
        {
            // Chuẩn bị tên stored procedure
            string storedProdureName = "Proc_Employee_GetByEmployeeCode";

            //Chuẩn bị thàm số đầu vào cho stored
            var parameters = new DynamicParameters();
            parameters.Add("p_EmployeeCode", employeeCode);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                var result = connection.QueryFirstOrDefault<Guid>(storedProdureName, parameters, commandType: CommandType.StoredProcedure);
                             
                return result;
            }
           
        }

    }
}
