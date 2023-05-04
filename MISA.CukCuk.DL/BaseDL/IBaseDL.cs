using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.API.Enums.DTO;
using MISA.Testamis.Common.Enums.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// API tìm kiếm theo tên và mã
        /// </summary>
        /// <param name="filter">Tên và mã đối tượng cần tìm kiếm</param>
        /// <param name="pageSize">Kích thước trong 1 bảng ghi</param>
        /// <param name="pageNumber">Số thứ tự hiện tại của trang</param>
        /// <returns>Danh sách đối tượng phân trang</returns>
        /// CreatedBy: Bien (17/1/2023)
        public PagingResult Filter([FromQuery] int offset = 1,
            [FromQuery] int limit = 20,
             [FromQuery] string? filter = null
            );

        /// <summary>
        /// Lấy 1 bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID của bản ghi muốn lấy</param>
        /// <returns>Đối tượng muốn lấy</returns>
        /// CreatedBy: Bien (6/2/2023)
        //public T GetById(Guid entityId);

        /// <summary>
        /// Lấy danh sách bản ghi theo ID
        /// </summary>
        /// <param name="entityId">ID của danh sách muốn lấy</param>
        /// <returns>Danh sách đối tượng</returns>
        /// CreatedBy: Bien (01/05/2023)
        public List<T> GetById(Guid id);

        /// <summary>
        /// Hàm thêm mới nhân viên
        /// </summary>
        /// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        /// <returns>
        /// 1. Nếu thêm thành công 
        /// 0. Nếu thêm thất bại
        /// </returns>
        /// CretaedBy: Bien (6/2/2023)
        public int Insert(T entity);

        /// <summary>
        /// API Sửa
        /// </summary>
        /// <param name="entityId">Id của đối tượng muốn sửa</param>
        /// <param name="entity">Đối tượng mang giá trị đã được thay đổi</param>
        /// <returns> 
        /// 1. Nếu sửa thành công,
        /// 0. Nếu sửa thất bại
        /// </returns>
        /// CreatedBy: Bien (6/2/2023)
        public int Update([FromRoute] Guid entityId, [FromBody] T entity);

        /// <summary>
        /// API Xóa
        /// </summary>
        /// <param name="entityId">Id đối tượng muốn xóa</param>
        /// <returns>
        ///  1.Nếu xóa thành công,
        ///  0.Nếu xóa thất bại
        /// </returns>
        /// CreatedBy: Bien (6/2/2023)
        public int Delete([FromRoute] Guid entityId);

        /// <summary>
        /// API xuất dữ liệu sang file Excel
        /// </summary>
        /// <returns>File Excel chứa dữ liệu</returns>
        /// CreatedBy: Bien (05/04/2023)
        public ServiceResult ExportToExcel(string? filter);

        /// <summary>
        /// Hàm xóa nhiều bản ghi
        /// </summary>
        /// <param name="entityIds">Danh sách id bản ghi muốn xóa</param>
        /// <returns>
        /// >0: Nếu xóa thành công
        /// 0: Nếu xóa thất bại
        /// </returns>
        /// CreatedBy: Bien (24/02/2023)
        public int Deletes(List<Guid> entityIds);
    }
}
