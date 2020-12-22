using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.GetAPI.Supplier;
using FDI.Simple.Logistics;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;

namespace FDI.Web.Controllers.Logistics
{
    public class PurchaseOrderDetailController : BaseController
    {
        private readonly OrderCarProductDetailAPI _carProductDetailApi = new OrderCarProductDetailAPI();
        private readonly OrderCarAPI _orderCarApi = new OrderCarAPI();
        private readonly DNUnitAPI _dnUnitApi = new DNUnitAPI();
        private readonly PurchaseOrderAPI _purchaseOrderApi = new PurchaseOrderAPI();
        
        //
        // GET: /OrderCarProductDetail/

        public async Task<ActionResult> Index(int ordercarId)
        {
            var model = await _orderCarApi.GetById(ordercarId);

            //lây khối lượng đã nhập
            model.QuantityReceived = await _carProductDetailApi.GetAmountRecevied(ordercarId);
            model.CountReceived = await _carProductDetailApi.CountRecevied(ordercarId);

            return View(model);
        }
        public ActionResult ListItems()
        {
            return View(_carProductDetailApi.ListItems(Request.Url.Query));
        }

        public async Task<ActionResult> Detail(int ordercarId)
        {
            var model = await _orderCarApi.GetById(ordercarId);

            //lây khối lượng đã nhập
            model.QuantityReceived = await _carProductDetailApi.GetAmountRecevied(ordercarId);
            model.CountReceived = await _carProductDetailApi.CountRecevied(ordercarId);
            return PartialView(model);
        }

        public async Task<ActionResult> AjaxForm(int ordercarId)
        {
            var model = new OrderCarProductDetailModel()
            {
                Order = await _orderCarApi.GetById(ordercarId),
                Units = _dnUnitApi.GetAllList()
            };
            model.Item.Code = StringExtensions.GetCode();
            if (DoAction == ActionType.Edit)
                model.Item = await _carProductDetailApi.GetById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            var lstID = string.Join(",", ArrId);
            switch (DoAction)
            {
                case ActionType.Add:
                    var request = new PurchaseOrderItem(); ;
                    UpdateModel(request);

                    var result = await _purchaseOrderApi.Add(request);
                    if (result.Erros)
                    {
                        msg.Erros = true;
                        msg.Message = result.Message;
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    var requestUpdate = new OrderCarProductDetailItem();
                    UpdateModel(requestUpdate);

                    await _carProductDetailApi.Update(requestUpdate);

                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Delete:
                    await _carProductDetailApi.Delete(ArrId.FirstOrDefault());
                    msg.Message = "Cập nhật thành công.";
                    break;
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetExport(int[] ids, int ordercarId)
        {
            var result = await _purchaseOrderApi.GetExport(ids, ordercarId);
            return View(result);
        }
    }
}
