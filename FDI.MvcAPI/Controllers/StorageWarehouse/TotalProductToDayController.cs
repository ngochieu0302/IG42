using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA.DA.StorageWarehouse;
using FDI.DA.DA.Supplier;
using FDI.Simple;
using FDI.Simple.Order;
using FDI.Simple.StorageWarehouse;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.StorageWarehouse
{
    public class TotalProductToDayController : BaseApiAuthController
    {
        //
        // GET: /TotalStorageWare/
        readonly TotalProductToDayDA _da = new TotalProductToDayDA();
        private readonly SupplierAmountProductDA _daSupplierAmountProduct = new SupplierAmountProductDA();
        public ActionResult ListItems(decimal todayCode, int productId)
        {
            var obj = new ModelTotalProductToDayItem { ListItems = _da.GetListSimpleByRequest(Request, todayCode, productId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddSupplier(RequestWareSupplierRequest[] request)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            _da.AddSupplier(request.Select(m =>
            {
                m.UserId = Userid ?? Guid.Empty; return m;
            }).ToArray());
            _da.Save();
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveSupplier(RequestWareSupplierRequest request)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            var item = _da.GetbyId(request.ToDayCode, request.ProductId, request.SupplierId);
            var lst = _daSupplierAmountProduct.GetItem(request.ToDayCode, request.SupplierId, request.ProductId);
            var valueReady = lst.Sum(m => m.AmountEstimate - m.AmountPayed);

            if (item == null)
            {
                if (valueReady < request.Quantity)
                {
                    return Json(new JsonMessage()
                    {
                        Erros = true,
                        Message = "Số lượng nhà cung câp không đủ"
                    });
                }

                _da.AddSupplier(request);

                var amountUpdate = request.Quantity;
                // tinh lai so da ban o nha cung cap
                foreach (var supplierAmountProduct in lst)
                {
                    if (supplierAmountProduct.AmountEstimate - supplierAmountProduct.AmountPayed >= amountUpdate)
                    {
                        supplierAmountProduct.AmountPayed += amountUpdate;
                        amountUpdate = 0;
                        break;
                    }
                    else if (supplierAmountProduct.AmountEstimate > supplierAmountProduct.AmountPayed)
                    {
                        amountUpdate -= supplierAmountProduct.AmountEstimate - supplierAmountProduct.AmountPayed;
                        supplierAmountProduct.AmountPayed = supplierAmountProduct.AmountEstimate;
                    }
                }

                if (amountUpdate != 0)
                {
                    throw new Exception("Quantity is error");
                }

            }
            else
            {
                var amountDiff = request.Quantity - item.Quantity;

                foreach (var supplierAmountProduct in lst)
                {

                    if (supplierAmountProduct.AmountPayed + amountDiff < 0)
                    {
                        supplierAmountProduct.AmountPayed = 0;
                        amountDiff += supplierAmountProduct.AmountPayed;
                    }
                    else if (supplierAmountProduct.AmountPayed + amountDiff > supplierAmountProduct.AmountEstimate)
                    {

                        amountDiff += supplierAmountProduct.AmountEstimate -supplierAmountProduct.AmountPayed;
                        supplierAmountProduct.AmountPayed = supplierAmountProduct.AmountEstimate;
                    }
                    else
                    {
                        supplierAmountProduct.AmountPayed = request.Quantity;
                        break;
                    }
                }

                item.Quantity = request.Quantity;
                item.Price =request.Price;
            }
            _da.Save();
            _daSupplierAmountProduct.Save();
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotalOrder(decimal totalCode)
        {
            return Json(_da.GetTotalOrder(totalCode));
        }

        public ActionResult GetSummaryTotalByToDay(decimal todayCode)
        {
            var obj = _da.GetSummaryTotalByToDay(todayCode);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetListByToDay(decimal todayCode)
        {
            var obj = _da.GetListByToDay(todayCode);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetItem(decimal todayCode, int productId, int supplierId)
        {
            return Json(_da.GetItem(todayCode, productId, supplierId));
        }
    }
}
