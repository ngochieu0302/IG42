using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA.DN_Sales;
using FDI.DA.DA.StorageWarehouse;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.GetAPI.Supplier;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FDI.Web.Controllers.WareHouse
{
    public class WarehouseActiveController : BaseController
    {
        //
        // GET: /WarehouseActive/
        private readonly StorageWarehouseAPI _api = new StorageWarehouseAPI();
        private readonly SupplieAPI _supplieApi = new SupplieAPI();
        readonly StorageWareHouseDA _da = new StorageWareHouseDA("#");
        private readonly SupplierAmountProductApi _supplierAmount = new SupplierAmountProductApi();
        private readonly TotalProductToDayDA _productToDayDa = new TotalProductToDayDA();


        public ActionResult Index()
        {
            if (!int.TryParse(Request.QueryString["orderId"], out var orderId))
            {
                return Redirect("/StorageWarehouse");
            }
            var item = _api.GetStorageWarehousingItem(orderId, UserItem.AgencyID);

            return View(item);
        }

        public ActionResult DetailOrder(int orderId)
        {
            var item = _da.GetStorageWarehousingItem(orderId);
            return View(item);
        }
        public ActionResult ProductConfirmedWithSupplier(int orderId)
        {
           
            var item = _da.GetStorageWarehousingItem(orderId);
            if (item.DateRecive == null)
            {
                return View(new List<TotalProductToDayItem>());
            }

            // get productid in order
            var productids = _da.GetProductInOrder(orderId);
            var products = _productToDayDa.GetListByToDay(item.DateRecive.Value, productids);

            
            // lay tong so don hang đã chốt với nhà cung câp

            // lay tong số đơn hang đã chốt với khách hàng

            var productConfirmCustomer = _da.GetProductSummaryConfirm(productids, item.DateRecive.Value);
            foreach (var totalProductToDayItem in productConfirmCustomer)
            {
                var product = products.FirstOrDefault(m => m.ProductId == totalProductToDayItem.ProductId);
                if (product != null)
                {
                    product.Quantity -= totalProductToDayItem.Quantity;
                }
            }

            return View(products);
        }

        public ActionResult ListItems()
        {
            var lst = _api.ListItemsByOrderId(UserItem.AgencyID, int.Parse(Request.QueryString["orderId"]));
            lst.StorageWare = _api.GetStorageWarehousingItem(int.Parse(Request.QueryString["orderId"]));

            return View(lst);
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetStorageWarehousingItem(ArrId.FirstOrDefault());
            return View(model);
        }

        public ActionResult UpdateQuantity(string gid, string quantity)
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công.!" };
            try
            {
                var guidId = string.IsNullOrEmpty(gid) ? Guid.NewGuid() : Guid.Parse(gid);
                var model = _da.GetByIdDetai(guidId);
                
                var q = model.QuantityActive;
                model.QuantityActive = string.IsNullOrEmpty(quantity) ? q : decimal.Parse(quantity);
                model.DateUpdate = DateTime.Now.TotalSeconds();
                model.UserUpdate = UserItem.UserName;
                _da.Save();
            }
            catch (Exception)
            {
                msg = new JsonMessage
                {
                    Erros = true,
                    Message = "Không thể Cập nhật số lượng.!"
                };
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ConfirmAmount(int orderId)
        {
            var msg = new JsonMessage();
            var item = _api.GetStorageWarehousingItem(orderId, UserItem.AgencyID);

            if (ValidateConfirmAmount(item, msg))
            {
                return Json(msg, JsonRequestBehavior.AllowGet);
            };

            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(item.LstImport);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //var response = await client.PostAsync(item.UrlConfirm, data);
            //if (response.IsSuccessStatusCode)
            //{
            var result = _api.ConfirmAmount(orderId, UserItem.AgencyID);
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> MoveProduce(int orderId)
        {
            var msg = new JsonMessage();
            //var item = _api.GetStorageWarehousingItem(orderId, UserItem.AgencyID);

            //if (ValidateConfirmAmount(item, msg))
            //{
            //    return Json(msg, JsonRequestBehavior.AllowGet);
            //};
            var result = await _api.MoveProduce(orderId);
            return Json(result);
        }

        private static bool ValidateConfirmAmount(StorageWarehousingItem item, JsonMessage msg)
        {
            if (item == null)
            {
                msg.Erros = true;
                msg.Message = "Đơn hàng không tồn tại";
            }

            if (item.Status != (int)CORE.StatusWarehouse.Pending)
            {
                msg.Erros = true;
                msg.Message = "Số lượng đã được duyệt";
            }

            var oderNotApproveAmount = item.LstImport.FirstOrDefault(m => m.QuantityActive == null);
            if (oderNotApproveAmount != null)
            {
                msg.Erros = true;
                msg.Message = $"{oderNotApproveAmount.ProductName} chưa được duyệt";
            }

            return msg.Erros;
        }

        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công" };
            //var url = Request.Form.ToString();
            var lstArrId = string.Join(",", ArrId);
            switch (DoAction)
            {
                case ActionType.Delete:
                    msg.MsgID = _api.Delete(lstArrId);
                    if (msg.MsgID != 0)
                    {
                        msg = new JsonMessage
                        {
                            Erros = false,
                            Message = "Xóa đơn đặt hàng thành công.!"
                        };
                    }
                    else
                    {
                        msg = new JsonMessage
                        {
                            Erros = true,
                            Message = "Có lỗi xảy ra.!"
                        };
                    }
                    break;
                case ActionType.Active:
                    var lstId = Request["lstId"] ?? lstArrId;
                    msg = _api.Active(lstId, UserItem.UserId);
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
        public ActionResult ViewCateDetails(int id)
        {
            var model = _da.GetListProductValueArr(id);
            return View(model);
        }
        #region xử lý đơn đặt hàng
        public ActionResult ProcessExportFile()
        {
            var lst = _api.GetListExcel(UserItem.AgencyID, Request.Url.Query);
            var lstC = (from item in lst
                        where item.LstImport != null
                        let dnRequestWareHouseItem = item.LstImport.FirstOrDefault()
                        where dnRequestWareHouseItem != null
                        select new ListCateActiveItem
                        {
                            Productname = item.LstImport != null ? dnRequestWareHouseItem.ProductName : "",
                            DateCreate = item.DateCreated,
                            DateActive = item.DateActive,
                            Useractive = item.UsernameActive,
                            Mobile = item.Mobile,
                            Quantity = item.LstImport != null ? item.LstImport.Sum(c => c.Quantity) : 1,
                            Fullname = item.UsernameCreate,
                            Hours = item.HoursRecive,
                            ID = item.ID,
                            Prizemoney = item.PrizeMoney,
                            DateRecive = item.DateRecive,
                            Address = item.Martketname
                        }).ToList();
            return View(lstC);
        }
        #endregion

        public async Task<ActionResult> AddSupplier(RequestWareSupplierRequest[] request)
        {
            var result = await _api.AddSupplier(request.Where(m => m.Quantity > 0).ToList());
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> AddSupplierView(Guid RequestWareId)
        {
            var requestWare = await _api.GetRequestWareById(RequestWareId);

            var lst = await _supplierAmount.GetSupplierByCategoryId(requestWare.CateID.Value);
            return View(lst);
        }


        public async Task<ActionResult> DeleteSupplier(int id)
        {
            var result = await _api.DeleteSupplier(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SupplierAutoSupplier()
        {
            var query = Request["query"];
            query = query.Replace("%", "");
            query = query.Replace("?", "");
            var ltsResults = await _supplieApi.GetByName(query);
            var resulValues = new AutoCompleteRate
            {
                query = query,
                suggestions = ltsResults.Select(m => new BaseSuggestions() { ID = m.ID, title = m.Name }).ToList(),
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }

    }
}
