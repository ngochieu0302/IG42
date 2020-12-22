using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA;
using FDI.DA.DA.Logistics;
using FDI.DA.DA.StorageWarehouse;
using FDI.MvcAPI.Common;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.Logistics
{

    public class PurchaseOrderController : BaseApiAuthController
    {
        private readonly OrderCarProductDetailDA _da = new OrderCarProductDetailDA();
        private readonly OrderCarDa _orderCarDa = new OrderCarDa();
        private readonly PurchaseOrderDA _purchaseOrderDa = new PurchaseOrderDA();
        private readonly TemplateDocumentDA _templateDocumentDa = new TemplateDocumentDA("#");
        //
        // GET: /PurchaseOrder/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems(decimal todayCode)
        {
            var obj = new PurchaseOrderModel() { Items = _purchaseOrderDa.GetListSimpleByRequest(Request), PageHtml = _purchaseOrderDa.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetById(int id)
        {
            return Json(_purchaseOrderDa.GetById(id));
        }

        public ActionResult Add(PurchaseOrderItem request)
        {
            //check exist ordercarid in table PurchaseOrder

            if (_purchaseOrderDa.CheckExistByOrderCarId(request.OrderCarID))
            {
                return Json(new JsonMessage() { Erros = true, Message = "Đơn hàng đã được nhập vào kho" });
            }

            var order = _orderCarDa.GetById(request.OrderCarID);

            var mode = new PurchaseOrder()
            {
                Quantity = request.Quantity,
                Weight = request.Weight,
                OrderCarId = request.OrderCarID,
                UserCreated = Userid,
                CreateDate = ConvertDate.TotalSeconds(DateTime.Now),
                Note = request.Note
            };

            //lay item dua vao kho
            var products = _da.GetItemByOrderCarId(request.OrderCarID);

            List<Cate_Value> lst = new List<Cate_Value>();
            var p = new PurchaseOrder();
            //using (var transaction = new System.Transactions.TransactionScope())
            //{
            _purchaseOrderDa.Add(mode);

            lst.AddRange(products.Select(product => new Cate_Value()
            {
                Quantity = 1,
                CateID = order.ProductId,
                Code = product.Code,
                PriceCost = product.Quantity * order.Price,
                DateCreated = ConvertDate.TotalSeconds(DateTime.Now),
                PurchaseOrderId = mode.ID,
                Weight = product.Quantity,
                PriceUnit = order.Price,
                IsDelete = false,
                QuantityOut = 0,
                Status = request.ItemID.Any(m => m == product.ID) ? (int)CateValueStatus.Active : (int)CateValueStatus.NoneActive

            }));
            mode.Cate_Value.AddRange(lst);
            _purchaseOrderDa.Save();
            order.Status = (int)OrderCarStatus.Done;
            _orderCarDa.Save();



            //    transaction.Complete();
            //}

            return Json(new JsonMessage() { Erros = false });
        }


        [HttpPost]
        public ActionResult GetExport(int[] ids, int ordercarId)
        {
            var products = _da.GetItemByOrderCarId(ordercarId);
            var model = new PurchaseOrderModel { ListItems = products.Where(m => ids.Any(n => n == m.ID)).ToList() };

            var template = _templateDocumentDa.GetTemplateDocItem((int)Template.PurchaseOrder);

            var result = RenderTemplate.Instance.GetString<PurchaseOrderModel>(template.Content, model, "TemplateDocument" + template.ID);
            return Json(new BaseResponse<string> { Data = result });
        }

        public ActionResult GetByOrderCarId(int ordercarId)
        {
            return Json(_purchaseOrderDa.GetByOrderCarId(ordercarId));
        }
    }
}
