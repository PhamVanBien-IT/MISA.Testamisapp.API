using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.Common.Entitis
{
    public class DataGrid
    {
        /// <summary>
        /// Nội dung tiêu đề
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Hiển thị hay không
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Trường dữ liệu tương ứng
        /// </summary>
        public string DataField { get; set; }
    }
}
