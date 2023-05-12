using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.DL
{
    public interface IMissionallowanceDL: IBaseDL<Missionallowance>
    {
        /// <summary>
        /// API Cập nhật trạng thái đơn công tác
        /// </summary>
        /// <param name="missionallowanceIds">Danh sách đơn muốn cập nhật</param>
        /// <param name="status">Gía trị trạng thái muốn cập nhập</param>
        /// <returns>
        /// >0 : Cập nhật thành cồn
        /// 0: Cập nhật thát bại
        /// </returns>
        /// CreatedBy: Bien (10/05/2023)
        public int UpdateMissionallowanceStatus(List<Guid> missionallowanceIds, int status);

        /// <summary>
        /// API xuất khẩu danh sách đơn đã chọn
        /// </summary>
        /// <param name="missionallowanceIds">Danh sách id đơn đã chọn</param>
        /// <returns>Danh sách đơn</returns>
        /// CreatedBy: Bien (10/05/2023)
        public ServiceResult ExportMissionnallowanceList(ExportListSelect dataSelected);

        /// <summary>
        /// API lấy danh sách bản ghi đã tạo ngày hôm nay
        /// </summary>
        /// <returns>
        /// Danh sách đơn công tác đã tạo hôm nay
        /// </returns>
        /// CreatedBy: Bien (12/05/2023
        public List<Guid> GetAddMissionallowanceToDay();
    }
}
