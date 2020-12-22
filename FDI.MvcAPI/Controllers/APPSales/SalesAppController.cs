using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA;
using FDI.MvcAPI.Models;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.APPSales
{
    public class SalesAppController : BaseApiAuthController
    {
        //
        // GET: /SalesApp/
        readonly DNSalesAppDA _da = new DNSalesAppDA();
        readonly OrdersDA _ordersDa = new OrdersDA();
        readonly OrderCusAppSaleDA _cusAppSaleDa = new OrderCusAppSaleDA("#");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keword">Barcode SP</param>
        /// <param name="agencyId">Sản phẩm theo đại lý</param>
        /// <returns></returns>
        public ActionResult GetListAutoOne(string keword)
        {
            var obj = _da.GetObjOne(keword, 0);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keword">SĐT or Mã QR cho KH</param>
        /// <param name="agencyId">Khánh hàng của đại lý or tất cả</param>
        /// <returns></returns>
        public ActionResult GetListCommentAuto(string key, string keword, int agencyId)
        {
            var obj = Request["key"] != Keyapi ? new CustomerItem() : _da.GetObjCustomerAuto(keword, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddOrder(OrderAgencyModel data)
        {
            var order = new Shop_Orders { AgencyId = AgencyId };

            var datenow = DateTime.Now.TotalSeconds();

            var lst = _da.GetListImportProductBarcode(data.BarCodes, AgencyId);
            var lstDetail = lst.Select(item => new Shop_Order_Details
            {
                Barcode = item.BarCode,
                Quantity = 1,
                QuantityOld = 0,
                Price = item.Price,
                TotalPrice = item.PriceNew,
                Total = item.PriceNew,
                Discount = 0,
                DateCreated = datenow,
                Status = (int)FDI.CORE.OrderStatus.Complete,
                Value = item.Value,
                GID = Guid.NewGuid(),
                ImportProductGID = item.GID,
                ProductID = item.ProductValueID 
            }).ToList();

            if (!lstDetail.Any()) return Json(new JsonMessage(true, "Không tồn tại mã sản phẩm"), JsonRequestBehavior.AllowGet);

            var total = lstDetail.Sum(c => c.Total);
            var totalprice = lstDetail.Sum(c => c.TotalPrice);
            order.Shop_Order_Details = lstDetail;
            order.TotalSaleSP = 0;
            order.DateCreated = datenow;
            order.Status = (int)FDI.CORE.OrderStatus.Complete;
            order.IsDelete = false;
            order.StartDate = datenow;
            order.Total = total;
            order.TotalPrice = totalprice;
            order.CustomerID = data.CustomerId;
            order.Payments = order.TotalPrice;
            order.PriceReceipt = order.Payments;
            order.Type = (int)TypeOrder.Banle;
            order.ContactOrderID = data.OrderId;
            order.ReceiveDate = DateTime.Now.TotalSeconds();
            
            
            _ordersDa.Add(order);
            _ordersDa.Save();
            if (data.OrderId > 0)
            {
                var contact = _cusAppSaleDa.GetByID(data.OrderId.Value);
                contact.Status = (int)FDI.CORE.OrderStatus.Complete;
            }
            return Json(new JsonMessage(false,""), JsonRequestBehavior.AllowGet);

        }
    }
}

