using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.Common.Entitis
{
    public class ExportListSelect
    {
        /// <summary>
        /// Danh sách đối tượng đã chọn
        /// </summary>
       public List<Guid> missionallowanceIds { get; set; }

        /// <summary>
        /// Danh sách cột hiển thị
        /// </summary>
        public List<DataGrid> dataGrid { get; set; }
    }
}
