using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.BL;
using MISA.Testamis;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;
using Amazon.Auth.AccessControlPolicy;

namespace MISA.Testamis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionallowancesController : BaseController<Missionallowance>
    {
    
        #region Field

        private IMissionallowanceBL _missionallowanceBL;

        #endregion

        #region Constractor

        public MissionallowancesController(IMissionallowanceBL missionallowanceBL) : base(missionallowanceBL)
        {
            _missionallowanceBL = missionallowanceBL;
        }

        #endregion

        #region Method
        /// <summary>
        /// API Xuất dữ liệu sang Excel
        /// </summary>
        /// <param name="filter">Tìm kiếm theo tên và mã</param>
        /// <returns>File dữ liệu Excel</returns>
        /// CreatedBy: Bien (05/04/2023)
        [HttpGet("ExportExcel")]
        public IActionResult ExportToExcel(string? filter)
        {
            try
            {
                var data = _missionallowanceBL.ExportToExcel(filter);

                if (data != null)
                {
                    return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Danh_sach_don_cong_tac.xlsx");
                }

                else
                {
                    // Lấy danh sách thất bại
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                    {
                        ErrorCode = Common.Enums.ErrorCode.UnknownError,
                        MsgDev = Common.Resource.ErrorMsgDev_Export,
                        MsgUser = Common.Resource.ErrorMsg,
                        TraceId = GetHttpContext().TraceIdentifier
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult
                {
                    ErrorCode = Common.Enums.ErrorCode.UnknownError,
                    MsgDev = ex.Message,
                    MsgUser = Common.Resource.ErrorMsg,
                    TraceId = GetHttpContext().TraceIdentifier
                });
            }
        }
        #endregion
    }
}
