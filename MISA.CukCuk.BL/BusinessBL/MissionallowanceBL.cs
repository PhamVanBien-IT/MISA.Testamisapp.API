﻿using MISA.Testamis.Common.Entitis;
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
using MISA.Testamis.Common.Enums;
using Newtonsoft.Json.Linq;
using MISA.Testamis.Common.Constants;
using System.IO;
using System.Reflection;
using static System.Net.WebRequestMethods;

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
        /// Hàm kiểm tra những lỗi riêng
        /// </summary>
        /// <param name="entity">Đối tượng muốn kiểm tra dữ liệu</param>
        /// <param name="isInsert">Kiểm tra thao tác thực hiện là thêm hay sửa</param>
        /// <returns>Danh sách lỗi</returns>
        /// CreatedBy: Bien (05/04/2023)
        protected override Dictionary<string, string> ValidateCustom(Missionallowance? missionallowance, bool isInsert = true)
        {

            var validateFailures = new Dictionary<string, string>();

            if (missionallowance.FromDate > missionallowance.ToDate)
            {
                validateFailures.Add("ToDate", Common.Resource.ErrorMsg_ToDate);
            }

            return validateFailures;
        }
        /// <summary>
        /// API xuất dữ liệu sang file Excel
        /// </summary>
        /// <returns>File Excel chứa dữ liệu</returns>
        /// CreatedBy: Bien (05/04/2023)
        public override MemoryStream ExportToExcel(string? filter, List<DataGrid> dataGrid)
        {
            ServiceResult serviceResult = new ServiceResult();

            var listCaption = new string[dataGrid.Count];

            var listDataField = new string[dataGrid.Count];

            var listHeader = new int[dataGrid.Count];

            var letters = new char[listHeader.Length];

            var numberI = 0;

            for (int i = 0; i < dataGrid.Count; i++)
            {
                if (dataGrid[i].Visible)
                {
                    numberI = numberI + 1;
                    listHeader[i] = numberI;
                    listCaption[i] = dataGrid[i].Caption;
                    listDataField[i] = dataGrid[i].DataField;
                }
            }

            for (int i = 0; i < listHeader.Length; i++)
            {
                if (listHeader[i] != 0)
                {
                    letters[i] = (char)(listHeader[i] + 64);
                }
                else
                {
                    letters[i] = letters[i];
                }

             ; // chuyển đổi giá trị số thành ký tự ASCII
            }
            var numberRow = string.Join(",", listHeader);

            var number = numberRow.Replace(",0", "");

            var listNumber = number.Split(',');

            Array.Resize(ref listNumber, listNumber.Length - 1);

            var listRow = string.Join(",", letters);

            var list = listRow.Replace("\0,", "");

            var listAlphabet = list.Split(',');

            Array.Resize(ref listAlphabet, listAlphabet.Length - 1);

            var captions = listCaption.Where(item => item != null).ToList();

            var dataFields = listDataField.Where(item => item != null).ToList();

            var nameExcel = new string[listAlphabet.Length];


            for (int i = 0; i < listAlphabet.Length; i++)
            {
                nameExcel[i] = (string)(listAlphabet[i] + '3');

            }
            // Gọi vào xuất dữ liệu trong BaseDL
            var data = _missionallowanceDL.ExportToExcel(filter, dataGrid);

            if (data.IsSuccess)
            {

                //var lengthDataGrid = dataGrid["ValueKind"];

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
                    using (var r = worksheet.Cells[$"{listAlphabet[0]}1:{listAlphabet[listAlphabet.Length - 1]}1"])
                    {
                        r.Merge = true;

                        // Định dạng kiểu chữ
                        r.Style.Font.Size = 16;
                        r.Style.Font.Bold = true;

                        // Căn chính giữa
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    using (var r = worksheet.Cells[$"{listAlphabet[0]}2:{listAlphabet[listAlphabet.Length - 1]}2"])
                    {
                        r.Merge = true;
                    }
                    using (var r = worksheet.Cells[$"{nameExcel[0]}:{nameExcel[nameExcel.Length - 1]}"])
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
                    for (int i = 0; i < nameExcel.Length; i++)
                    {
                        worksheet.Cells[$"{nameExcel[i]}"].Value = captions[i];
                    }

                    worksheet.Column(1).Width = 20;
                    worksheet.Column(2).Width = 20;
                    worksheet.Column(3).Width = 25;
                    worksheet.Column(4).Width = 20;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(6).Width = 30;
                    worksheet.Column(7).Width = 20;
                    worksheet.Column(8).Width = 20;
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
                        for(var i = 0; i< dataFields.Count; i++)
                        {
                            var field = dataFields[i];
                            switch(field)
                            {
                                case "EmployeeCode":
                                    worksheet.Cells[row, i + 1].Value = entity.EmployeeCode;
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    break;
                                case "FullName":
                                    worksheet.Cells[row, i + 1].Value = entity.FullName;
                                    break;
                                case "PositionName":
                                    worksheet.Cells[row, i + 1].Value = entity.PositionName;
                                    break;
                                case "DepartmentName":
                                    worksheet.Cells[row, i + 1].Value = entity.DepartmentName;
                                    break;
                                case "RequestDate":
                                    worksheet.Cells[row, i + 1].Value = entity.RequestDate;
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, i + 1].Value = entity.RequestDate?.ToString("dd/MM/yyyy hh:mm");

                                    break;
                                case "FromDate":
                                    worksheet.Cells[row, i + 1].Value = entity.FromDate;
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, i + 1].Value = entity.RequestDate?.ToString("dd/MM/yyyy hh:mm");
                                    break;
                                case "ToDate":
                                    worksheet.Cells[row, i + 1].Value = entity.ToDate;
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, i + 1].Value = entity.RequestDate?.ToString("dd/MM/yyyy hh:mm");
                                    break;
                                case "LeaveDay":
                                    worksheet.Cells[row, i + 1].Value = entity.LeaveDay;
                                    break;
                                case "Location":
                                    worksheet.Cells[row, i + 1].Value = entity.Location;
                                    break;
                                case "Purpose":
                                    worksheet.Cells[row, i + 1].Value = entity.Purpose;
                                    break;
                                case "Request":
                                    worksheet.Cells[row, i + 1].Value = entity.Request;
                                    break;
                                case "SupportNames":
                                    worksheet.Cells[row, i + 1].Value = entity.SupportNames;
                                    break;
                                case "ApprovalNames":
                                    worksheet.Cells[row, i + 1].Value = entity.ApprovalNames;
                                    break;
                                case "RelationShipNames":
                                    worksheet.Cells[row, i + 1].Value = entity.RelationShipNames;
                                    break;
                                case "Notes":
                                    worksheet.Cells[row, i + 1].Value = entity.Notes;
                                    break;
                                case "StatusName":
                                    worksheet.Cells[row, i + 1].Value = entity.StatusName;
                                    break;
                            }
                        }
                        
                        // Tạo border 1 trường dữ liệu
                        var recordRow = worksheet.Cells[$"{listAlphabet[0]}" + start++ + $":{listAlphabet[listAlphabet.Length - 1]}" + end++];

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
        public MemoryStream ExportMissionnallowanceList(ExportListSelect dataSelected)
        {
            var listCaption = new string[dataSelected.dataGrid.Count];

            var listDataField = new string[dataSelected.dataGrid.Count];

            var listHeader = new int[dataSelected.dataGrid.Count];

            var letters = new char[listHeader.Length];

            var numberI = 0;

            for (int i = 0; i < dataSelected.dataGrid.Count; i++)
            {
                if (dataSelected.dataGrid[i].Visible)
                {
                    numberI = numberI + 1;
                    listHeader[i] = numberI;
                    listCaption[i] = dataSelected.dataGrid[i].Caption;
                    listDataField[i] = dataSelected.dataGrid[i].DataField;
                }
            }

            for (int i = 0; i < listHeader.Length; i++)
            {
                if (listHeader[i] != 0)
                {
                    letters[i] = (char)(listHeader[i] + 64);
                }
                else
                {
                    letters[i] = letters[i];
                }

             ; // chuyển đổi giá trị số thành ký tự ASCII
            }
            var numberRow = string.Join(",", listHeader);

            var number = numberRow.Replace(",0", "");

            var listNumber = number.Split(',');

            Array.Resize(ref listNumber, listNumber.Length - 1);

            var listRow = string.Join(",", letters);

            var list = listRow.Replace("\0,", "");

            var listAlphabet = list.Split(',');

            Array.Resize(ref listAlphabet, listAlphabet.Length - 1);

            var captions = listCaption.Where(item => item != null).ToList();

            var dataFields = listDataField.Where(item => item != null).ToList();

            var nameExcel = new string[listAlphabet.Length];


            for (int i = 0; i < listAlphabet.Length; i++)
            {
                nameExcel[i] = (string)(listAlphabet[i] + '3');

            }
            // Gọi vào xuất dữ liệu trong BaseDL
            var data = _missionallowanceDL.ExportMissionnallowanceList(dataSelected);

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
                    using (var r = worksheet.Cells[$"{listAlphabet[0]}1:{listAlphabet[listAlphabet.Length - 1]}1"])
                    {
                        r.Merge = true;

                        // Định dạng kiểu chữ
                        r.Style.Font.Size = 16;
                        r.Style.Font.Bold = true;

                        // Căn chính giữa
                        r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        r.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    using (var r = worksheet.Cells[$"{listAlphabet[0]}2:{listAlphabet[listAlphabet.Length - 1]}2"])
                    {
                        r.Merge = true;
                    }
                    using (var r = worksheet.Cells[$"{nameExcel[0]}:{nameExcel[nameExcel.Length - 1]}"])
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
                    for (int i = 0; i < nameExcel.Length; i++)
                    {
                        worksheet.Cells[$"{nameExcel[i]}"].Value = captions[i];
                    }

                    worksheet.Column(1).Width = 20;
                    worksheet.Column(2).Width = 20;
                    worksheet.Column(3).Width = 25;
                    worksheet.Column(4).Width = 20;
                    worksheet.Column(5).Width = 20;
                    worksheet.Column(6).Width = 30;
                    worksheet.Column(7).Width = 20;
                    worksheet.Column(8).Width = 20;
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
                        for (var i = 0; i < dataFields.Count; i++)
                        {
                            var field = dataFields[i];
                            switch (field)
                            {
                                case "EmployeeCode":
                                    worksheet.Cells[row, i + 1].Value = entity.EmployeeCode;
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                    break;
                                case "FullName":
                                    worksheet.Cells[row, i + 1].Value = entity.FullName;
                                    break;
                                case "PositionName":
                                    worksheet.Cells[row, i + 1].Value = entity.PositionName;
                                    break;
                                case "DepartmentName":
                                    worksheet.Cells[row, i + 1].Value = entity.DepartmentName;
                                    break;
                                case "RequestDate":
                                    worksheet.Cells[row, i + 1].Value = entity.RequestDate;
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, i + 1].Value = entity.RequestDate?.ToString("dd/MM/yyyy hh:mm");

                                    break;
                                case "FromDate":
                                    worksheet.Cells[row, i + 1].Value = entity.FromDate;
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, i + 1].Value = entity.RequestDate?.ToString("dd/MM/yyyy hh:mm");
                                    break;
                                case "ToDate":
                                    worksheet.Cells[row, i + 1].Value = entity.ToDate;
                                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[row, i + 1].Value = entity.RequestDate?.ToString("dd/MM/yyyy hh:mm");
                                    break;
                                case "LeaveDay":
                                    worksheet.Cells[row, i + 1].Value = entity.LeaveDay;
                                    break;
                                case "Location":
                                    worksheet.Cells[row, i + 1].Value = entity.Location;
                                    break;
                                case "Purpose":
                                    worksheet.Cells[row, i + 1].Value = entity.Purpose;
                                    break;
                                case "Request":
                                    worksheet.Cells[row, i + 1].Value = entity.Request;
                                    break;
                                case "SupportNames":
                                    worksheet.Cells[row, i + 1].Value = entity.SupportNames;
                                    break;
                                case "ApprovalNames":
                                    worksheet.Cells[row, i + 1].Value = entity.ApprovalNames;
                                    break;
                                case "RelationShipNames":
                                    worksheet.Cells[row, i + 1].Value = entity.RelationShipNames;
                                    break;
                                case "Notes":
                                    worksheet.Cells[row, i + 1].Value = entity.Notes;
                                    break;
                                case "StatusName":
                                    worksheet.Cells[row, i + 1].Value = entity.StatusName;
                                    break;
                            }
                        }

                        // Tạo border 1 trường dữ liệu
                        var recordRow = worksheet.Cells[$"{listAlphabet[0]}" + start++ + $":{listAlphabet[listAlphabet.Length - 1]}" + end++];

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
        /// API lấy danh sách bản ghi đã tạo ngày hôm nay
        /// </summary>
        /// <returns>
        /// Danh sách đơn công tác đã tạo hôm nay
        /// </returns>
        /// CreatedBy: Bien (12/05/2023
        public ServiceResult GetAddMissionallowanceToDay()
        {
            var serviceResult = new ServiceResult();

            var data = _missionallowanceDL.GetAddMissionallowanceToDay();

            if (data != null)
            {
                serviceResult.Data = data;
                serviceResult.IsSuccess = true;
                serviceResult.ErrorCode = ErrorCode.NoError;
            }
            else
            {
                serviceResult.IsSuccess = false;
                serviceResult.ErrorCode = ErrorCode.UnknownError;
            }

            return serviceResult;
        }
        #endregion
    }
}
