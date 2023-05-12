using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.BL
{
    public interface IMissionallowanceBL: IBaseBL<Common.Entitis.Missionallowance>
    {
        /// <summary>
        /// API Cập nhật trạng thái đơn công tác
        /// </summary>
        /// <param name="missionallowanceIds">Danh sách đơn muốn cập nhật</param>
        /// <param name="status">Gía trị trạng thái muốn cập nhập</param>
        /// <returns>
        /// isSucces:True sửa thành công
        /// isSucces:False sửa thất bại
        /// </returns>
        /// CreatedBy: Bien (10/05/2023)
        public ServiceResult UpdateMissionallowanceStatus(List<Guid> missionallowanceIds, int status);

        /// <summary>
        /// API xuất khẩu danh sách đơn đã chọn
        /// </summary>
        /// <param name="missionallowanceIds">Danh sách id đơn đã chọn</param>
        /// <returns>File Excel chứa dữ liệu</returns>
        /// CreatedBy: Bien (10/05/2023)
        public MemoryStream ExportMissionnallowanceList(List<Guid> missionallowanceIds);

        /// <summary>
        /// API lấy danh sách bản ghi đã tạo ngày hôm nay
        /// </summary>
        /// <returns>
        /// Danh sách đơn công tác đã tạo hôm nay
        /// </returns>
        /// CreatedBy: Bien (12/05/2023
        public ServiceResult GetAddMissionallowanceToDay();
    }
}
