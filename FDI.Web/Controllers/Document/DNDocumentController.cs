using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.DA.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FDI.Web.Controllers
{
    public class DNDocumentController : BaseController
    {
        private readonly DNDrawerAPI _dnDrawerApi = new DNDrawerAPI();
        private readonly DNDocumentAPI _documentApi = new DNDocumentAPI();
        private readonly DNDocumentFilesAPI _documentFilesApi = new DNDocumentFilesAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        readonly TemplateDocumentDA _templateDocumentDa = new TemplateDocumentDA();
        readonly DNUserAPI _dnUserApi = new DNUserAPI();
        readonly SupplieAPI _supplieApi = new SupplieAPI();
        
        public ActionResult Index()
        {
            var model = new ModelDocumentItem
            {
                 ListDrawerItems= _dnDrawerApi.GetListSimple(),
            };
            return View(model);
        }
        public ActionResult ListItems()
        {
            ViewBag.isadmin = IsAdmin;
            return View(_documentApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
           var model = _documentApi.GetItemsByID(ArrId.FirstOrDefault());
           return View(model);
        }
        #region xuất ra file excel
        public ActionResult ProcessExportFile()
        {
            var lst = _documentApi.ExcelDocument(UserItem.AgencyID, Request.Url.Query);
            var fileName = string.Format("Tai-lieu-van-ban_{0}.xlsx", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
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

        public virtual void ExportProductsExcel(string filePath, ModelDocumentItem modelDocument)
        {
            var newFile = new FileInfo(filePath);

            // ok, we can run the real code of the sample now
            int dem = 0;
            using (var xlPackage = new ExcelPackage(newFile))
            {
                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 
                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Document");
                xlPackage.Workbook.CalcMode = ExcelCalcMode.Manual;
                //Create Headers and format them
                var properties = new string[]
                    {
                        "STT",
                        "Thuộc tầng",
                        "Phòng",
                        "Tủ đựng tài liệu",
                        "Ngăn đựng tài liệu",
                        "Tên tài liệu",
                        "Số/Kí hiệu",
                        //"Loại tài liệu",
                        "Nơi ban hành",
                        //"Ngày ban hành",
                        "Người ký"
                    };
                for (var i = 0; i < properties.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                var row = 2;
                foreach (var item in modelDocument.ListItem)
                {
                    dem++;
                    int col = 1;
                    worksheet.Cells[row, col].Value = dem;
                    col++;
                    // tầng
                    worksheet.Cells[row, col].Value = item.LevelName;
                    col++;
                    // phòng
                    worksheet.Cells[row, col].Value = item.RoomName;
                    col++;
                    // tủ tài liệu
                    worksheet.Cells[row, col].Value = item.CabinetName;
                    col++;
                    // ngăn tài liệu
                    worksheet.Cells[row, col].Value = item.DrawerName;
                    col++;
                    // tên tài liệu
                    worksheet.Cells[row, col].Value = item.Name;
                    col++;
                    // số/kí hiệu
                    worksheet.Cells[row, col].Value = item.Numberbill;
                    col++;
                    // loại tài liệu
                    //worksheet.Cells[row, col].Value = item.ListCategoryItem != null ?  string.Join(",", item.ListCategoryItem.Select(m=> m.Name)) : string.Empty;
                    //col++;
                    // Nơi ban hành
                    worksheet.Cells[row, col].Value = item.NoiBanHanh;
                    col++;
                    // ngày ban hành
                    //worksheet.Cells[row, col].Value = ConvertDate.DecimalToDate(item.PublishedDate).ToString("dd/MM/yyyy");
                    //col++;
                    // người ký
                    worksheet.Cells[row, col].Value = item.NguoiKy;
                    row++;
                }

                // we had better add some document properties to the spreadsheet 
                // set some core property values
                var nameexcel = "Tài liệu văn bản " + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
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

        public ActionResult AjaxForm()
        {
            var model = new DocumentItem();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            ViewBag.Drawer = _dnDrawerApi.GetListSimple();
            ViewBag.agencyId = _agencyApi.GetAll(UserItem.EnterprisesID ?? 0);
            ViewBag.UserId = _dnUserApi.GetAll(UserItem.AgencyID);
            ViewBag.SupId = _supplieApi.GetList(UserItem.AgencyID);
            if (DoAction == ActionType.Edit)
            {
                model = _documentApi.GetItemsByID(ArrId.FirstOrDefault());
            }
            return View(model);
        }
        public ActionResult UploadFiles()
        {
            var item = new FileObj();
            foreach (string file in Request.Files)
            {
                var hpf = Request.Files[file];
                var fileNameRoot = hpf != null ? hpf.FileName : string.Empty;
                if (hpf != null && hpf.ContentLength == 0)
                    continue;
                if (hpf != null)
                {
                    var urlFolder = ConfigData.TempFolder;
                    if (!Directory.Exists(urlFolder))
                        Directory.CreateDirectory(urlFolder);
                    if (fileNameRoot.Length > 1)
                    {
                        var fileLocal = fileNameRoot.Split('.');
                        var fileName = FomatString.Slug(fileLocal[0]) + "-" + DateTime.Now.ToString("MMddHHmmss") + "." + fileLocal[1];
                        var savedFileName = Path.Combine((urlFolder), Path.GetFileName(fileName));
                        hpf.SaveAs(savedFileName);
                        item = new FileObj
                        {
                            Name = fileName,
                            Type = fileLocal[0].SubString(14),
                            Icon = "/Images/Icons/file/" + fileLocal[1] + ".png"
                        };
                    }
                }
            }
            return Json(item);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công !" };
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            var lstFile = Request["lstFile"];
            var lstDocument = new List<FilesItem>();
            switch (DoAction)
            {
                case ActionType.Add:
                    if (!string.IsNullOrEmpty(lstFile))
                    {
                        lstDocument = Utility.UploadDocument(lstFile);
                    }
                    var jsonAdd = new JavaScriptSerializer().Serialize(lstDocument);
                    var json1 = _documentFilesApi.AddList(jsonAdd);
                    if (_documentApi.Add(string.Join(",", json1), url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    break;
                case ActionType.Edit:
                    if (!string.IsNullOrEmpty(lstFile))
                    {
                        lstDocument = Utility.UploadDocument(lstFile);
                    }
                    var jsonUpdate = new JavaScriptSerializer().Serialize(lstDocument);
                    var json2 = _documentFilesApi.AddList(jsonUpdate);
                    if (_documentApi.Update(string.Join(",", json2), url) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    break;

                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    _documentApi.Delete(lst1,UserItem.UserName);
                    msg.Message = "Cập nhật thành công.";
                    break;
                case ActionType.Active:
                    var lstArrId = string.Join(",", ArrId);
                    if (_documentApi.Active(lstArrId, UserItem.UserId) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra.";
                    }
                    break;
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BinDataToPattern(int id, int type)
        {
            var model = _templateDocumentDa.GetItemByIDAndIDDoc(id, type);
            return View("BinDataToPattern", null, model);
        }
    }
}
