using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.CORE;
using FDI.Simple;
using OfficeOpenXml;
using OfficeOpenXml.Style;
namespace FDI.Utils
{
    public class Excels
    {
        public static void ExportListCard(string filePath, ModelContentVoteItem lst)
        {
            var newFile = new FileInfo(filePath);
            using (var xlPackage = new ExcelPackage(newFile))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("LSGD");
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                var properties = new[]
                    {
                        "STT",
                        "Nhân viên đánh giá",
                        "Tiêu chí",
                        "Mức độ",
                        "Giá trị",
                        "NV bị đánh giá",
                        "Ngày đánh giá",
                        "Nội dung"
                    };
                worksheet.Cells["A1:H1"].Value = "DANH SÁCH ĐÁNH GIÁ CỦA NHÂN VIÊN";
                worksheet.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                worksheet.Cells["A1:H1"].Style.Font.Bold = true;
                worksheet.Cells["A1:H1"].Style.Font.Size = 16;
                worksheet.Cells["A1:H1"].Merge = true;
                worksheet.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var row = 7;
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
                foreach (var item in lst.ListItems)
                {
                    var col = 1;
                    worksheet.Cells[row, col].Value = stt++;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "0";
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[row, col].Value = item.US_NVDG;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    worksheet.Cells[row, col++].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    worksheet.Cells[row, col].Value = item.VoteName;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    worksheet.Cells[row, col++].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    worksheet.Cells[row, col].Value = item.LevelName;
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

                    worksheet.Cells[row, col].Value = item.Value;
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

                    worksheet.Cells[row, col].Value = item.US_NVBDG;
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

                    worksheet.Cells[row, col].Value = item.DateEvaluation.DecimalToString("dd/MM/yyyy");
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

                    worksheet.Cells[row, col].Value = item.Content;
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    row++;
                }
                var nameexcel = "Danh sách đánh giá nhân viên" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                xlPackage.Workbook.Properties.Title = string.Format("{0} reports", nameexcel);
                xlPackage.Workbook.Properties.Author = "Admin-IT";
                xlPackage.Workbook.Properties.Subject = "reports";
                xlPackage.Workbook.Properties.Category = "Report";
                xlPackage.Workbook.Properties.Company = "SSC";
                xlPackage.Save();
            }
        }
        public static void ExportListValueDetail(string filePath, ModuleShopProductValueItem lst,string date)
        {
            var newFile = new FileInfo(filePath);
            using (var xlPackage = new ExcelPackage(newFile))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("LSGD");
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                var properties = new[]
                    {
                        "STT",
                        "Tên nguyên liệu",
                        "Đơn vị",
                        "Số lượng",
                        "Đã Xuất",
                    };
                worksheet.Cells["A1:H1"].Value = "DANH SÁCH NGUYÊN LIỆU SỬ DỤNG TRONG NGÀY " + DateTime.Parse(date).ToString("dd/MM/yyyy");
                worksheet.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                worksheet.Cells["A1:H1"].Style.Font.Bold = true;
                worksheet.Cells["A1:H1"].Style.Font.Size = 16;
                worksheet.Cells["A1:H1"].Merge = true;
                worksheet.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var row = 6;
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
                foreach (var item in lst.ListItems)
                {
                    var ex = lst.LstImportItem.Where(c => c.ValueId == item.ID).ToList();
                    var col = 1;
                    worksheet.Cells[row, col].Value = stt++;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "0";
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[row, col].Value = item.Name;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    worksheet.Cells[row, col++].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    worksheet.Cells[row, col].Value = item.UnitName;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    worksheet.Cells[row, col++].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    worksheet.Cells[row, col].Value = item.Quantity.Quantity("0:0.####");
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

                    worksheet.Cells[row, col].Value = ex.Sum(c => c.Quantity).Quantity("0:0.####");
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    row++;
                }
                var nameexcel = "Danh sách nguyên liệu sử dụng ngày" + DateTime.Parse(date).ToString("dd/MM/yyyy");
                xlPackage.Workbook.Properties.Title = string.Format("{0} reports", nameexcel);
                xlPackage.Workbook.Properties.Author = "Admin-IT";
                xlPackage.Workbook.Properties.Subject = "reports";
                xlPackage.Workbook.Properties.Category = "Report";
                xlPackage.Workbook.Properties.Company = "SSC";
                xlPackage.Save();
            }
        }
        public static void ExportListOrderDetail(string filePath, ModelProductExportItem lst, string date)
        {
            var newFile = new FileInfo(filePath);
            using (var xlPackage = new ExcelPackage(newFile))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("LSGD");
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                var properties = new[]
                    {
                        "STT",
                        "Tên sản phẩm",
                        "Đơn vị",
                        "Giá bán",
                        "Số lượng",
                        "Đã Xuất",
                    };
                worksheet.Cells["A1:H1"].Value = "DANH SÁCH SẢN PHẨM BÁN TRONG NGÀY " + DateTime.Parse(date).ToString("dd/MM/yyyy");
                worksheet.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 255));
                worksheet.Cells["A1:H1"].Style.Font.Bold = true;
                worksheet.Cells["A1:H1"].Style.Font.Size = 16;
                worksheet.Cells["A1:H1"].Merge = true;
                worksheet.Cells["A1:H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var row = 7;
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
                foreach (var item in lst.ListItem)
                {
                    var ex = lst.LstExportItem.Where(c => c.ProductID == item.ID).ToList();
                    var col = 1;
                    worksheet.Cells[row, col].Value = stt++;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "0";
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells[row, col].Value = item.Name;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    worksheet.Cells[row, col++].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    worksheet.Cells[row, col].Value = item.UnitName;
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    worksheet.Cells[row, col++].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    worksheet.Cells[row, col].Value = item.Price.Money();
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    worksheet.Cells[row, col++].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);

                    worksheet.Cells[row, col].Value = item.Quantity.Quantity("0:0.####");
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col++].Style.Numberformat.Format = "@";

                    worksheet.Cells[row, col].Value = ex.Sum(c => c.Quantity).Quantity("0:0.####");
                    worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.Black);
                    worksheet.Cells[row, col].Style.Numberformat.Format = "@";
                    row++;
                }
                var nameexcel = "Danh sách sản phaamrm bán trong ngày" + DateTime.Parse(date).ToString("dd/MM/yyyy");
                xlPackage.Workbook.Properties.Title = string.Format("{0} reports", nameexcel);
                xlPackage.Workbook.Properties.Author = "Admin-IT";
                xlPackage.Workbook.Properties.Subject = "reports";
                xlPackage.Workbook.Properties.Category = "Report";
                xlPackage.Workbook.Properties.Company = "SSC";
                xlPackage.Save();
            }
        }
    }
}
