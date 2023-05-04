namespace MISA.Testamis.Common.Enums
{
    public enum ErrorCode
    {
        /// <summary>
        /// Không có lỗi
        /// </summary>
        NoError = 0,

        /// <summary>
        /// Lỗi không xác định
        /// </summary>
        UnknownError = 1,

        /// <summary>
        /// Lỗi validate
        /// </summary>
        ValidateError = 2,

        /// <summary>
        /// Lỗi khi gọi vào DL
        /// </summary>
        GetMethodByDLError = 3,

        /// <summary>
        /// Lỗi nhập dữ liệu
        /// </summary>
        ValueInputError = 4,
    
    }
}
