using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.DA;
using FDI.DA.DA.Logistics;
using FDI.DA.DA.StorageWarehouse;
using FDI.DA.DA.Supplier;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class RequestWareController : BaseApiAuthController
    {
        private readonly RequestWareDA _da = new RequestWareDA();
        private readonly SupplierDA _supplierDa = new SupplierDA();
        private readonly SupplierAmountProductDA _amountProductDa = new SupplierAmountProductDA();
        private readonly TotalProductToDayDA _productToDayDa = new TotalProductToDayDA();
        private readonly  OrderCarDa _orderCarDa = new OrderCarDa();
        public ActionResult GetTotalOrder(decimal todayCode)
        {
            return Json(_da.GetTotalOrder(todayCode), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSummaryProductsByToDayCode(decimal todayCode)
        {
            return Json(_da.GetSummaryProductsByToDayCode(todayCode));
        }

        [HttpPost]
        public ActionResult GetTotalProductNotConfirm(decimal todayCode)
        {
            return Json(_da.GetTotalProductNotConfirm(todayCode));
        }
        [HttpPost]
        public ActionResult GetTotalProductConfirm(decimal todayCode)
        {
            //lay so luong da confirm
            var products = _da.GetTotalProductConfirm(todayCode);//_da.GetTotalProductConfirm(todayCode) 
            var supplierids = products.Select(m => m.SupplierId).Distinct().ToList();

            var model = new List<TotalProductToDayConfirmItem>();

            //lay so luong da dat xe
            var orderCars = _orderCarDa.GetByToday(todayCode).GroupBy(m=>new {m.ProductId, m.SupplierId});

            var suppliers = _supplierDa.GetList(supplierids = products.Select(m => m.SupplierId).Distinct().ToList());
            foreach (var dnSupplierItem in suppliers)
            {
                var item = new TotalProductToDayConfirmItem()
                {
                    SupplierName = dnSupplierItem.Name,
                    SupplierPhone = dnSupplierItem.Mobile,
                    SupplierAddress = dnSupplierItem.Address,
                    SupplierId = dnSupplierItem.ID,
                    Details = new List<ProductConfirm>()
                };
                var detal = products.Where(m => m.SupplierId == item.SupplierId).ToList();

                foreach (var productConfirm in detal)
                {
                    var amountActive = _amountProductDa.GetAmount(productConfirm.ProductId, todayCode, dnSupplierItem.ID);
                    if (amountActive != null)
                    {
                        productConfirm.QuantityActive = amountActive.AmountEstimate - amountActive.AmountPayed;
                    }

                    var total = orderCars
                        .FirstOrDefault(m => m.Key.SupplierId == productConfirm.SupplierId &&
                                             m.Key.ProductId == productConfirm.ProductId);
                    if (total != null)
                    {
                        productConfirm.QuantityReady = total.Sum(m => m.Quantity);
                    }
                }

                item.Details.AddRange(detal);
                model.Add(item);
            }

            return Json(model);
        }


        [HttpPost]
        public ActionResult ListItems(decimal todayCode)
        {
            var lst = _da.GetSummaryProductsByToDayCode(todayCode);

            //lay nha cung cap theo categoryid
            var suppliersupports = _supplierDa.GetSupplierByProductIds(lst.Select(n => n.CateID.Value).ToList());


            var model = new List<DNRequestWareTotalItem>();


            //lay so luong da goi nha cung cap
            var products = _productToDayDa.GetListByToDay(todayCode);
            products = products.GroupBy(m => new { m.SupplierId, m.ProductId, m.SupplierName, m.SupplierPhone, m.Price }).Select(m => new TotalProductToDayItem()
            {
                SupplierName = m.Key.SupplierName,
                ProductId = m.Key.ProductId,
                SupplierId = m.Key.SupplierId,
                SupplierPhone = m.Key.SupplierPhone,
                Price = m.Key.Price,
                Quantity = m.Sum(n => n.Quantity)
            }).ToList();

            foreach (var dnRequestWareItem in lst)
            {
                var item = new DNRequestWareTotalItem()
                {
                    CateID = dnRequestWareItem.CateID,
                    Quantity = dnRequestWareItem.Quantity,
                    ProductName = dnRequestWareItem.ProductName
                };

                item.Details = new List<TotalProductToDayItem>();

                var notConfirm = _da.GetTotalProductNotConfirm(todayCode);

                //lay so luong nha cung cap co
                var suppliers = _amountProductDa.GetAmount(dnRequestWareItem.CateID.Value, todayCode);

                foreach (var supplierAmountProductItem in suppliers)
                {
                    var detail = new TotalProductToDayItem()
                    {
                        SupplierName = supplierAmountProductItem.SupplierName,
                        ProductId = supplierAmountProductItem.ProductID.Value,
                        SupplierId = supplierAmountProductItem.SupplierId,
                        QuantityActive = supplierAmountProductItem.AmountEstimate - supplierAmountProductItem.AmountPayed
                    };

                    var tmp = products.FirstOrDefault(m => m.ProductId == supplierAmountProductItem.ProductID && m.SupplierId == supplierAmountProductItem.SupplierId);
                    if (tmp != null)
                    {
                        detail.Quantity = tmp.Quantity;
                        detail.Price = tmp.Price;
                    }
                    item.Details.Add(detail);
                    var supplier = suppliersupports
                        .FirstOrDefault(m => m.ID == supplierAmountProductItem.SupplierId);
                    if (supplier != null)
                    {
                        detail.SupplierPhone = supplier.Mobile;
                    }
                }

                var itemConfirm = notConfirm.FirstOrDefault(m => m.CateID == item.CateID);
                if (itemConfirm != null)
                {
                    item.QuantityNotConfirm = itemConfirm.Quantity;
                }

                item.Details = item.Details.OrderByDescending(m => m.Quantity).ToList();

                //calculate so luong da nhap
                item.QuantityActive = item.Details.Sum(m => m.Quantity);
                model.Add(item);
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult GetSummary(decimal todayCode)
        {
            return Json(_da.GetSummary(todayCode));
        }

        [HttpPost]
        public ActionResult GetDetails(Guid[] ids)
        {
            return Json(_da.GetDetails(ids));
        }
    }

}
