using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.Common.Entitis
{
    /// <summary>
    /// Đơn xin đi công tác
    /// </summary>
    public class Missionallowance
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid MissionallowanceId { get; set; }

        /// <summary>
        /// Id nhân viên
        /// </summary>
        [Required(ErrorMessage = "Người đề nghị không được để trống.")]
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string? EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Chức danh
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// ID Đơn vị
        /// </summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// Đơn vị
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// Ngày đi
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Ngày đề nghị
        /// </summary>
        public DateTime? RequestDate { get; set; }

        /// <summary>
        /// Ngày về
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Số ngày đi công tác
        /// </summary>
        public int? LeaveDay { get; set; }

        /// <summary>
        /// Địa chỉ 
        /// </summary>
        [Required(ErrorMessage = "Địa điểm công tác không được để trống.")]

        public string? Location { get; set; }

        /// <summary>
        /// Lý do công tác
        /// </summary>
        [Required(ErrorMessage = "Lý do công tác không được để trống.")]
        public string? Purpose { get; set; }

        /// <summary>
        /// Yêu cầu hỗ trợ
        /// </summary>
        public string? Request { get; set; }

        /// <summary>
        /// Danh sách id người hỗ trợ
        /// </summary>
        public string? SupportIds { get; set; }

        /// <summary>
        /// Danh sách tên người hỗ trợ
        /// </summary>
        public string? SupportNames { get; set; }

        /// <summary>
        /// Id người duyệt
        /// </summary>
        [Required(ErrorMessage = "Người duyệt không được để trống.")]
        public string? ApprovalIds { get; set; }

        /// <summary>
        /// Tên người duyệt
        /// </summary>
        public string? ApprovalNames { get; set; }

        /// <summary>
        /// Danh sách id người liên quan
        /// </summary>
        public string? RelationShipIds { get; set; }

        /// <summary>
        /// Danh sách tên người liên quan
        /// </summary>
        public string? RelationShipNames { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public string? StatusName { get; set; }

        /// <summary>
        /// Danh sách người đi công tác
        /// </summary>
        public string? EmployeeMissionallowances { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày cập nhập
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người cập nhật
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
