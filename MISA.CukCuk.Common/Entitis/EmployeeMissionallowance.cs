using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.Common.Entitis
{
    /// <summary>
    /// Nhân viên đi công tác
    /// </summary>
    public class EmployeeMissionallowance
    {
        /// <summary>
        /// Khóa chính
        /// </summary>
        public Guid MissionAllowanceId { get; set; }

        /// <summary>
        /// Id nhân viên
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// Id đơn công tác
        /// </summary>
        public Guid BusinessId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string PositionName { get; set; }

    }
}
