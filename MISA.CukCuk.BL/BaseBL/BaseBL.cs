using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Testamis.DL;
using MISA.Testamis.API.Enums.DTO;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MISA.Testamis.Common.Enums.DTO;
using MISA.Testamis.Common;
using System.Text.RegularExpressions;
using System.ComponentModel;
using MISA.Testamis.Common.Entitis;

namespace MISA.Testamis.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }
        #endregion

        #region Method
        /// <summary>
        /// Hàm validate dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng được validate</param>
        /// <param name="isInsert">Kiểm tra thao tác thực hiện là thêm hay sửa</param>
        /// <returns>
        /// Danh sách lỗi
        /// </returns>
        /// CreatedBy: Bien (12/2/2023)

        protected virtual Dictionary<string, string> Validate(T? entity, bool isInsert = true)
        {
            var result = ValidateCustom(entity, isInsert);

            var properties = typeof(T).GetProperties();

            // Kiểm tra xem property nào có attribute là Requidred
            var validateFailures = new Dictionary<string, string>();
            foreach (var property in properties)
            {
                var propertyName = property.Name;
                var propertyValue = property.GetValue(entity);

                // Kiểm tra lỗi để trống dữ liệu
                var requiredAttribute = (RequiredAttribute?)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();

                if (requiredAttribute != null && string.IsNullOrEmpty(propertyValue?.ToString()))
                {
                    validateFailures.Add(propertyName, requiredAttribute.ErrorMessage);
                }

                // Kiểu tra lỗi không đúng định dạng chuổi regex
                var displayAtrribute = (DisplayNameAttribute?)property.GetCustomAttributes(typeof(DisplayNameAttribute), false).FirstOrDefault();
                var regularAttribute = (RegularExpressionAttribute?)property.GetCustomAttributes(typeof(RegularExpressionAttribute), false).FirstOrDefault();
                if (regularAttribute != null)
                {
                    if (propertyValue != null && propertyValue != "")
                    {
                        var pattern = regularAttribute.Pattern;
                        var isMatch = Regex.Match(propertyValue.ToString(), pattern, RegexOptions.IgnoreCase);
                        if (!isMatch.Success)
                        {
                            validateFailures.Add(propertyName, displayAtrribute.DisplayName + Common.Resource.ErrorMsg_CustomValidate);
                        }
                    }
                }
            } 
            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    validateFailures.Add(item.Key, item.Value);
                }
            }

            return validateFailures;
        }

        /// <summary>
        /// Hàm kiểm tra những lỗi riêng
        /// </summary>
        /// <param name="entity">Đối tượng muốn kiểm tra dữ liệu</param>
        /// <param name="isInsert">Kiểm tra thao tác thực hiện là thêm hay sửa</param>
        /// <returns>Danh sách lỗi</returns>
        /// CreatedBy: Bien (05/04/2023)
        protected virtual Dictionary<string, string> ValidateCustom(T? entity, bool isInsert = true)
        {
            var validateFailures = new Dictionary<string, string>();
       
            return validateFailures;
        }

        /// <summary>
        /// API tìm kiếm theo tên và mã
        /// </summary>
        /// <param name="filter">Tên và mã đối tượng cần tìm kiếm</param>
        /// <param name="pageSize">Kích thước trong 1 bảng ghi</param>
        /// <param name="pageNumber">Số thứ tự hiện tại của trang</param>
        /// <returns>
        /// Danh sách đối tượng phân trang
        /// </returns>
        /// CreatedBy: Bien (17/1/2023)
        public PagingResult Filter(
            [FromQuery] int offset = 1,
            [FromQuery] int limit = 20,
             [FromQuery] string? filter = null
            )
        {
            // Gọi vào hàm tìm kiếm trong BaseDL
            var data = _baseDL.Filter(offset, limit, filter);

            if (data != null)
            {
                return data;
            }
            return data;
        }
       
        /// <summary>
        /// Lấy 1 bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID của bản ghi muốn lấy</param>
        /// <returns>Đối tượng có id được truyền vào</returns>
        /// CreatedBy: Bien (6/2/2023)
        //public ServiceResult GetById(Guid entityId)
        //{
        //    // Gọi vào hàm lấy theo ID trong BaseDL
        //    var data = _baseDL.GetById(entityId);
        //    if (data != null)
        //    {
        //        return new ServiceResult
        //        {
        //            IsSuccess = true,
        //            ErrorCode = Common.Enums.ErrorCode.NoError,
        //            Data = data
        //        };
        //    }
        //    else
        //    {
        //        return new ServiceResult
        //        {
        //            IsSuccess = false,
        //            ErrorCode = Common.Enums.ErrorCode.GetMethodByDLError,
        //        };
        //    }

        //}

        /// <summary>
        /// Lấy danh sách bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID của danh sách muốn lấy</param>
        /// <returns>Danh sách đối tượng</returns>
        /// CreatedBy: Bien (01/05/2023)
        public ServiceResult GetById(Guid id)
        {
            // Gọi vào hàm lấy theo ID trong BaseDL
            var data = _baseDL.GetById(id);
            if (data != null)
            {
                return new ServiceResult
                {
                    IsSuccess = true,
                    ErrorCode = Common.Enums.ErrorCode.NoError,
                    Data = data
                };
            }
            else
            {
                return new ServiceResult
                {
                    IsSuccess = false,
                    ErrorCode = Common.Enums.ErrorCode.GetMethodByDLError,
                };
            }
        }

        /// <summary>
        /// Hàm thêm mới
        /// </summary>
        /// <param name="entity">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>
        /// IsSuccess = true. Nếu thêm thành công 
        /// IsSuccess = false. Nếu thêm thất bại
        /// </returns>
        /// CretaedBy: Bien (6/2/2023)
        public virtual ServiceResult Insert(T entity)
        {
            // Khai báo biến nhận kết quả trả về
            ServiceResult serviceResult = new ServiceResult();

            // Validate dữ liệu 
            var validateResults = Validate(entity);

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
                var numberOfAffectedRows = _baseDL.Insert(entity);

                if (numberOfAffectedRows > 0)
                {
                    serviceResult.IsSuccess = true;
                    serviceResult.ErrorCode = Common.Enums.ErrorCode.NoError;
                    serviceResult.Data = entity;
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
        /// API sửa
        /// </summary>
        /// <param name="entityId">Id của đối tượng muốn sửa</param>
        /// <param name="entity">Đối tượng mang giá trị đã được thay đổi</param>
        /// <returns> 
        /// IsSuccess = true. Nếu sửa thành công 
        /// IsSuccess = false. Nếu sửa thất bại
        /// </returns>
        /// CreatedBy: Bien (17/1/2023)
        public virtual ServiceResult Update([FromRoute] Guid entityId, [FromBody] T entity)
        {
            // Khai báo biến nhận kết quả trả về
            ServiceResult serviceResult = new ServiceResult();

            // Validate dữ liệu 
            var validateResults = Validate(entity,false);

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
                var numberOfAffectedRows = _baseDL.Update(entityId, entity);

                if (numberOfAffectedRows > 0)
                {
                    serviceResult.IsSuccess = true;
                }
                else
                {
                    serviceResult.IsSuccess = false;
                    serviceResult.ErrorCode = Common.Enums.ErrorCode.GetMethodByDLError;
                    serviceResult.Message = Resource.ErrorMsg_UpdateByDL;
                }
            }

            return serviceResult;
        }

        /// <summary>
        /// API Xóa
        /// </summary>
        /// <param name="entityId">Id của đối tượng muốn xóa</param>
        /// <returns>
        /// Kết quả của hành động xóa 
        /// IsSuccess = true. Nếu xóa thành công 
        /// IsSuccess = false. Nếu xóa thất bại
        /// </returns>
        /// CreatedBy: Bien (12/2/2023)
        public ServiceResult Delete([FromRoute] Guid entityId)
        {
            var serviceResult = new ServiceResult();

            // Xử lý kết quả trả về
            int numberOfAffectedRows = _baseDL.Delete(entityId);

            if (numberOfAffectedRows > 0)
            {
                serviceResult.IsSuccess = true;
            }
            else
            {
                serviceResult.IsSuccess = false;
            }
            return serviceResult;
        }

        /// <summary>
        /// API xuất dữ liệu sang file Excel
        /// </summary>
        /// <returns>File Excel chứa dữ liệu</returns>
        /// CreatedBy: Bien (05/04/2023)
        public virtual MemoryStream ExportToExcel(string? filter)
        {
            return new MemoryStream();
        }

        /// <summary>
        /// API xóa nhiều đối tượng
        /// </summary>
        /// <param name="entityIds">Danh sách ID đối tượng muốn xóa</param>
        /// <returns>
        /// IsSuccess: True nếu xóa thành thông
        /// IsSuccess: False nếu xóa thất bại
        /// </returns>
        public ServiceResult Deletes(List<Guid> entityIds)
        {
            var serviceResult = new ServiceResult();
            
           int numberOfAffectedRows = _baseDL.Deletes(entityIds);
            
            if (numberOfAffectedRows > 0)
            {
                serviceResult.IsSuccess = true;
            }
            else
            {
                serviceResult.IsSuccess = false;
            }
            return serviceResult;
        }
        #endregion

    }
}
