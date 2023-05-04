using Dapper;
using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.API.Enums.DTO;
using MISA.Testamis.Common;
using MISA.Testamis.Common.Constants;
using MISA.Testamis.Common.Database;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MISA.Testamis.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {

        #region Field

        protected IDatabase _database;

        #endregion

        #region Constructor

        public BaseDL(IDatabase database)
        {
            _database = database;
        }

        #endregion

        #region Method

        /// <summary>
        /// API tìm kiếm theo tên và mã
        /// </summary>
        /// <param name="filter">Tên và mã đối tượng cần tìm kiếm</param>
        /// <param name="limit">Kích thước trong 1 bảng ghi</param>
        /// <param name="offset">Vị trí muốn lấy</param>
        /// <returns>Danh sách đối tượng phân trang</returns>
        /// CreatedBy: Bien (17/1/2023)
        public virtual PagingResult Filter(
            [FromQuery] int offset = 1,
            [FromQuery] int limit = 20,
            [FromQuery] string? filter = null
            )
        {
            // Khai tên class truyền vào
            var entityName = typeof(T).Name;

            offset = (offset - 1) * limit;

            // Chuẩn bị tên stored procedure
            string storedProdureName = string.Format(ProcedureName.PROC_GET_BY_FILTER, entityName);

            // Chuẩn bị thàm số đầu vào cho stored
            var parameters = new DynamicParameters();
            parameters.Add($"p_{entityName}Filter", filter);
            parameters.Add("p_LiMit", limit);
            parameters.Add("p_OffSet", offset);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                var multi = connection.QueryMultiple(storedProdureName, parameters, commandType: CommandType.StoredProcedure);
                var dataList = multi.Read<T>().ToList();
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
        /// Lấy 1 bản ghi theo ID
        /// </summary>
        /// <param name="enitityId">ID của bản ghi muốn lấy</param>
        /// <returns>Đối tượng muốn lấy</returns>
        /// CreatedBy: Bien (6/2/2023)
        //public virtual T GetById(Guid entityId)
        //{
        //    // Khai tên class truyền vào
        //    var entityName = typeof(T).Name;

        //    // Chuẩn bị tên stored procecure
        //    string storedProdureName = String.Format(ProcedureName.PROC_GET_BY_ID, entityName);

        //    // Chuẩn bị tham số đầu vào cho stored
        //    var parameters = new DynamicParameters();
        //    parameters.Add($"p_{entityName}Id", entityId);

        //    // Gọi vào DB
        //    using (var connection = _database.CreateConnection())
        //    {
        //        connection.Open();

        //        var data = connection.QueryFirstOrDefault<T>(storedProdureName, parameters, commandType: CommandType.StoredProcedure);
        //        return data;
        //    }
        //}

        /// <summary>
        /// Lấy danh sách bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID của danh sách muốn lấy</param>
        /// <returns>Danh sách đối tượng</returns>
        /// CreatedBy: Bien (01/05/2023)
        public virtual List<T> GetById(Guid id)
        {
            var entities = new List<T>();

            // Khai tên class truyền vào
            var entityName = typeof(T).Name;

            // Chuẩn bị tên stored procecure
            string storedProdureName = String.Format(ProcedureName.PROC_GET_BY_ID, entityName);

            // Chuẩn bị tham số đầu vào cho stored
            var parameters = new DynamicParameters();
            parameters.Add($"p_{entityName}Id", id);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                var multi = connection.QueryMultiple(storedProdureName, parameters, commandType: CommandType.StoredProcedure);
                entities = multi.Read<T>().ToList();

            }
            return entities;
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
        public virtual int Insert(T entity)
        {
            // Khai tên class truyền vào
            var entityName = typeof(T).Name;

            // Chuẩn bị tên stored procedure
            string storedProdureName = String.Format(ProcedureName.PROC_INSERT, entityName);
            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                int numberOfAffectedRows = connection.Execute(storedProdureName, entity, commandType: CommandType.StoredProcedure);

                return numberOfAffectedRows;
            }
        }

        /// <summary>
        /// API sửa
        /// </summary>
        /// <param name="entityId">Id của đối tượng muốn sửa</param>
        /// <param name="entity">Đối tượng mang giá trị đã được thay đổi</param>
        /// <returns> 
        /// 1. Sửa thành công, 
        /// 0. Sửa thất bại
        /// </returns>
        /// CreatedBy: Bien (17/1/2023)
        public int Update([FromRoute] Guid entityId, [FromBody] T entity)
        {
            // Khai tên class truyền vào
            var entityName = typeof(T).Name;

            // Lấy toàn bộ property của T
            var properties = typeof(T).GetProperties();

            // Chuẩn bị tên stored procedure
            string storedProdureName = string.Format(ProcedureName.PROC_UPDATE, entityName);

            //Chuẩn bị thàm số đầu vào cho stored
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                parameters.Add($"p_{property.Name}", property.GetValue(entity));
            }
            parameters.Add($"p_{entityName}Id", entityId);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                int numberOfAffectedRows = connection.Execute(storedProdureName, parameters, commandType: CommandType.StoredProcedure);

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
        /// API Xóa
        /// </summary>
        /// <param name="entityId">Id của đối tượng muốn xóa</param>
        /// <returns>
        /// 1. Nếu xóa thành công,
        /// 0. Nếu xóa thất bại
        /// </returns>
        /// CreatedBy: Bien (05/04/2023)
        public int Delete([FromRoute] Guid entityId)
        {
            var numberOfAffectedRows = 0;
            // Khai tên class truyền vào
            var entityName = typeof(T).Name;

            // Chuẩn bị tên stored procecure
            string storedProdureName = string.Format(ProcedureName.PROC_DELETE, entityName);

            // Chuẩn bị tham số đầu vào cho stored
            var parameters = new DynamicParameters();
            parameters.Add($"p_{entityName}Id", entityId);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                connection.Open();

                numberOfAffectedRows = connection.Execute(storedProdureName, parameters, commandType: CommandType.StoredProcedure);

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
        /// API xuất dữ liệu sang file Excel
        /// </summary>
        /// <returns>File Excel chứa dữ liệu</returns>
        /// CreatedBy: Bien (05/04/2023)
        public ServiceResult ExportToExcel(string? filter)
        {
            // Khai tên class truyền vào
            var entityName = typeof(T).Name;

            // Chuẩn bị tên stored procecure
            string storedProdureName = string.Format(ProcedureName.PROC_EXPORT, entityName);
            var parameters = new DynamicParameters();
            parameters.Add($"p_{entityName}Filter", filter);

            // Gọi vào DB
            using (var connection = _database.CreateConnection())
            {
                // Thực hiện lệnh gọi vào DB
                var multi = connection.QueryMultiple(storedProdureName, parameters, commandType: CommandType.StoredProcedure);

                // Xử lý dữ liệu trả về
                var records = multi.Read<T>().ToList();
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
        /// Hàm xóa nhiều bản ghi
        /// </summary>
        /// <param name="entityIds">Danh sách id bản ghi muốn xóa</param>
        /// <returns>
        /// >0: Nếu xóa thành công
        /// 0: Nếu xóa thất bại
        /// </returns>
        /// CreatedBy: Bien (24/02/2023)
        public int Deletes(List<Guid> entityIds)
        {
            var numberOfAffectedRows = 0;

            // Khai tên class truyền vào
            var entityName = typeof(T).Name;

            var sizeList = entityIds.Count();
            // Khai báo tên stored procedure z
            string storedProcedureName = String.Format(Common.Constants.ProcedureName.PROC_DELETES, entityName);

            var entityIdList = "";
            // Chuẩn bị tham số đầu vào cho procedure
            var parameters = new DynamicParameters();
            entityIdList = $"{String.Join(",", entityIds)}";

            parameters.Add($"p_{entityName}Ids", entityIdList);
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
        #endregion
    }
}
