using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.StorageWarehouse;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FDI.Web.Controllers.WareHouse
{

    public class StorageWarehouseController : BaseController
    {
        //
        // GET: /StorageWarehouse/
        private readonly StorageWarehouseAPI _api = new StorageWarehouseAPI();
        readonly StorageWareHouseDA _da = new StorageWareHouseDA("#");
        private readonly DNUserAPI _userAPI = new DNUserAPI();
        readonly DNAgencyAPI _dnAgencyApi = new DNAgencyAPI();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult GetUser(int orderId)
        {
            var lst = _userAPI.FindByName("");
            var order = _api.GetStorageWarehousingItem(orderId);

            foreach (var item in lst.Where(item => order.Users.Any(m => m.UserId == item.UserId)))
            {
                item.IsActive = true;
            }

            return View(lst);
        }
        [HttpPost]
        public async Task<ActionResult> AssignUser(UserStorageWarehousingRequest data)
        {
            var result = await _api.AssignUser(data);

            //add log

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxView()
        {
            var model = _api.GetStorageWarehousingItem(ArrId.FirstOrDefault(), UserItem.AgencyID);
            return View(model);
        }

        public ActionResult ChangeListTimebyDate(string date)
        {
            var dt = ConvertUtil.ToDateTime(date);
            var datet = TypeTime.Hours(dt.TotalSeconds(), DateTime.Now.AddHours(12).TotalSeconds());
            return Json(datet, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxForm()
        {
            var model = new StorageWarehousingItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetStorageWarehousingItem(ArrId.FirstOrDefault());
            ViewBag.UserID = UserItem.UserId;
            ViewBag.User = UserItem.UserName;
            var agent = _dnAgencyApi.GetItemById(UserItem.AgencyID);
            ViewBag.Wallet = agent.WalletValue;
            ViewBag.Deposit = UserItem.AgencyDeposit ?? 0;
            ViewBag.marketId = UserItem.MarketID;
            ViewBag.areaId = UserItem.AreaID;
            ViewBag.listtime = TypeTime.Hours(DateTime.Today.AddDays(1).TotalSeconds(), DateTime.Now.AddHours(12).TotalSeconds());
            ViewBag.Action = DoAction;

            SupplierDA _supplierDa = new SupplierDA();

            return View(model);
        }

        public ActionResult AjaxFormImport()
        {
            var model = _api.GetStorageWarehousingItem(ArrId.FirstOrDefault());
            ViewBag.UserID = UserItem.UserId;
            ViewBag.User = UserItem.UserName;
            ViewBag.Action = DoAction;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công" };
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            switch (DoAction)
            {
                case ActionType.Add:
                    if (_api.Add(url, UserItem.AgencyID, CodeLogin()) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra.";
                    }
                    break;
                case ActionType.Edit:
                    if (_api.Update(url, UserItem.AgencyID, CodeLogin()) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra.";
                    }
                    break;
                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    if (_api.Delete(lst1) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra.";
                    }
                    break;
                case ActionType.UserModule:
                    if (_api.Imported(url, UserItem.AgencyID, CodeLogin()) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra.";
                    }
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không có quyền thực hiện chứ năng này.";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoProduct()
        {
            var query = Request["query"];
            var type = Request["type"] ?? "0";
            var ltsResults = _api.GetListAutoProductValue(query, 10, UserItem.AgencyID, int.Parse(type));
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
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

        public virtual void ExportProductsExcel(string filePath, List<StorageWarehousingItem> lstStorageProduct)
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

        [HttpPost]
        public async Task<ActionResult> ChangeStatus(int orderId, int status)
        {
            var result = await _api.ChangeStatus(orderId, status);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> GetRequestWareSummaryByProduct(decimal today, int productId)
        {
            var result = await _api.GetRequestWareSummaryByProduct(today, productId);
            return Json(result);
        }

        public async Task<ActionResult> AddSupplier(RequestWareSupplierRequest request)
        {
            var requestWare = _da.GetRequestWareById(request.RequestWareId);
            if (requestWare == null)
            {
                return Json(new JsonMessage() { Erros = true, Message = "Request not exits" }, JsonRequestBehavior.AllowGet);
            }

            //remove nha cung cap
            if (request.SupplierId == 0)
            {
                var requestSuppliers = _da.GetAllRequestWareByRequestWareId(requestWare.GID);
                foreach (var dnRequestWareSupplier in requestSuppliers)
                {
                    dnRequestWareSupplier.IsDelete = true;
                }

                _da.Save();
                return Json(new JsonMessage(false, "Đã bỏ chọn NCC"));
            }

            var requestWareSupplier = _da.GetRequestWareBySupplier(request.RequestWareId, request.SupplierId);
            if (requestWareSupplier != null)
            {
                return Json(new JsonMessage() { Erros = true, Message = requestWareSupplier.DN_Supplier.Name + " đã được chọn" }, JsonRequestBehavior.AllowGet);
            }

            var item = new DN_RequestWareSupplier()
            {
                RequestWareId = request.RequestWareId,
                SupplierId = request.SupplierId,
                Quantity = requestWare.Quantity??0,
                IsDelete = false
            };

            _da.AddRequestWareSupplier(item);


            _da.Save();
            return Json(new JsonMessage(false, "Đã thêm NCC"), JsonRequestBehavior.AllowGet);
        }
    }
}
