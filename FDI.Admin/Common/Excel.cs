using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using FDI.Simple;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FDI.Admin.Common
{
    public class Excel
    {
        public static void ExportListCard(string filePath, List<DNCardItem> lst)
        {
            var newFile = new FileInfo(filePath);
            using (var xlPackage = new ExcelPackage(newFile))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("LSGD");
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                var properties = new[]
                    {
                        "STT",
                        "Mã từ",
                        "Mã thẻ",
                        "Mã pin"
                    };
                worksheet.Cells["A1:D1"].Value = "DANH SACH THẺ";
                worksheet.Cells["A1:D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                worksheet.Cells["A1:D1"].Style.Font.Bold = true;
                worksheet.Cells["A1:D1"].Style.Font.Size = 12;
                worksheet.Cells["A1:D1"].Merge = true;
                worksheet.Cells["A1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var row = 3;
                for (var i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[row, i + 1].Value = properties[i];
                    worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[row, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
                row++;
                var stt = 1;
                foreach (var item in lst)
                {
                    var col = 1;
                    worksheet.Cells[row, col].Value = stt++;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "0";
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[row, col].Value = item.Code;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    worksheet.Cells[row, col++].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    worksheet.Cells[row, col].Value = item.Serial;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    worksheet.Cells[row, col++].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    worksheet.Cells[row, col].Value = item.PinCard;
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";

                    row++;
                }
                var nameexcel = "Danh sách thẻ" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                xlPackage.Workbook.Properties.Title = string.Format("{0} reports",nameexcel);
                xlPackage.Workbook.Properties.Author = "Admin-IT";
                xlPackage.Workbook.Properties.Subject = "reports";
                xlPackage.Workbook.Properties.Category = "Report";
                xlPackage.Workbook.Properties.Company = "SSC";
                xlPackage.Save();
            }
        }

        //public static void ExportToCard(string filePath, List<CardItem> lst)
        //{
        //    var newFile = new FileInfo(filePath);
        //    using (var xlPackage = new ExcelPackage(newFile))
        //    {
        //        var worksheet = xlPackage.Workbook.Worksheets.Add("Order");
        //        xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
        //        var properties = new[]
        //            {
        //                "STT",
        //                "Mã KH",
        //                "Mã thẻ",
        //                "Tên KH",
        //                "Số dư",
        //                "Loại thẻ",
        //                "Trạng thái",
        //                "Ngày phát hành"
        //            };
        //        worksheet.Cells["A1:H2"].Value = "DANH SÁCH THẺ";
        //        worksheet.Cells["A1:H2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells["A1:H2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //        worksheet.Cells["A1:H2"].Style.Font.Bold = true;
        //        worksheet.Cells["A1:H2"].Style.Font.Size = 12;
        //        worksheet.Cells["A1:H2"].Merge = true;
        //        worksheet.Cells["A1:H2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        var row = 4;
        //        for (var i = 0; i < properties.Length; i++)
        //        {
        //            worksheet.Cells[row, i + 1].Value = properties[i];
        //            worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }
        //        worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
        //        row++;
        //        var stt = 1;
        //        foreach (var item in lst)
        //        {
        //            var col = 1;
        //            worksheet.Cells[row, col].Value = stt++;
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.CustomerID;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CardNumber;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CustomerName;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Balance;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells[row, col].Value = item.CardType;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CardStatus;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.DateIssue;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "@";
        //            row++;
        //        }
        //        var nameexcel = "Danh sách thẻ" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        //        xlPackage.Workbook.Properties.Title = $"{nameexcel} reports";
        //        xlPackage.Workbook.Properties.Author = "Admin-IT";
        //        xlPackage.Workbook.Properties.Subject = $"{""} reports";
        //        xlPackage.Workbook.Properties.Category = "Report";
        //        xlPackage.Workbook.Properties.Company = "NetPos";
        //        xlPackage.Save();
        //    }
        //}

        //public static void ExportToCardBackList(string filePath, List<BackListItem> grid)
        //{
        //    var newFile = new FileInfo(filePath);
        //    using (var xlPackage = new ExcelPackage(newFile))
        //    {
        //        var worksheet = xlPackage.Workbook.Worksheets.Add("Order");
        //        xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
        //        var properties = new[]
        //            {
        //                "STT",
        //                "Ngày khóa",
        //                "Mã thẻ",
        //                "Số dư",
        //                "Trạng thái",
        //                "Mã KH",
        //                "Lớp",
        //                "Tên KH",
        //                "Loại thẻ",
        //                "Năm học",
        //                "Mô tả"
        //            };
        //        worksheet.Cells["A1:J2"].Value = "DANH SÁCH THẺ ĐEN";
        //        worksheet.Cells["A1:J2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells["A1:J2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //        worksheet.Cells["A1:J2"].Style.Font.Bold = true;
        //        worksheet.Cells["A1:J2"].Style.Font.Size = 12;
        //        worksheet.Cells["A1:J2"].Merge = true;
        //        worksheet.Cells["A1:J2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        var row = 4;
        //        for (var i = 0; i < properties.Length; i++)
        //        {
        //            worksheet.Cells[row, i + 1].Value = properties[i];
        //            worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }
        //        worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
        //        row++;
        //        var stt = 1;
        //        foreach (var item in grid)
        //        {
        //            var col = 1;
        //            worksheet.Cells[row, col].Value = stt++;
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.Date;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "dd/MM/yyyy";

        //            worksheet.Cells[row, col].Value = item.CardNumber;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Balance;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells[row, col].Value = item.CardStatus;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CustomerID;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CustomerClass;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CustomerName;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CardType;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.SchoolYear;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Desc;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";
        //            row++;
        //        }
        //        var nameexcel = "Danh sách thẻ đen" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        //        xlPackage.Workbook.Properties.Title = $"{nameexcel} reports";
        //        xlPackage.Workbook.Properties.Author = "Admin-IT";
        //        xlPackage.Workbook.Properties.Subject = $"{""} reports";
        //        xlPackage.Workbook.Properties.Category = "Report";
        //        xlPackage.Workbook.Properties.Company = "NetPos";
        //        xlPackage.Save();
        //    }
        //}

        //public static void ExportToReportCardDetail(string filePath, List<EventItem> grid)
        //{
        //    var newFile = new FileInfo(filePath);
        //    using (var xlPackage = new ExcelPackage(newFile))
        //    {
        //        var worksheet = xlPackage.Workbook.Worksheets.Add("GiaoDich");
        //        xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
        //        var properties = new[]
        //            {
        //                "STT",
        //                "Thời gian",
        //                "Mã thẻ",
        //                "Loại giao dịch",
        //                "Số tiền",
        //                "Đối tượng",
        //                "Khu vục"
        //            };
        //        worksheet.Cells["A1:G2"].Value = "CHI TIẾT GIAO DỊCH";
        //        worksheet.Cells["A1:G2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells["A1:G2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //        worksheet.Cells["A1:G2"].Style.Font.Bold = true;
        //        worksheet.Cells["A1:G2"].Style.Font.Size = 14;
        //        worksheet.Cells["A1:G2"].Merge = true;
        //        worksheet.Cells["A1:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        var row = 4;
        //        for (var i = 0; i < properties.Length; i++)
        //        {
        //            worksheet.Cells[row, i + 1].Value = properties[i];
        //            worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }
        //        worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
        //        row++;
        //        var stt = 1;
        //        foreach (var item in grid)
        //        {
        //            var col = 1;
        //            worksheet.Cells[row, col].Value = stt++;
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.Date;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "dd/MM/yyyy";

        //            worksheet.Cells[row, col].Value = item.CardNumber;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Event;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Value;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells[row, col].Value = item.Object;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Area;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "@";
        //            row++;
        //        }
        //        var nameexcel = "tong giao dich" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        //        xlPackage.Workbook.Properties.Title = $"{nameexcel} reports";
        //        xlPackage.Workbook.Properties.Author = "Admin-IT";
        //        xlPackage.Workbook.Properties.Subject = $"{""} reports";
        //        xlPackage.Workbook.Properties.Category = "Report";
        //        xlPackage.Workbook.Properties.Company = "NetPos";
        //        xlPackage.Save();
        //    }
        //}

        //public static void ExportToTalCard(string filePath, ModelCardItem model)
        //{
        //    var newFile = new FileInfo(filePath);
        //    using (var xlPackage = new ExcelPackage(newFile))
        //    {
        //        var worksheet = xlPackage.Workbook.Worksheets.Add("TKT");
        //        xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
        //        var properties = new[]
        //            {
        //                "",
        //                "Tổng số thẻ",
        //                "Tổng số thẻ đã phát hành - T",
        //                "Tổng số thẻ còn lại - R",
        //                "Tổng số thẻ đã khóa",
        //                "Số dư tài khoản"
        //            };

        //        var row = 2;
        //        for (var i = 0; i < properties.Length; i++)
        //        {
        //            worksheet.Cells[row, i + 1].Value = properties[i];
        //            worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }
        //        worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
        //        row++;
        //        foreach (var item in model.LsThongKeTheItems)
        //        {
        //            var col = 1;
        //            worksheet.Cells[row, col].Value = item.NameType;
        //            worksheet.Cells[row, col].Style.Font.Bold = true;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.TotalCard;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "0";

        //            worksheet.Cells[row, col].Value = item.TotalUsed;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "0";

        //            worksheet.Cells[row, col].Value = item.TotalNotUsed;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "0";

        //            worksheet.Cells[row, col].Value = item.TotalBlock;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.TotalBalance;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "#,##";
        //            row++;
        //        }

        //        // chi tiết
        //        row++;
        //        var properties1 = new[]
        //            {
        //                "STT",
        //                "Mã KH",
        //                "Mã thẻ",
        //                "Tên KH",
        //                "Số dư",
        //                "Loại thẻ",
        //                "Trạng thái",
        //                "Ngày phát hành"
        //            };
        //        worksheet.Cells["A" + row + ":H" + row].Value = "DANH SÁCH THẺ";
        //        worksheet.Cells["A" + row + ":H" + row].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells["A" + row + ":H" + row].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //        worksheet.Cells["A" + row + ":H" + row].Style.Font.Bold = true;
        //        worksheet.Cells["A" + row + ":H" + row].Style.Font.Size = 12;
        //        worksheet.Cells["A" + row + ":H" + row].Merge = true;
        //        worksheet.Cells["A" + row + ":H" + row].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        row++;
        //        for (var i = 0; i < properties1.Length; i++)
        //        {
        //            worksheet.Cells[row, i + 1].Value = properties1[i];
        //            worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }
        //        worksheet.Cells[row, 1, row, properties1.Length].AutoFilter = true;
        //        row++;
        //        var stt1 = 1;
        //        foreach (var item in model.ListItems)
        //        {
        //            var col = 1;
        //            worksheet.Cells[row, col].Value = stt1++;
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.CustomerID;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CardNumber;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CustomerName;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Balance;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells[row, col].Value = item.CardType;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CardStatus;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.DateIssue;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "@";
        //            row++;
        //        }

        //        var nameexcel = "Thong ke the" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        //        xlPackage.Workbook.Properties.Title = $"{nameexcel} reports";
        //        xlPackage.Workbook.Properties.Author = "Admin-IT";
        //        xlPackage.Workbook.Properties.Subject = $"{""} reports";
        //        xlPackage.Workbook.Properties.Category = "Report";
        //        xlPackage.Workbook.Properties.Company = "NetPos";
        //        xlPackage.Save();
        //    }
        //}

        //public static void BCDoanhThuBanThe(string filePath, List<EventItem> grid, HttpRequestBase httpRequest)
        //{
        //    var newFile = new FileInfo(filePath);
        //    using (var xlPackage = new ExcelPackage(newFile))
        //    {
        //        var worksheet = xlPackage.Workbook.Worksheets.Add("DTBT");
        //        xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
        //        var properties = new[]
        //            {
        //                "STT",
        //                "Mã giao dịch",
        //                "Thời gian",
        //                "Mã HS",
        //                "Mã thẻ",
        //                "Loại giao dịch",
        //                "Số tiền",
        //                "Số dư",
        //                "Đối tượng",
        //                "Khu vực",
        //                "Nguồn tiền"
        //            };
        //        worksheet.Cells["A2:K2"].Value = "BÁO CÁO DOANH THU BÁN THẺ";
        //        worksheet.Cells["A2:K2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells["A2:K2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //        worksheet.Cells["A2:K2"].Style.Font.Bold = true;
        //        worksheet.Cells["A2:K2"].Style.Font.Size = 12;
        //        worksheet.Cells["A2:K2"].Merge = true;
        //        worksheet.Cells["A2:K2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        worksheet.Cells["A3:H3"].Value =
        //            $"({httpRequest.QueryString["fromDate"]} - {httpRequest.QueryString["toDate"]})";
        //        worksheet.Cells["A3:H3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells["A3:H3"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //        worksheet.Cells["A3:H3"].Style.Font.Italic = true;
        //        worksheet.Cells["A3:H3"].Merge = true;
        //        worksheet.Cells["A3:H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        worksheet.Cells["A9:C9"].Value = "Tổng số thẻ đã phát hành:";
        //        worksheet.Cells["A9:C9"].Style.Font.Bold = true;
        //        worksheet.Cells["A9:C9"].Merge = true;
        //        worksheet.Cells["D9:E9"].Value = grid.Count(u => u.EventCode == "00");
        //        worksheet.Cells["D9:E9"].Merge = true;

        //        worksheet.Cells["A10:C10"].Value = "Tổng số tiền đã nạp:";
        //        worksheet.Cells["A10:C10"].Style.Font.Bold = true;
        //        worksheet.Cells["A10:C10"].Merge = true;
        //        worksheet.Cells["D10:E10"].Value = grid.Where(u => u.EventCode == "01").Sum(c => c.Value);
        //        worksheet.Cells["D10:E10"].Style.Numberformat.Format = "#,##";
        //        worksheet.Cells["D10:E10"].Merge = true;

        //        worksheet.Cells["A11:C11"].Value = "Tổng số tiền đã rút:";
        //        worksheet.Cells["A11:C11"].Style.Font.Bold = true;
        //        worksheet.Cells["A11:C11"].Merge = true;
        //        worksheet.Cells["D11:E11"].Value = -grid.Where(u => u.EventCode == "02").Sum(c => c.Value);
        //        worksheet.Cells["D11:E11"].Style.Numberformat.Format = "#,##";
        //        worksheet.Cells["D11:E11"].Merge = true;

        //        worksheet.Cells["A13:H13"].Value = "Danh sách giao dịch chi tiết";
        //        worksheet.Cells["A13:H13"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        worksheet.Cells["A13:H13"].Merge = true;

        //        var row = 15;
        //        for (var i = 0; i < properties.Length; i++)
        //        {
        //            worksheet.Cells[row, i + 1].Value = properties[i];
        //            worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }
        //        worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
        //        row++;
        //        var stt = 1;
        //        foreach (var item in grid)
        //        {
        //            var col = 1;
        //            worksheet.Cells[row, col].Value = stt++;
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.ID;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.Date;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "dd/MM/yyyy";

        //            worksheet.Cells[row, col].Value = item.OwnerCode;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CardNumber;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Event;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Value;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells[row, col].Value = item.Balance;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells[row, col].Value = item.Object;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Area;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";
        //            worksheet.Cells[row, col].Value = item.IsBankTransfer==true ? "Chuyển khoản" : "Tiền mặt";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "@";
        //            row++;
        //        }
        //        var nameexcel = "doanh thu ban the" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        //        xlPackage.Workbook.Properties.Title = $"{nameexcel} reports";
        //        xlPackage.Workbook.Properties.Author = "Admin-IT";
        //        xlPackage.Workbook.Properties.Subject = $"{""} reports";
        //        xlPackage.Workbook.Properties.Category = "Report";
        //        xlPackage.Workbook.Properties.Company = "NetPos";
        //        xlPackage.Save();
        //    }
        //}

        //public static void BCDoanhThuBanHang(string filePath, List<EventItem> grid, HttpRequestBase httpRequest)
        //{
        //    var newFile = new FileInfo(filePath);
        //    using (var xlPackage = new ExcelPackage(newFile))
        //    {
        //        var worksheet = xlPackage.Workbook.Worksheets.Add("DTBH");
        //        xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
        //        var properties = new[]
        //            {
        //                "STT",
        //                "Mã giao dịch",
        //                "Thời gian",
        //                "Mã HS",
        //                "Mã thẻ",
        //                "Loại giao dịch",
        //                "Số tiền",
        //                "Số dư",
        //                "Đối tượng",
        //                "Khu vực"
        //            };
        //        worksheet.Cells["A2:J2"].Value = "BÁO CÁO DOANH THU BÁN THẺ";
        //        worksheet.Cells["A2:J2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells["A2:J2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //        worksheet.Cells["A2:J2"].Style.Font.Bold = true;
        //        worksheet.Cells["A2:J2"].Style.Font.Size = 12;
        //        worksheet.Cells["A2:J2"].Merge = true;
        //        worksheet.Cells["A2:J2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        worksheet.Cells["A3:H3"].Value =
        //            $"({httpRequest.QueryString["fromDate"]} - {httpRequest.QueryString["toDate"]})";
        //        worksheet.Cells["A3:H3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells["A3:H3"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //        worksheet.Cells["A3:H3"].Style.Font.Italic = true;
        //        worksheet.Cells["A3:H3"].Merge = true;
        //        worksheet.Cells["A3:H3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        //worksheet.Cells["A6:B6"].Value = "Khu vực:";
        //        //worksheet.Cells["C5:D5"].Value = modelItem.AreaName;
        //        //worksheet.Cells["A7:B7"].Value = "Đầu đọc thẻ:";
        //        //worksheet.Cells["C5:D5"].Value = modelItem.ObjectName;
        //        //worksheet.Cells["A5:B5"].Merge = true;
        //        //worksheet.Cells["C5:D5"].Merge = true;
        //        //worksheet.Cells["A6:B6"].Merge = true;
        //        //worksheet.Cells["C5:D5"].Merge = true;
        //        //worksheet.Cells["A7:B7"].Merge = true;
        //        //worksheet.Cells["C5:D5"].Merge = true;

        //        worksheet.Cells["A9:C9"].Value = "Tổng doanh thu bán hàng:";
        //        worksheet.Cells["A9:C9"].Style.Font.Bold = true;
        //        worksheet.Cells["A9:C9"].Merge = true;
        //        worksheet.Cells["D9:E9"].Value = -grid.Where(u => u.EventCode == "03").Sum(c => c.Value);
        //        worksheet.Cells["D9:E9"].Style.Numberformat.Format = "#,##";
        //        worksheet.Cells["D9:E9"].Merge = true;

        //        worksheet.Cells["A10:C10"].Value = "Tổng số giao dịch:";
        //        worksheet.Cells["A10:C10"].Style.Font.Bold = true;
        //        worksheet.Cells["A10:C10"].Merge = true;
        //        worksheet.Cells["D10:E10"].Value = grid.Count;
        //        worksheet.Cells["D10:E10"].Style.Numberformat.Format = "#,##";
        //        worksheet.Cells["D10:E10"].Merge = true;

        //        worksheet.Cells["A12:H12"].Value = "Danh sách giao dịch chi tiết";
        //        worksheet.Cells["A12:H12"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        worksheet.Cells["A12:H12"].Merge = true;

        //        var row = 14;
        //        for (var i = 0; i < properties.Length; i++)
        //        {
        //            worksheet.Cells[row, i + 1].Value = properties[i];
        //            worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }
        //        worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
        //        row++;
        //        var stt = 1;
        //        foreach (var item in grid)
        //        {
        //            var col = 1;
        //            worksheet.Cells[row, col].Value = stt++;
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.ID;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.Date;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "dd/MM/yyyy";

        //            worksheet.Cells[row, col].Value = item.CardNumber;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.OwnerCode;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Event;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Value;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells[row, col].Value = item.Balance;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells[row, col].Value = item.Object;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Area;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "@";

        //            row++;
        //        }
        //        var nameexcel = "doanh thu ban the" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        //        xlPackage.Workbook.Properties.Title = $"{nameexcel} reports";
        //        xlPackage.Workbook.Properties.Author = "Admin-IT";
        //        xlPackage.Workbook.Properties.Subject = $"{""} reports";
        //        xlPackage.Workbook.Properties.Category = "Report";
        //        xlPackage.Workbook.Properties.Company = "NetPos";
        //        xlPackage.Save();
        //    }
        //}

        //public static void BCSuatAnNV(HttpRequestBase httpRequest, string filePath, List<PersonnelItem> grid, List<PersonnelItem> lst)
        //{
        //    var now = DateTime.Now;
        //    var from = httpRequest["fromDate"];
        //    var to = httpRequest["toDate"];
        //    var fromDate = !string.IsNullOrEmpty(from)
        //    ? ConvertUtil.ToDate(from)
        //    : new DateTime(now.Year, now.Month, 1);
        //    var toDate = ConvertUtil.ToDate(to);

        //    var newFile = new FileInfo(filePath);
        //    using (var xlPackage = new ExcelPackage(newFile))
        //    {
        //        var worksheet = xlPackage.Workbook.Worksheets.Add("DTBH");
        //        xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
        //        var properties = new[]
        //            {
        //                "STT",
        //                "Mã KH",
        //                "Mã thẻ",
        //                "Tên KH",
        //                "Số bữa ăn",
        //                "Loại thẻ",
        //            };

        //        var row = 3;
        //        var leng = properties.Length;
        //        for (var i = 0; i < leng; i++)
        //        {
        //            worksheet.Cells[row, i + 1].Value = properties[i];
        //            worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }
        //        for (var day = fromDate.Date; day.Date <= toDate.Date; day = day.AddDays(1))
        //        {
        //            leng++;
        //            worksheet.Cells[row, leng].Value = day.ToString("dd/MM/yyyy");
        //            worksheet.Cells[row, leng].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, leng].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, leng].Style.Font.Bold = true;
        //            worksheet.Cells[row, leng].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            worksheet.Cells[row, leng].Style.Numberformat.Format = "@";
        //        }
        //        worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
        //        row++;
        //        var stt = 1;
        //        foreach (var item in grid)
        //        {
        //            var col = 1;
        //            worksheet.Cells[row, col].Value = stt++;
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.CustomerID;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "@";
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.CardNumber;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CustomerName;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Totals;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "0";

        //            worksheet.Cells[row, col].Value = "Thẻ nhân viên";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "@";
        //            leng = properties.Length;
        //            for (var day = fromDate.Date; day.Date <= toDate.Date; day = day.AddDays(1))
        //            {
        //                leng++;
        //                var itemnew = lst.FirstOrDefault(c => c.Date.Value.Date == day && c.CardNumber == item.CardNumber);
        //                var total = itemnew == null ? 0 : itemnew.Totals;
        //                worksheet.Cells[row, leng].Value = total;
        //                worksheet.Cells[row, leng].Style.Numberformat.Format = "0";
        //                if (!(total > 1)) continue;
        //                worksheet.Cells[row, leng].Style.Font.Bold = true;
        //                worksheet.Cells[row, leng].Style.Font.Color.SetColor(Color.Red);
        //            }

        //            row++;
        //        }
        //        var nameexcel = "bao cao suat an nv" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        //        xlPackage.Workbook.Properties.Title = $"{nameexcel} reports";
        //        xlPackage.Workbook.Properties.Author = "Admin-IT";
        //        xlPackage.Workbook.Properties.Subject = $"{""} reports";
        //        xlPackage.Workbook.Properties.Category = "Report";
        //        xlPackage.Workbook.Properties.Company = "NetPos";
        //        xlPackage.Save();
        //    }
        //}

        //public static void ChiTietThe(string filePath, CustomerItem customerItem, List<GiaoDichItem> grid, string starDate, string endDate)
        //{
        //    try
        //    {


        //        var newFile = new FileInfo(filePath);
        //        using (var xlPackage = new ExcelPackage(newFile))
        //        {
        //            var worksheet = xlPackage.Workbook.Worksheets.Add("Chi tiet the");
        //            xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
        //            var properties = new[]
        //            {
        //                "STT",
        //                "Miêu tả",
        //                "Thời gian",
        //                "Thanh toán",
        //                "Đối tượng"
        //            };
        //            worksheet.Cells["A2:B2"].Value = "Mã khách hàng:";
        //            worksheet.Cells["A2:B2"].Merge = true;
        //            worksheet.Cells["C2"].Value = customerItem.CustomerID;

        //            worksheet.Cells["A3:B3"].Value = "Tên khách hàng:";
        //            worksheet.Cells["A3:B3"].Merge = true;
        //            worksheet.Cells["C3"].Value = customerItem.CustomerName;

        //            worksheet.Cells["A4:B4"].Value = "Ngày sinh:";
        //            worksheet.Cells["A4:B4"].Merge = true;
        //            worksheet.Cells["C4"].Value = customerItem.BirthDate;

        //            worksheet.Cells["A5:B5"].Value = "Năm học:";
        //            worksheet.Cells["A5:B5"].Merge = true;
        //            worksheet.Cells["C5"].Value = customerItem.SchoolYear;

        //            worksheet.Cells["A6:B6"].Value = "Lớp:";
        //            worksheet.Cells["A6:B6"].Merge = true;
        //            worksheet.Cells["C6"].Value = customerItem.CustomerClass;

        //            worksheet.Cells["E2:F2"].Value = "Mã thẻ:";
        //            worksheet.Cells["E2:F2"].Merge = true;
        //            worksheet.Cells["G2"].Value = customerItem.CardNumber;

        //            worksheet.Cells["E3:F3"].Value = "Số dư:";
        //            worksheet.Cells["E3:F3"].Merge = true;
        //            worksheet.Cells["G3"].Value = customerItem.Balance;
        //            worksheet.Cells["G3"].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells["E4:F4"].Value = "Loại thẻ:";
        //            worksheet.Cells["E4:F4"].Merge = true;
        //            worksheet.Cells["G4"].Value = customerItem.CardType;

        //            worksheet.Cells["E5:F5"].Value = "Trạng thái:";
        //            worksheet.Cells["E5:F5"].Merge = true;
        //            worksheet.Cells["G5"].Value = customerItem.CardStatus;

        //            worksheet.Cells["E6:F6"].Value = "Ngày phát hành:";
        //            worksheet.Cells["E6:F6"].Merge = true;
        //            worksheet.Cells["G6"].Value = customerItem.DateIssue;

        //            worksheet.Cells["A8:G8"].Value = "CÁC GIAO DỊCH";
        //            worksheet.Cells["A8:G8"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells["A8:G8"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //            worksheet.Cells["A8:G8"].Style.Font.Bold = true;
        //            worksheet.Cells["A8:G8"].Style.Font.Size = 12;
        //            worksheet.Cells["A8:G8"].Merge = true;
        //            worksheet.Cells["A8:G8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells["A9:G9"].Value = "(" + starDate + " - " + endDate + ")";
        //            worksheet.Cells["A9:G9"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells["A9:G9"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //            worksheet.Cells["A9:G9"].Style.Font.Italic = true;
        //            worksheet.Cells["A9:G9"].Merge = true;
        //            worksheet.Cells["A9:G9"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            var row = 11;
        //            for (var i = 0; i < properties.Length; i++)
        //            {
        //                worksheet.Cells[row, i + 1].Value = properties[i];
        //                worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //                worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //                worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //                worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //            }
        //            worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
        //            row++;
        //            var stt = 1;
        //            foreach (var item in grid)
        //            {
        //                var col = 1;
        //                worksheet.Cells[row, col].Value = stt++;
        //                worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //                worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //                worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //                worksheet.Cells[row, col].Value = item.Event;
        //                worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //                worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //                worksheet.Cells[row, col].Value = item.Date;
        //                worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //                worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //                worksheet.Cells[row, col].Value = item.Value;
        //                worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //                worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //                worksheet.Cells[row, col].Value = item.Object;
        //                worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //                worksheet.Cells[row, col].Style.Numberformat.Format = "@";

        //                row++;
        //            }
        //            var nameexcel = "Chi_tiet_the_" + DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
        //            xlPackage.Workbook.Properties.Title = $"{nameexcel} reports";
        //            xlPackage.Workbook.Properties.Author = "Admin-IT";
        //            xlPackage.Workbook.Properties.Subject = $"{""} reports";
        //            xlPackage.Workbook.Properties.Category = "Report Manager";
        //            xlPackage.Workbook.Properties.Company = "Report Manager";
        //            xlPackage.Save();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log2File.LogExceptionToFile(ex);
        //    }
        //}

        //public static void ExportToBankTransfer(string filePath, List<BankTransferItem> lst)
        //{
        //    var newFile = new FileInfo(filePath);
        //    using (var xlPackage = new ExcelPackage(newFile))
        //    {
        //        var worksheet = xlPackage.Workbook.Worksheets.Add("Order");
        //        xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
        //        var properties = new[]
        //            {
        //                "STT",
        //                "Mã KH",
        //                "Tên KH",
        //                "Số tiền",
        //                "Ngày chuyển khoản",
        //                "Ngày nạp vào thẻ",
        //                "Ghi chú"
        //            };
        //        worksheet.Cells["A1:G2"].Value = "DANH SÁCH NẠP TIỀN QUA NGÂN HÀNG";
        //        worksheet.Cells["A1:G2"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //        worksheet.Cells["A1:G2"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
        //        worksheet.Cells["A1:G2"].Style.Font.Bold = true;
        //        worksheet.Cells["A1:G2"].Style.Font.Size = 12;
        //        worksheet.Cells["A1:G2"].Merge = true;
        //        worksheet.Cells["A1:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //        var row = 4;
        //        for (var i = 0; i < properties.Length; i++)
        //        {
        //            worksheet.Cells[row, i + 1].Value = properties[i];
        //            worksheet.Cells[row, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            worksheet.Cells[row, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
        //            worksheet.Cells[row, i + 1].Style.Font.Bold = true;
        //            worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        }
        //        worksheet.Cells[row, 1, row, properties.Length].AutoFilter = true;
        //        row++;
        //        var stt = 1;
        //        foreach (var item in lst)
        //        {
        //            var col = 1;
        //            worksheet.Cells[row, col].Value = stt++;
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "0";
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        //            worksheet.Cells[row, col].Value = item.CustomerCode;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.CustomerName;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

        //            worksheet.Cells[row, col].Value = item.Deposit;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "#,##";

        //            worksheet.Cells[row, col].Value = item.DepositDate;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "dd/MM/yyyy";

        //            worksheet.Cells[row, col].Value = item.WithdrawDate;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col++].Style.Numberformat.Format = "dd/MM/yyyy";

        //            worksheet.Cells[row, col].Value = item.DepositNotes;
        //            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
        //            worksheet.Cells[row, col].Style.Numberformat.Format = "@";
        //            row++;
        //        }
        //        var nameexcel = "Danh sách nạp tiền qua ngân hàng" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        //        xlPackage.Workbook.Properties.Title = $"{nameexcel} reports";
        //        xlPackage.Workbook.Properties.Author = "Admin-IT";
        //        xlPackage.Workbook.Properties.Subject = $"{""} reports";
        //        xlPackage.Workbook.Properties.Category = "Report";
        //        xlPackage.Workbook.Properties.Company = "NetPos";
        //        xlPackage.Save();
        //    }
        //}

    }
}
