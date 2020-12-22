using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.GetAPI.StorageWarehouse;
using FDI.GetAPI.Supplier;
using FDI.Simple;
using FDI.Simple.Logistics;
using FDI.Simple.Supplier;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class OrderCarController : BaseController
    {
        //
        // GET: /Supplie/
        readonly CategoryAPI _categoryApi = new CategoryAPI();
        readonly SupplieAPI _apiSupplieAPI = new SupplieAPI();
        private readonly CarApi _carApi = new CarApi();
        private readonly OrderCarAPI _orderCarApi = new OrderCarAPI();
        private readonly TotalProductToDayAPI _productToDayApi = new TotalProductToDayAPI();
        private  readonly WorkshopAPI _workshopApi = new WorkshopAPI();
        public ActionResult Index(decimal? todayCode)
        {
            if (todayCode == null)
            {
                return RedirectToAction("Index", new { todayCode = DateTime.Now.AddDays(1).Date.TotalSeconds() });
            }

            return View(todayCode.Value);
        }
        public ActionResult ListItems(decimal todayCode)
        {
            return View(_orderCarApi.ListItems(Request.Url.Query));
        }
        public async Task<ActionResult> AjaxForm()
        {
            var model = new OrderCarModel()
            {
                OrderCar = new OrderCarItem()
                {
                    DepartureDate = DateTime.Now.AddDays(1).Date.TotalSeconds(),
                    ReceiveDate = DateTime.Now.AddDays(1).Date.TotalSeconds(),
                    ReturnDate = DateTime.Now.AddDays(1).Date.TotalSeconds()
                }

            };

            if (DoAction == ActionType.Edit)
                model.OrderCar = await _orderCarApi.GetById(ArrId.FirstOrDefault());

            model.Supplier = _apiSupplieAPI.GetItemById(model.OrderCar.SupplierId);
            model.Category = _categoryApi.GetItemById(model.OrderCar.CarId);
            model.OrderCar.Quantity = model.OrderCar.Quantity;
            model.OrderCar.UnitName = model.Category.UnitName;
            model.Workshops = await _workshopApi.GetAll();
            model.Cars = await _carApi.GetListAssign(model.Category.UnitID.Value);

            ViewBag.Action = DoAction;
            return View("AddForm", model);
        }

        public async Task<ActionResult> AddForm(int supplierId, int categoryId, decimal quantity, int todayCode)
        {
            
            var model = new OrderCarModel
            {
                OrderCar = new OrderCarItem()
                {
                    DepartureDate = DateTime.Now.AddDays(1).Date.TotalSeconds(),
                    ReceiveDate = DateTime.Now.AddDays(1).Date.TotalSeconds(),
                    ReturnDate = DateTime.Now.AddDays(1).Date.TotalSeconds()
                },
                Supplier = _apiSupplieAPI.GetItemById(supplierId),
                Category = _categoryApi.GetItemById(categoryId),
                Workshops = await _workshopApi.GetAll(),
            };
            model.OrderCar.Quantity = quantity;
            model.OrderCar.UnitName = model.Category.UnitName;
            model.Cars = await _carApi.GetListAssign(model.Category.UnitID.Value);

            var productitem = await _productToDayApi.GetItem(todayCode, categoryId, supplierId);
            model.OrderCar.Price = productitem.Price.Value;
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
                    var request = new OrderCarItem();
                    UpdateModel(request);
                    var result = await _orderCarApi.Add(request);
                    if (result.Erros)
                    {
                        msg.Erros = true;
                        msg.Message = result.Message;
                        break;
                    }
                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;

                case ActionType.Edit:
                    var requestUpdate = new OrderCarItem();
                    UpdateModel(requestUpdate);

                    await _orderCarApi.Update(requestUpdate);

                    msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                case ActionType.Delete:
                    await _orderCarApi.Delete(ArrId.FirstOrDefault());
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
    }
}