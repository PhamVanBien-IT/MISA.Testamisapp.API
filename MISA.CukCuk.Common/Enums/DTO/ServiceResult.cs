using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.Common.Enums.DTO
{
    public class ServiceResult
    {
        /// <summary>
        /// Hàm khởi tạo lỗi
        /// </summary>
        /// CreatedBy: Bien (24/03/2023)
        public ServiceResult() { 
        }
        /// <summary>
        /// Hàm khởi tạo lỗi
        /// </summary>
        /// <param name="isSucces">Trạng thái thực hiện</param>
        /// <param name="errorCode">Mã lỗi</param>
        /// <param name="massage">Nội dung lỗi</param>
        /// CreatedBy: Bien (24/03/2023)
        public ServiceResult(bool isSucces,ErrorCode errorCode, string massage)
        {
            IsSuccess = isSucces;
            ErrorCode = errorCode;
            Message = massage;
        }
        /// <summary>
        /// Kết quả validate: 
        /// true là không có lỗi,
        /// false là có lỗi
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Mã lỗi
        /// </summary>
        public ErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Danh sách lỗi
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        ///  Thông báo lỗi
        /// </summary>
        public string Message { get; set; }
    }
}
