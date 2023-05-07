using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.API.Enums.DTO;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MISA.Testamis.DL;
using MISA.Testamis.Common.Enums.DTO;

namespace MISA.Testamis.BL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// API Lấy tất cả danh sách đối tượng
        /// </summary>
        /// <returns>Danh sách đối tượng</returns>
        /// CreatedBy: Bien (27/04/2023)
        public ServiceResult GetAll();
        /// <summary>
        /// API tìm kiếm theo tên và mã
        /// </summary>
        /// <param name="filter">Tên và mã đối tượng cần tìm kiếm</param>
        /// <param name="pageSize">Kích thước trong 1 bảng ghi</param>
        /// <param name="pageNumber">Số thứ tự hiện tại của trang</param>
        /// <returns>Danh sách đối tượng phân trang</returns>
        /// CreatedBy: Bien (17/1/2023)
        public PagingResult Filter(
           [FromQuery] int offset = 1,
            [FromQuery] int limit = 20,
             [FromQuery] string? filter = null,
             int? statusFilter = 0,
             string? misaCode = null
           );

        /// <summary>
        /// Lấy 1 bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID của bản ghi muốn lấy</param>
        /// <returns>Đối tượng muốn lấy</returns>
        /// CreatedBy: Bien (6/2/2023)
        //public ServiceResult GetById(Guid entityId);

        /// <summary>
        /// Lấy danh sách bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID của danh sách muốn lấy</param>
        /// <returns>Danh sách đối tượng</returns>
        /// CreatedBy: Bien (01/05/2023)
        public ServiceResult GetById(Guid id);

        /// <summary>
        /// Hàm thêm mới nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>
        /// IsSuccess = true. Nếu thêm thành công 
        /// IsSuccess = false. Nếu thêm thất bại
        /// </returns>
        /// CretaedBy: Bien (6/2/2023)
        public ServiceResult Insert(T entity);

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
        public ServiceResult Update([FromRoute] Guid entityId, [FromBody] T entity);

        /// <summary>
        /// API Xóa
        /// </summary>
        /// <param name="entityId">Id đối tượng muốn xóa</param>
        /// <returns>
        /// IsSuccess = true. Nếu sửa thành công 
        /// IsSuccess = false. Nếu sửa thất bại
        /// </returns>
        public ServiceResult Delete([FromRoute] Guid entityId);

        /// <summary>
        /// API xuất dữ liệu sang file Excel
        /// </summary>
        /// <returns>File Excel chứa dữ liệu</returns>
        /// CreatedBy: Bien (05/04/2023)
        public MemoryStream ExportToExcel(string? filter);

        /// <summary>
        /// API xóa nhiều bản ghi
        /// </summary>
        /// <param name="entityIds">Danh sách Id muốn xóa</param>
        /// <returns>
        /// IsSuccess = true. Nếu xóa thành công 
        /// IsSuccess = false. Nếu xóa thất bại
        ///  </returns>
        public ServiceResult Deletes(List<Guid> entityIds);
    }
}
