using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.API.Enums.DTO;
using MISA.Testamis.Common.Constants;
using MISA.Testamis.Common.Database;
using MISA.Testamis.Common.Entitis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace MISA.Testamis.DL
{
    public class MissionallowanceDL : BaseDL<Missionallowance>, IMissionallowanceDL
    {
        public MissionallowanceDL(IDatabase database) : base(database)
        {
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
            var dataStore = DataStore.Instance;

            // Khai tên class truyền vào
            var missionallowanceName = typeof(Missionallowance).Name;

            offset = (offset - 1) * limit;

            // Chuẩn bị tên stored procedure
            string storedProdureName = string.Format(ProcedureName.PROC_GET_BY_FILTER, missionallowanceName);

            // Chuẩn bị thàm số đầu vào cho stored
            var parameters = new DynamicParameters();
            parameters.Add($"p_MissionallowanceFilter", filter);
            parameters.Add("p_LiMit", limit);
            parameters.Add("p_OffSet", offset);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                var multi = connection.QueryMultiple(storedProdureName, parameters, commandType: CommandType.StoredProcedure);
                var dataList = multi.Read<Missionallowance>().ToList();
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
        /// API Thêm dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng muốn thêm</param>
        /// <returns>
        /// 1: Nếu thêm thành công
        /// 0: Nếu thêm thất bại
        /// </returns>
        /// CreatedBy: Bien (6/2/2023)
        public override int Insert(Missionallowance missionallowance)
        {
            var numberOfAffectedRows = 0;

            // Khai tên class truyền vào
            var entityName = typeof(Missionallowance).Name;

            // Chuẩn bị tên stored procedure
            string storedProdureName = String.Format(ProcedureName.PROC_INSERT, entityName);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                numberOfAffectedRows = connection.Execute(storedProdureName, missionallowance, commandType: CommandType.StoredProcedure);


                if (numberOfAffectedRows > 0)
                {
                    var missionallowanceId = GetMissionallowanceId();
                    var numberInsertEmployeeMissionallowance = InsertEmployeeMissionallowance(missionallowance.EmployeeMissionallowances, missionallowanceId);
                }

                return numberOfAffectedRows;

            }


        }

        /// <summary>
        /// Hàm lất id đơn công tác tương ứng
        /// </summary>
        /// <returns>Id đơn công tác chứa danh sách nhân viên công tác</returns>
        /// CreatedBy: Bien (30/04/2023)
        private Guid GetMissionallowanceId()
        {
            // Chuẩn bị tên stored procecure
            string storedProdureName = "p_Missionallowance_GetId";

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                var data = connection.QueryFirstOrDefault<Guid>(storedProdureName, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        /// <summary>
        /// Hàm thêm mới danh sách nhân viên công tác
        /// </summary>
        /// <param name="employeeIds">Danh sách id nhân viên công tác</param>
        /// <param name="missionallowanceId">Id đơn công tác</param>
        /// <returns>
        /// >0: Nếu thêm thành công
        /// 0: Nếu thêm thất bạt
        /// </returns>
        /// CreatedBy: Bien (30/04/2023)
        private int InsertEmployeeMissionallowance(string employeeIds, Guid missionallowanceId)
        {
            int numberOfAffectedRows = 0;
            // Khai tên class truyền vào
            var entityName = typeof(EmployeeMissionallowance).Name;

            // Chuẩn bị tên stored procedure
            string storedProdureName = String.Format(ProcedureName.PROC_INSERT, entityName);

            // Chuẩn bị tham số đầu vào cho stored
            var parameters = new DynamicParameters();
            parameters.Add($"p_EmployeeIds", employeeIds);
            parameters.Add($"p_MissionallowanceId", missionallowanceId);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                numberOfAffectedRows = connection.Execute(storedProdureName, parameters, commandType: CommandType.StoredProcedure);

            }

            return numberOfAffectedRows;

        }
    }
}
