using MISA.Testamis.Common.Entitis;
using MISA.Testamis.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Testamis.DL;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data;
using MISA.Testamis.Common.Enums.DTO;
using MISA.Testamis.Common;
using Microsoft.AspNetCore.Http;
using MISA.Testamis.Common.Enums;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;
using System.IO;

namespace MISA.Testamis.BL
{
    public class EmployeeBL : BaseBL<Employee>, IEmployeeBL
    {
        #region Field

        private IEmployeeDL _employeeDL;

        #endregion

        #region Constractor

        public EmployeeBL(IEmployeeDL employeeDL) : base(employeeDL)
        {
            _employeeDL = employeeDL;
        }
        #endregion

        #region Method
        /// <summary>
        /// API Lấy tất cả danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: Bien (27/04/2023)
        public ServiceResult GetEmployees()
        {
            var serviceResult = new ServiceResult();

            var data = _employeeDL.GetEmployees();

            if(data != null)
            {
                serviceResult.Data = data;
                serviceResult.IsSuccess = true;
                serviceResult.ErrorCode = ErrorCode.NoError;
            }
            else
            {
                serviceResult.IsSuccess = false;
                serviceResult.ErrorCode = ErrorCode.UnknownError;
            }

            return serviceResult;
        }
        /// <summary>
        /// Hàm thêm mới nhân viên
        /// </summary>
        /// <param name="employee">Nhân viên cần thêm mới</param>
        /// <returns>
        /// IsSuccess = true. Nếu thêm thành công 
        /// IsSuccess = false. Nếu thêm thất bại
        /// </returns>
        /// CretaedBy: Bien (6/2/2023)
        public override ServiceResult Insert(Employee employee)
        {
            // Khai báo biến nhận kết quả trả về
            ServiceResult serviceResult = new ServiceResult();

            // Validate dữ liệu 
            var validateResults = Validate(employee);

            if (validateResults.Count > 0)
            {
                serviceResult.IsSuccess = false;
                serviceResult.ErrorCode = Common.Enums.ErrorCode.ValidateError;
                serviceResult.Data = validateResults.ToList();
                serviceResult.Message = Resource.ErrorMsg_Validate;
            }
            else
            {
                // Gọi vào DB
                var numberOfAffectedRows = _employeeDL.Insert(employee);

                if (numberOfAffectedRows > 0)
                {
                    serviceResult.IsSuccess = true;
                    serviceResult.ErrorCode = Common.Enums.ErrorCode.NoError;
                }
                else
                {
                    serviceResult.IsSuccess = false;
                    serviceResult.ErrorCode = Common.Enums.ErrorCode.GetMethodByDLError;
                    serviceResult.Message = Resource.ErrorMsg_InsertByDL;
                }
            }
            return serviceResult;
        }

        /// <summary>
        /// Hàm kiểm tra những lỗi dữ liệu đầu vào lớp Employee
        /// </summary>
        /// <param name="employee">Nhân viên muốn sửa</param>
        /// <returns>Chuỗi lỗi hoặc null</returns>
        /// CreatedBy: Bien (05/04/2023)
        protected override Dictionary<string, string> ValidateCustom(Employee employee, bool isInsert = true)
        {
            var validateFailures = new Dictionary<string, string>();

            if (employee.DateOfBirth != null && employee.IdentityDate != null)
            {
                var dateNow = DateTime.Now;
                if (employee.DateOfBirth > dateNow)
                {
                    validateFailures.Add("DateOfBirth", Common.Resource.ErrorMsg_DataOfBirth);
                }

                if (employee.IdentityDate > dateNow)
                {
                    validateFailures.Add("IdentityDate", Common.Resource.ErrorMsg_IdentityDate);
                }
            }

            // Check trùng mã nhân viên
            var resultCheck = CheckEmployeeCode(employee, isInsert);
            if (resultCheck)
            {
                validateFailures.Add("EmployeeCode", Common.Resource.ErrorMsg_Check);
            }

            return validateFailures;
        }

        /// <summary>
        /// API sinh mới mã
        /// </summary>
        /// <returns>Mã mới được tạo</returns>
        /// CreatedBy: Bien (17/1/2023)
        public ServiceResult NewEmployeeCode()
        {
            // Tính toán mã nhân viên mới
            var bewEmployeeCode = _employeeDL.GetMaxCode();

            return new ServiceResult
            {
                IsSuccess = true,
                Data = bewEmployeeCode
            };
        }

        /// <summary>
        /// Hàm kiểm tra trùng mã nhân viên
        /// </summary>
        /// <returns>
        /// True: Nếu mã nhân viên đã tồn tại
        /// False: Nếu mã nhân viên hợp lệ
        /// </returns>
        /// CreatedBy: Bien (12/2/2023)
        public bool CheckEmployeeCode(Employee employee, bool isInsert = true)
        {

            if (!string.IsNullOrEmpty(employee.EmployeeCode))
            {
                // Lấy id nhân viên theo mã 
                Guid id = _employeeDL.CheckEmployeeCode(employee.EmployeeCode.Trim());

                if ((isInsert && id != Guid.Empty) || (!isInsert && id != Guid.Empty && id != employee.EmployeeId))
                    return true;
            }
            return false;
        }

    }

    #endregion
}
