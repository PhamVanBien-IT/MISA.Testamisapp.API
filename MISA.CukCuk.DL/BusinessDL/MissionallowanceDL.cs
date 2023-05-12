using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.API.Enums.DTO;
using MISA.Testamis.Common.Constants;
using MISA.Testamis.Common.Database;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
           [FromQuery] string? filter = null,
           int? statusFilter = 0,
           string? misaCode = null
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
            parameters.Add("p_StatusFilter", statusFilter);
            parameters.Add("p_MisaCodeFilter", misaCode);



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
            var numberInsertEmployeeMissionallowance = 0;

            // Khai tên class truyền vào
            var entityName = typeof(Missionallowance).Name;

            // Chuẩn bị tên stored procedure
            string storedProdureName = String.Format(ProcedureName.PROC_INSERT, entityName);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                var numberOfAffectedRows = connection.Execute(storedProdureName, missionallowance, commandType: CommandType.StoredProcedure);

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        if (numberOfAffectedRows > 0)
                        {
                            var missionallowanceId = GetMissionallowanceId();
                            numberInsertEmployeeMissionallowance = InsertEmployeeMissionallowance(missionallowance.EmployeeMissionallowances, missionallowanceId);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                    }
                }
               
                return numberInsertEmployeeMissionallowance;

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
            string storedProdureName = "Proc_Missionallowance_GetId";

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

        /// <summary>
        /// API sửa đơn công tác
        /// </summary>
        /// <param name="missionallowanceId">Id của đơn công tác muốn sửa</param>
        /// <param name="missionallowance">Đơn công tác mang giá trị đã được thay đổi</param>
        /// <returns> 
        /// 1. Sửa thành công, 
        /// 0. Sửa thất bại
        /// </returns>
        /// CreatedBy: Bien (17/1/2023)
        public override int Update([FromRoute] Guid missionallowanceId, [FromBody] Missionallowance missionallowance)
        {
            var numberOfAffectedRows = 0;
            // Khai tên class truyền vào
            var entityName = typeof(Missionallowance).Name;

            // Lấy toàn bộ property của T
            var properties = typeof(Missionallowance).GetProperties();

            // Chuẩn bị tên stored procedure
            string storedProdureName = string.Format(ProcedureName.PROC_UPDATE, entityName);

            //Chuẩn bị thàm số đầu vào cho stored
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                parameters.Add($"p_{property.Name}", property.GetValue(missionallowance));
            }
            parameters.Add($"p_{entityName}Id", missionallowanceId);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int numberDeleteEmployeeMissionallowance = DeleteEmployeeMissionallowance(missionallowanceId);

                        var numberInsertEmployeeMissionallowance = InsertEmployeeMissionallowance(missionallowance.EmployeeMissionallowances, missionallowanceId);

                        numberOfAffectedRows = connection.Execute(storedProdureName, parameters, transaction, commandType: CommandType.StoredProcedure);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                    }
                }

              
               
                // Xử lý kết quả trả về
                if (numberOfAffectedRows > 0)
                {
                    return numberOfAffectedRows;
                }
                else
                {
                    return numberOfAffectedRows;
                }
            }
        }

        /// <summary>
        /// Hàm xóa danh sách nhân viên đi công tác
        /// </summary>
        /// <param name="missionallowanceId">Id đơn công tác</param>
        /// <returns>
        /// > 0: Nếu xóa thành công
        /// 0: Nếu xóa thất bại
        /// </returns>
        /// CreatedBy: Bien (05/05/2023)
        private int DeleteEmployeeMissionallowance(Guid missionallowanceId)
        {
            int numberOfAffectedRows = 0;

            // Khai tên class truyền vào
            var entityName = typeof(EmployeeMissionallowance).Name;

            // Chuẩn bị tên stored procedure
            string storedProdureName = String.Format(ProcedureName.PROC_DELETE, entityName);

            // Chuẩn bị tham số đầu vào cho stored

            var parameters = new DynamicParameters();
            parameters.Add($"p_EmployeeMissionallowanceId", missionallowanceId);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                numberOfAffectedRows = connection.Execute(storedProdureName, parameters, commandType: CommandType.StoredProcedure);

            }

            return numberOfAffectedRows;
        }

        /// <summary>
        /// API Cập nhật trạng thái đơn công tác
        /// </summary>
        /// <param name="missionallowanceIds">Danh sách đơn muốn cập nhật</param>
        /// <param name="status">Gía trị trạng thái muốn cập nhập</param>
        /// <returns>
        /// >0 : Cập nhật thành cồn
        /// 0: Cập nhật thát bại
        /// </returns>
        /// CreatedBy: Bien (10/05/2023)
        public int UpdateMissionallowanceStatus(List<Guid> missionallowanceIds, int status)
        {
            var numberOfAffectedRows = 0;

            // Khai tên class truyền vào
            var entityName = typeof(Missionallowance).Name;

            var sizeList = missionallowanceIds.Count();
            // Khai báo tên stored procedure z
            string storedProcedureName = String.Format(Common.Constants.ProcedureName.PROC_UPDATE_STATUS, entityName);

            var entityIdList = "";
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            entityIdList = $"{String.Join(",", missionallowanceIds)}";

            parameters.Add($"p_MisionallowanceIds", entityIdList);
            parameters.Add($"p_Status", status);
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        numberOfAffectedRows = connection.Execute(storedProcedureName, parameters, transaction, commandType: CommandType.StoredProcedure);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        transaction.Rollback();
                    }
                }
            }
            // Xử lý kết quả trả về
            if (numberOfAffectedRows > 0)
            {
                return numberOfAffectedRows;
            }
            else
            {
                return numberOfAffectedRows;
            }
        }

        /// <summary>
        /// API xuất khẩu danh sách đơn đã chọn
        /// </summary>
        /// <param name="missionallowanceIds">Danh sách id đơn đã chọn</param>
        /// <returns>Danh sách đơn</returns>
        /// CreatedBy: Bien (10/05/2023)
        public ServiceResult ExportMissionnallowanceList(ExportListSelect dataSelected)
        {
            // Khai tên class truyền vào
            var entityName = typeof(Missionallowance).Name;

            //var sizeList = missionallowanceIds.Count();
            // Khai báo tên stored procedure z
            string storedProcedureName = "Proc_Missionallowance_ExportListSelected";

            var entityIdList = "";
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            entityIdList = $"{String.Join(",", dataSelected.ids)}";

            parameters.Add($"p_MisionallowanceIds", entityIdList);
            using (var connection = _database.CreateConnection())
            {
                // Thực hiện lệnh gọi vào DB
                var multi = connection.QueryMultiple(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                var records = multi.Read<Missionallowance>().ToList();
                var totalCount = multi.Read<int>().Single();

                // Xử lý kết quả trả về
                return new ServiceResult
                {
                    IsSuccess = true,
                    ErrorCode = Common.Enums.ErrorCode.NoError,
                    Data = records,
                };
            }
    }

        /// <summary>
        /// API lấy danh sách bản ghi đã tạo ngày hôm nay
        /// </summary>
        /// <returns>
        /// Danh sách đơn công tác đã tạo hôm nay
        /// </returns>
        /// CreatedBy: Bien (12/05/2023
        public List<Guid> GetAddMissionallowanceToDay()
        {

            // Chuẩn bị tên stored procedure
            string storedProdureName = "Proc_Missionallwance_GetNow";

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                var multi = connection.QueryMultiple(storedProdureName, commandType: CommandType.StoredProcedure);
                var dataList = multi.Read<Guid>().ToList();

                return dataList;
            }
        }
    }
}
