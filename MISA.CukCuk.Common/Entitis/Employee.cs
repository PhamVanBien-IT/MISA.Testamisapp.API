
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MISA.Testamis.Common.Entitis
{
    // Thông tin nhân viên
    public class Employee
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        //[Key]
        public Guid EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [Required(ErrorMessage = "Mã không được để trống")]
        [DisplayName("Mã nhân viên")]
        [RegularExpression(@"NV-[0-9]{5,17}\b")]

        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required(ErrorMessage = "Tên không được để trống")]
        public string FullName { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Khóa ngoài
        /// </summary>
        [Required(ErrorMessage = "Đơn vị không được để trống")]
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// Số điên thoại cố định
        /// </summary>
        [DisplayName("Điện thoại cố định")]
        [RegularExpression(@"([\+84|84|0]+(3|5|7|8|9|1[2|6|8|9]))+([0-9]{8})\b")]

        public string? LandlineNumber { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        [DisplayName("Điện thoại di động")]
        [RegularExpression(@"([\+84|84|0]+(3|5|7|8|9|1[2|6|8|9]))+([0-9]{8})\b")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DisplayName("Email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,5})+)$")]
        public string? Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Số chứng minh nhân dân
        /// </summary>
        [DisplayName("Số chứng minh nhân dân")]
        [RegularExpression(@"^([0-9]{12})\b")]
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Chức vụ
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// Số tài khoản
        /// </summary>
        public string? BankAccount { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh ngân hàng
        /// </summary>
        public string? BankBranch { get; set; }

        /// <summary>
        /// Tên giới tính
        /// </summary>
        public string? GenderName { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }

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
