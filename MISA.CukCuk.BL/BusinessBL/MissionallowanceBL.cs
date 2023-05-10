using MISA.Testamis.Common.Entitis;
using MISA.Testamis.Common.Enums.DTO;
using MISA.Testamis.DL;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.BL
{
    public class MissionallowanceBL : BaseBL<Common.Entitis.Missionallowance>, IMissionallowanceBL
    {
        #region Field

        private IMissionallowanceDL _missionallowanceDL;

        #endregion

        #region Constractor

        public MissionallowanceBL(IMissionallowanceDL missionallowanceDL) : base(missionallowanceDL)
        {
            _missionallowanceDL = missionallowanceDL;
        }
        #endregion

        #region Method
        /// <summary>
        /// API xuất dữ liệu sang file Excel
        /// </summary>
        /// <returns>File Excel chứa dữ liệu</returns>
        /// CreatedBy: Bien (05/04/2023)
        public override MemoryStream ExportToExcel(string? filter)
        {
            ServiceResult serviceResult = new ServiceResult();
            // Gọi vào xuất dữ liệu trong BaseDL
            var data = _missionallowanceDL.ExportToExcel(filter);

            if (data.IsSuccess)
            {
                List<Missionallowance> missionallowances = new List<Missionallowance>();

                missionallowances = (List<Missionallowance>)data.Data;


                var stream = new MemoryStream();

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Danh_sach_don_cong_tac");

                    worksheet.Row(2).Height = 20;
                    worksheet.Row(3).Height = 20;

                    worksheet.Cells["A1"].Value = "Danh sách đơn công tác";

                    // Hợp cột A1 -> J1 của dòng 1 trong sheet Danh_sach_don_cong_tac
                    using (var r = worksheet.Cells["A1:Q1"])
                    {
                        r.Merge = true;

                        // Định dạng kiểu chữ
                        r.Style.Font.Size = 16;
                        r.Style.Font.Bold = true;

                        // Căn chính giữa
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    using (var r = worksheet.Cells["A2:Q2"])
                    {
                        r.Merge = true;
                    }
                    using (var r = worksheet.Cells["A3:Q3"])
                    {
                        // Định dạng kiểu chữ
                        r.Style.Font.Size = 12;
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        // Căn chính giữa
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Định dạng border
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    worksheet.Cells["A3"].Value = "STT";
                    worksheet.Cells["B3"].Value = "Mã nhân viên";
                    worksheet.Cells["C3"].Value = "Người đề nghị";
                    worksheet.Cells["D3"].Value = "Vị trí công việc";
                    worksheet.Cells["E3"].Value = "Đơn vị công tác";
                    worksheet.Cells["F3"].Value = "Ngày đề nghị";
                    worksheet.Cells["G3"].Value = "Ngày đi";
                    worksheet.Cells["H3"].Value = "Ngày về";
                    worksheet.Cells["I3"].Value = "Số ngày đi công tác";
                    worksheet.Cells["J3"].Value = "Địa điểm công tác";
                    worksheet.Cells["K3"].Value = "Lý do công tác";
                    worksheet.Cells["L3"].Value = "Yêu cầu hỗ trợ";
                    worksheet.Cells["M3"].Value = "Người hỗ trợ";
                    worksheet.Cells["N3"].Value = "Người duyệt";
                    worksheet.Cells["O3"].Value = "Người liên quan";
                    worksheet.Cells["P3"].Value = "Ghi chú";
                    worksheet.Cells["Q3"].Value = "Trạng thái";

                    worksheet.Column(1).Width = 6;
                    worksheet.Column(2).Width = 20;
                    worksheet.Column(3).Width = 25;
                    worksheet.Column(4).Width = 12;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(6).Width = 30;
                    worksheet.Column(7).Width = 20;
                    worksheet.Column(8).Width = 40;
                    worksheet.Column(9).Width = 20;
                    worksheet.Column(10).Width = 20;
                    worksheet.Column(11).Width = 25;
                    worksheet.Column(12).Width = 25;
                    worksheet.Column(13).Width = 50;
                    worksheet.Column(14).Width = 25;
                    worksheet.Column(15).Width = 50;
                    worksheet.Column(16).Width = 25;
                    worksheet.Column(17).Width = 25;

                    int row = 4;
                    int STT = 1;
                    int start = 4;
                    int end = 4;
                    foreach (var entity in missionallowances)
                    {
                        worksheet.Cells[row, 1].Value = STT++;
                        worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 2].Value = entity.EmployeeCode;
                        worksheet.Cells[row, 3].Value = entity.FullName;
                        worksheet.Cells[row, 4].Value = entity.PositionName;
                        worksheet.Cells[row, 5].Value = entity.DepartmentName;
                        worksheet.Cells[row, 6].Value = entity.RequestDate?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 7].Value = entity.FromDate?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 8].Value = entity.ToDate?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 9].Value = entity.LeaveDay;
                        worksheet.Cells[row, 10].Value = entity.Location;
                        worksheet.Cells[row, 11].Value = entity.Purpose;
                        worksheet.Cells[row, 12].Value = entity.Request;
                        worksheet.Cells[row, 13].Value = entity.SupportNames;
                        worksheet.Cells[row, 14].Value = entity.ApprovalNames;
                        worksheet.Cells[row, 15].Value = entity.RelationShipNames;
                        worksheet.Cells[row, 16].Value = entity.Notes;
                        worksheet.Cells[row, 17].Value = entity.StatusName;

                        // Tạo border 1 trường dữ liệu
                        var recordRow = worksheet.Cells["A" + start++ + ":Q" + end++];

                        recordRow.Style.Font.Size = 12;
                        recordRow.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        recordRow.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        recordRow.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        recordRow.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        row++;
                    }

                    //worksheet.Cells.AutoFitColumns();
                    worksheet.Cells.Style.Font.Name = "Arial";

                    xlPackage.Save();

                }
                stream.Position = 0;

                return stream;
            }
            return new MemoryStream();
        }

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
        public ServiceResult UpdateMissionallowanceStatus(List<Guid> missionallowanceIds, int status)
        {
            var serviceResult = new ServiceResult();

            int numberOfAffectedRows = _missionallowanceDL.UpdateMissionallowanceStatus(missionallowanceIds, status);

            if (numberOfAffectedRows > 0)
            {
                serviceResult.IsSuccess = true;
            }
            else
            {
                serviceResult.IsSuccess = false;
            }
            return serviceResult;
        }

        /// <summary>
        /// API xuất khẩu danh sách đơn đã chọn
        /// </summary>
        /// <param name="missionallowanceIds">Danh sách id đơn đã chọn</param>
        /// <returns>File Excel chứa dữ liệu</returns>
        /// CreatedBy: Bien (10/05/2023)
        public MemoryStream ExportMissionnallowanceList(List<Guid> missionallowanceIds)
        {
            // Gọi vào xuất dữ liệu trong BaseDL
            var data = _missionallowanceDL.ExportMissionnallowanceList(missionallowanceIds);

            if (data.IsSuccess)
            {
                List<Missionallowance> missionallowances = new List<Missionallowance>();

                missionallowances = (List<Missionallowance>)data.Data;


                var stream = new MemoryStream();

                using (var xlPackage = new ExcelPackage(stream))
                {
                    var worksheet = xlPackage.Workbook.Worksheets.Add("Danh_sach_don_cong_tac");

                    worksheet.Row(2).Height = 20;
                    worksheet.Row(3).Height = 20;

                    worksheet.Cells["A1"].Value = "Danh sách đơn công tác";

                    // Hợp cột A1 -> J1 của dòng 1 trong sheet Danh_sach_don_cong_tac
                    using (var r = worksheet.Cells["A1:Q1"])
                    {
                        r.Merge = true;

                        // Định dạng kiểu chữ
                        r.Style.Font.Size = 16;
                        r.Style.Font.Bold = true;

                        // Căn chính giữa
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    using (var r = worksheet.Cells["A2:Q2"])
                    {
                        r.Merge = true;
                    }
                    using (var r = worksheet.Cells["A3:Q3"])
                    {
                        // Định dạng kiểu chữ
                        r.Style.Font.Size = 12;
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(Color.LightGray);

                        // Căn chính giữa
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        // Định dạng border
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    worksheet.Cells["A3"].Value = "STT";
                    worksheet.Cells["B3"].Value = "Mã nhân viên";
                    worksheet.Cells["C3"].Value = "Người đề nghị";
                    worksheet.Cells["D3"].Value = "Vị trí công việc";
                    worksheet.Cells["E3"].Value = "Đơn vị công tác";
                    worksheet.Cells["F3"].Value = "Ngày đề nghị";
                    worksheet.Cells["G3"].Value = "Ngày đi";
                    worksheet.Cells["H3"].Value = "Ngày về";
                    worksheet.Cells["I3"].Value = "Số ngày đi công tác";
                    worksheet.Cells["J3"].Value = "Địa điểm công tác";
                    worksheet.Cells["K3"].Value = "Lý do công tác";
                    worksheet.Cells["L3"].Value = "Yêu cầu hỗ trợ";
                    worksheet.Cells["M3"].Value = "Người hỗ trợ";
                    worksheet.Cells["N3"].Value = "Người duyệt";
                    worksheet.Cells["O3"].Value = "Người liên quan";
                    worksheet.Cells["P3"].Value = "Ghi chú";
                    worksheet.Cells["Q3"].Value = "Trạng thái";

                    worksheet.Column(1).Width = 6;
                    worksheet.Column(2).Width = 20;
                    worksheet.Column(3).Width = 25;
                    worksheet.Column(4).Width = 12;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(6).Width = 30;
                    worksheet.Column(7).Width = 20;
                    worksheet.Column(8).Width = 40;
                    worksheet.Column(9).Width = 20;
                    worksheet.Column(10).Width = 20;
                    worksheet.Column(11).Width = 25;
                    worksheet.Column(12).Width = 25;
                    worksheet.Column(13).Width = 50;
                    worksheet.Column(14).Width = 25;
                    worksheet.Column(15).Width = 50;
                    worksheet.Column(16).Width = 25;
                    worksheet.Column(17).Width = 25;

                    int row = 4;
                    int STT = 1;
                    int start = 4;
                    int end = 4;
                    foreach (var entity in missionallowances)
                    {
                        worksheet.Cells[row, 1].Value = STT++;
                        worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 2].Value = entity.EmployeeCode;
                        worksheet.Cells[row, 3].Value = entity.FullName;
                        worksheet.Cells[row, 4].Value = entity.PositionName;
                        worksheet.Cells[row, 5].Value = entity.DepartmentName;
                        worksheet.Cells[row, 6].Value = entity.RequestDate?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 7].Value = entity.FromDate?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 8].Value = entity.ToDate?.ToString("dd/MM/yyyy hh:mm");
                        worksheet.Cells[row, 8].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 9].Value = entity.LeaveDay;
                        worksheet.Cells[row, 10].Value = entity.Location;
                        worksheet.Cells[row, 11].Value = entity.Purpose;
                        worksheet.Cells[row, 12].Value = entity.Request;
                        worksheet.Cells[row, 13].Value = entity.SupportNames;
                        worksheet.Cells[row, 14].Value = entity.ApprovalNames;
                        worksheet.Cells[row, 15].Value = entity.RelationShipNames;
                        worksheet.Cells[row, 16].Value = entity.Notes;
                        worksheet.Cells[row, 17].Value = entity.StatusName;

                        // Tạo border 1 trường dữ liệu
                        var recordRow = worksheet.Cells["A" + start++ + ":Q" + end++];

                        recordRow.Style.Font.Size = 12;
                        recordRow.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        recordRow.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        recordRow.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        recordRow.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                        row++;
                    }

                    //worksheet.Cells.AutoFitColumns();
                    worksheet.Cells.Style.Font.Name = "Arial";

                    xlPackage.Save();

                }
                stream.Position = 0;

                return stream;
            }
            return new MemoryStream();
        }
        #endregion
    }
}
