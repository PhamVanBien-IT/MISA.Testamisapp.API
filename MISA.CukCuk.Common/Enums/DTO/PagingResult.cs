using MISA.Testamis.Common.Enums;

namespace MISA.Testamis.API.Enums.DTO
{   /// <summary>
    /// Pagingm
    /// </summary>
    public class PagingResult
    {
        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public int TotalRecord { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPage { get; set; }
        /// <summary>
        /// Dữ liệu
        /// </summary>
        public object Data { get; set; }
    }
}
