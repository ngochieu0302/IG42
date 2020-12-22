using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class OrderCusAppSalesController : BaseApiAuthAppSaleController
    {
        private int rowPerPage = 4;
        //
        // GET: /AgencyApp/
        private readonly OrderCusAppSaleDA _dl = new OrderCusAppSaleDA("#");
        private readonly OrdersDA _ordersDa = new OrdersDA();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ListAll(int pageIndex, string txt, int orderStatus, decimal startDate, decimal endDate, bool? orderbyPrice)
        {
            var total = 0;
            var obj = _dl.GetAll(AgencyId, rowPerPage, pageIndex, orderStatus, txt, startDate, endDate, orderbyPrice, ref total);
            var result = new BaseResponse<List<ContactOrderAppItem>>()
            {
                Data = obj,
                TotalItems = total,
                RowPerPage = rowPerPage
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListOrder(string key, int rowPerPage, int page, int aid, string cus, string fd, string td)
        {
            int total = 0;
            var obj = key != Keyapi
                ? new ModelOrderAppSaleItem()
                : new ModelOrderAppSaleItem
                {
                    ListItems = _dl.GetListOrderAppSale(rowPerPage, page, aid, cus, fd, td, ref total),
                    Total = total
                };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetOrderDetail(int id, bool isContactOrder)
        {
            var response = new ContactOrderItem();
            if (isContactOrder)
            {
                response = _dl.getDetailById(id);
                var list = _dl.GetListDetailsById(id);
                response.LstOrderDetailItems = list;
            }
            else
            {
                var shopOrder = _ordersDa.GetItemById(id);

                response.TotalPrice = shopOrder.TotalPrice;
                response.Status = shopOrder.Status;
                response.ReceiveDate = shopOrder.ReceiveDate ?? 0;
                response.AgencyId = shopOrder.AgencyId;
                response.CutomerID = shopOrder.CustomerID;
                response.ID = shopOrder.ID;

                var lst = new OrderDetailDA().GetList(id);
                var tmp = new List<OrderDetailItem>();

                foreach (var item in lst)
                {
                    tmp.Add(new OrderDetailItem()
                    {
                        ProductName = item.ProductName,
                        UrlImg = item.UrlImg,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice,
                        Weight = item.Weight,
                        Price = item.Price
                    });
                }

                response.LstOrderDetailItems = tmp;
            }

            if (response.AgencyId != AgencyId)
            {
                return Json(new JsonMessage(true, "'"));
            }

            //get customer info
            if (response.CutomerID != null)
            {
                var customerDa = new CustomerDA();
                response.CustomerItem = customerDa.GetCustomerItem(response.CutomerID.Value);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        void getShopOrder(ContactOrderItem response, int id)
        {
            var shopOrder = _ordersDa.GetItemById(id);
            response.TotalPrice = shopOrder.TotalPrice;
            response.Status = shopOrder.Status;
            response.ReceiveDate = shopOrder.ReceiveDate.Value;
        }

        void getContactOrder(ContactOrderItem response, int id)
        {
            var order = _dl.GetByID(id);
            if ((OrderStatus)order.Status == OrderStatus.Pending)
            {
                response.TotalPrice = order.TotalPrice;
                response.Status = order.Status;
                response.ReceiveDate = order.ReceiveDate;
            }
        }
        public ActionResult StatusCancel(string key, int id, int status, string note, int time)
        {

            if (key == Keyapi)
            {
                var model = _dl.GetByID(id);
                model.Status = status;
                model.Content = note;
                model.TotalMinute = time;
                _dl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserByQrCode(string key, string qrcode)
        {
            var obj = key != Keyapi ? new CustomerAppItem() : _dl.GetCusbyQrCode(qrcode);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

      
    }
}
