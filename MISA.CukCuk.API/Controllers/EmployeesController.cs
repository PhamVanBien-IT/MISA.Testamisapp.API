
using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums;
using MISA.Testamis.Common.Enums.DTO;
using MISA.Testamis.BL;
using MISA.Testamis.DL;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using MISA.Testamis.API.Controllers;
using MISA.Testamis.API;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;

namespace MISA.Testamis.Common.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        #region Field

        private IEmployeeBL _employeeBL;

        #endregion

        #region Constractor

        public EmployeesController(IEmployeeBL employeeBL) : base(employeeBL)
        {
            _employeeBL = employeeBL;
        }

        #endregion

        #region Method
        /// <summary>
        /// API Lấy tất cả danh sách nhân viên
        /// </summary>
        /// <returns>Danh sách nhân viên</returns>
        /// CreatedBy: Bien (27/04/2023)
        [HttpGet]
        public IActionResult GetEmployees()
        {

            try
            {
                var data = _employeeBL.GetEmployees();

                if (data != null)
                {
                    return Ok(data);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                    {
                        ErrorCode = ErrorCode.UnknownError,
                        MsgDev = Resource.ErrorMsgDev_Filter,
                        TraceId = GetHttpContext().TraceIdentifier
                    });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = ErrorCode.UnknownError,
                    MsgDev = ex.Message,
                    MsgUser = Resource.ErrorMsg,
                    TraceId = GetHttpContext().TraceIdentifier
                });
            }

            
        }
        /// <summary>
        /// API sinh mới mã nhân viên
        /// </summary>
        /// <returns>Sinh mới mã nhân viên</returns>
        /// CreatedBy: Bien (17/1/2023)
        [HttpGet("NewEmployeeCode")]
        public IActionResult NewEmployeeCode()
        {
            try
            {
                var data = _employeeBL.NewEmployeeCode();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = Enums.ErrorCode.UnknownError,
                    MsgDev = ex.Message,
                    MsgUser = Resource.ErrorMsg,
                });
            }
        }

        #endregion
    }
}
