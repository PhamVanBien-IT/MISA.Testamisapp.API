using MISA.Testamis.Common.Entitis;

namespace MISA.Testamis.Common.Entitis
{
    public class Department
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Tên phòng
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Mã định dạng đơn vị
        /// </summary>
        public string MisaCode { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Thời gian tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Thời gian sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa cuối cùng
        /// </summary>
        public string? ModifiedBy { get; set; }

    }
}
