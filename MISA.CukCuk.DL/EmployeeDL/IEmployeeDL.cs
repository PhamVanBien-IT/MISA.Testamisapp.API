using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.API.Enums.DTO;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.DL
{
    public interface IEmployeeDL : IBaseDL<Employee>
    {
        /// <summary>
        /// API Sinh mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        public string GetMaxCode();

        /// <summary>
        /// API kiểm tra trùng mã nhân viên
        /// </summary>
        /// <returns>
        /// Id nhân viên: Nếu mã nhân viên đã tồn tại
        /// Null: Nếu mã nhân viên hợp lệ
        /// </returns>
        /// CreatedBy: Bien (20/03/2023)
        public Guid CheckEmployeeCode(string employeeCode);

        /// <summary>
        /// API Lấy tất cả danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: Bien (27/04/2023)
        public List<Employee> GetEmployees();
    }
}
