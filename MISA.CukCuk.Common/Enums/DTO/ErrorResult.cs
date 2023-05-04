using MISA.Testamis.Common.Enums;
using System.Net;

namespace MISA.Testamis.Common.Enums.DTO
{
    public class ErrorResult
    {
        /// <summary>
        /// Mã lỗi
        /// </summary>
        public ErrorCode? ErrorCode { get; set; }

        /// <summary>
        /// Thông báo lỗi dev
        /// </summary>
        public string? MsgDev { get; set; }

        /// <summary>
        /// Thông báo lỗi user
        /// </summary>
        public string? MsgUser { get; set; }

        /// <summary>
        /// Thông tin lỗi chia tiết
        /// </summary>
        public object? MoreInfo { get; set; }

        /// <summary>
        /// Mã truy vết lỗi
        /// </summary>
       public string? TraceId { get; set; }
    }
}
