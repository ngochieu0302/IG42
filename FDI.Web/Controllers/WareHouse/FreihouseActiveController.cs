using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI.StorageWarehouse;
using FDI.Simple;
using FDI.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FDI.Web.Controllers.WareHouse
{
    public class FreihouseActiveController : BaseController
    {
        //
        // GET: /FreihouseActive/
        private readonly StorageFreightWarehouseAPI _api = new StorageFreightWarehouseAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItemsAll(Request.Url.Query));
        }
        //public ActionResult AjaxForm()
        //{
        //    var model = new StorageFreightWarehouseItem();
        //    if (DoAction == ActionType.Edit)
        //        model = _api.GetStorageFreightWarehousesItem(ArrId.FirstOrDefault());
        //    ViewBag.UserID = UserItem.UserId;
        //    ViewBag.User = UserItem.UserName;
        //    ViewBag.Action = DoAction;
        //    return View(model);
        //}
        public ActionResult AjaxView()
        {
            var model = _api.GetStorageFreightWarehousesItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công" };
            switch (DoAction)
            {
                case ActionType.Active:
                    var lstArrId = string.Join(",", ArrId);
                    if (_api.ActiveFrei(lstArrId, UserItem.UserId) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra.";
                    }
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        #region xuất ra file excel
        public ActionResult ProcessExportFile()
        {
            var lst = _api.GetListExcel(UserItem.AgencyID, Request.Url.Query);
            var fileName = string.Format("nhap-kho_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            var filePath = Path.Combine(Request.PhysicalApplicationPath, "File\\ExportImport", fileName);
            var folder = Request.PhysicalApplicationPath + "File\\ExportImport";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            ExportProductsExcel(filePath, lst);
            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "text/xls", fileName);
        }

        public virtual void ExportProductsExcel(string filePath, List<StorageFreightWarehouseItem> lstStorageProduct)
        {
            var newFile = new FileInfo(filePath);

            // ok, we can run the real code of the sample now
            int dem = 0;
            using (var xlPackage = new ExcelPackage(newFile))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 
                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Phiếu nhập kho");
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                //Create Headers and format them
                var properties = new string[]
                    {
                        "STT",
                        "Mã phiếu",
                        "Ngày nhập",
                        "Tổng tiền",
                        "Ghi chú"
                    };
                for (var i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                var row = 2;
                foreach (var item in lstStorageProduct)
                {
                    dem++;
                    int col = 1;
                    worksheet.Cells[row, col].Value = dem;
                    col++;

                    worksheet.Cells[row, col].Value = item.Code;
                    col++;
                    // email
                    worksheet.Cells[row, col].Value = ConvertDate.DecimalToDate(item.DateCreated).ToString("dd/MM/yyyy");
                    col++;
                    // điện thoại
                    worksheet.Cells[row, col].Value = item.TotalPrice.Money();
                    col++;
                    // địa chỉ
                    worksheet.Cells[row, col].Value = item.Note;
                    row++;
                }

                // we had better add some document properties to the spreadsheet 
                // set some core property values
                var nameexcel = "Danh sách nhập kho" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                xlPackage.Workbook.Properties.Title = string.Format("{0} reports", nameexcel);
                xlPackage.Workbook.Properties.Author = "FDI-IT";
                xlPackage.Workbook.Properties.Subject = string.Format("{0} reports", "");
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} orders", _storeInformationSettings.StoreName);
                xlPackage.Workbook.Properties.Category = "Report";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} orders", _storeInformationSettings.StoreName);
                // set some extended property values
                xlPackage.Workbook.Properties.Company = "FDI ";
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(_storeInformationSettings.StoreUrl);
                // save the new spreadsheet
                xlPackage.Save();
            }
        }

        #endregion
    }
}
