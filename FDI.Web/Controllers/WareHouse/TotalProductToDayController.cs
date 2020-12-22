using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.DA.DA.StorageWarehouse;
using FDI.DA.DA.Supplier;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.GetAPI.Supplier;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Simple.StorageWarehouse;
using FDI.Simple.Supplier;
using FDI.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FDI.Web.Controllers.WareHouse
{

    public class TotalProductToDayController : BaseController
    {
        //
        // GET: /StorageWarehouse/
        private readonly TotalProductToDayAPI _api = new TotalProductToDayAPI();
        readonly StorageWareHouseDA _da = new StorageWareHouseDA("#");
        private readonly DNUserAPI _userAPI = new DNUserAPI();
        readonly DNAgencyAPI _dnAgencyApi = new DNAgencyAPI();
        private readonly SupplierAmountProductApi _supplierAmount = new SupplierAmountProductApi();
        private readonly StorageWarehouseAPI _apiStorageWare = new StorageWarehouseAPI();
        private readonly CategoryAPI _apiCategoryAPI = new CategoryAPI();
        private readonly RequestWareAPI _apiRequestWareAP = new RequestWareAPI();
        private readonly TotalProductToDayAPI _apiTotalProductToDayAPI = new TotalProductToDayAPI();
        public async Task<ActionResult> Index(decimal todayCode, int categoryId)
        {
            //get info detail
            var model = new TotalProductToDayModel();
            model.CategoryModel = _apiCategoryAPI.GetCategoryById(categoryId);
            model.Quantity = await _apiRequestWareAP.GetTotalOrder(todayCode);

            model.QuantityActive = await _apiTotalProductToDayAPI.GetTotalOrder(todayCode);

            return View(model);
        }
        public ActionResult ListItems(decimal todayCode, int categoryId)
        {
            return View(_api.ListItems(Request.Url.Query, todayCode, categoryId));
        }

        public async Task<ActionResult> AjaxForm(decimal todayCode, int categoryId)
        {
            SupplierAmountProductDA _da = new SupplierAmountProductDA();
            var requestWare = await _apiStorageWare.GetRequestWareSummaryByProduct(todayCode, categoryId);
            var lst =  _da.GetSupplierByCategoryId(categoryId);
            ////var lst = await _supplierAmount.GetSupplierByCategoryId(requestWare.CateID.Value);

            //var model = new StorageWarehousingItem();
            ////if (DoAction == ActionType.Edit)
            ////    model = _api.GetStorageWarehousingItem(ArrId.FirstOrDefault());
            //ViewBag.UserID = UserItem.UserId;
            //ViewBag.User = UserItem.UserName;
            //var agent = _dnAgencyApi.GetItemById(UserItem.AgencyID);
            //ViewBag.Wallet = agent.WalletValue;
            //ViewBag.Deposit = UserItem.AgencyDeposit ?? 0;
            //ViewBag.marketId = UserItem.MarketID;
            //ViewBag.areaId = UserItem.AreaID;
            //ViewBag.listtime = TypeTime.Hours(DateTime.Today.AddDays(1).TotalSeconds(), DateTime.Now.AddHours(12).TotalSeconds());
            //ViewBag.Action = DoAction;
            return View(lst);
        }

        [HttpPost]
        public async Task<ActionResult> AddSupplier(RequestWareSupplierRequest[] request)
        {
            var result = await _api.AddSupplier(request);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> SaveSupplier(RequestWareSupplierRequest request)
        {
            var result = await _api.SaveSupplier(request);
            return Json(result);
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
                //case ActionType.Add:
                //  if (_api.Add(url, UserItem.AgencyID, CodeLogin()) == 0)
                //  {
                //      msg.Erros = true;
                //      msg.Message = "Có lỗi xảy ra.";
                //  }
                //    break;
                //case ActionType.Edit:
                //    if (_api.Update(url, UserItem.AgencyID, CodeLogin()) == 0)
                //    {
                //        msg.Erros = true;
                //        msg.Message = "Có lỗi xảy ra.";
                //    }
                //    break;
                //case ActionType.Delete:
                //    var lst1 = string.Join(",", ArrId);
                //    if (_api.Delete(lst1) == 0)
                //    {
                //        msg.Erros = true;
                //        msg.Message = "Có lỗi xảy ra.";
                //    }
                //    break;
                //case ActionType.UserModule:
                //    if (_api.Imported(url, UserItem.AgencyID, CodeLogin()) == 0)
                //    {
                //        msg.Erros = true;
                //        msg.Message = "Có lỗi xảy ra.";
                //    }
                //    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không có quyền thực hiện chứ năng này.";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
