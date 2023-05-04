using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;
using MISA.Testamis.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.BL
{
    public interface IEmployeeBL : IBaseBL<Employee>
    {
        /// <summary>
        /// API Sinh mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// CreatedBy: Bien (05/04/2023)
        public ServiceResult NewEmployeeCode();

        /// <summary>
        /// Hàm kiểm tra trùng mã nhân viên
        /// </summary>
        /// <returns>
        /// True: Nếu mã nhân viên đã tồn tại
        /// False: Nếu mã nhân viên hợp lệ
        /// </returns>
        /// CreatedBy: Bien (05/04/2023)
        public bool CheckEmployeeCode(Employee employee, bool isInsert = true);

        /// <summary>
        /// API Lấy tất cả danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: Bien (27/04/2023)
        public ServiceResult GetEmployees();
    }
}
