using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Testamis.BL;
using MISA.Testamis;
using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;
using Amazon.Auth.AccessControlPolicy;
using MISA.Testamis.Common.Enums;

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

        /// <summary>
        /// API Cập nhật trạng thái đơn công tác
        /// </summary>
        /// <param name="missionallowanceIds">Danh sách đơn muốn cập nhật</param>
        /// <param name="status">Gía trị trạng thái muốn cập nhập</param>
        /// <returns>
        /// 200: Nếu xóa thành công
        /// 500: Nếu lỗi try catch
        /// </returns>
        /// CreatedBy: Bien (10/05/2023)
        [HttpPut]
        public IActionResult UpdateMissionallowanceStatus(List<Guid> missionallowanceIds, int status)
        {
            try
            {
                // Gọi vào hàm xóa trong BaseBL
                var data = _missionallowanceBL.UpdateMissionallowanceStatus(missionallowanceIds, status);
                return Ok(data);
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

        /// <summary>
        /// API xuất khẩu danh sách đơn đã chọn
        /// </summary>
        /// <param name="missionallowanceIds">Danh sách id đơn đã chọn</param>
        /// <returns>File Excel chứa dữ liệu</returns>
        /// CreatedBy: Bien (10/05/2023)
        [HttpPost("ExportExcelSelected")]
        public IActionResult ExportMissionnallowanceList(List<Guid> missionallowanceIds)
        {
            try
            {
                var data = _missionallowanceBL.ExportMissionnallowanceList(missionallowanceIds);

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


        /// <summary>
        /// API lấy danh sách bản ghi đã tạo ngày hôm nay
        /// </summary>
        /// <returns>
        /// Danh sách đơn công tác đã tạo hôm nay
        /// </returns>
        /// CreatedBy: Bien (12/05/2023
        [HttpGet("MissionallowanceToDay")]
        public IActionResult GetAddMissionallowanceToDay() {

        try
        {
            var data = _missionallowanceBL.GetAddMissionallowanceToDay();

            if (data != null)
            {
                return Ok(data);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResult
                {
                    ErrorCode = ErrorCode.UnknownError,
                    MsgDev = Common.Resource.ErrorMsg_GetAll,
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
                MsgUser = Common.Resource.ErrorMsg,
                TraceId = GetHttpContext().TraceIdentifier
            });
        }

    }
    #endregion
}
}
