using FDI.DA.DA.Supplier;
using FDI.Simple.Supplier;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers.Supplier
{
    public class SupplierAmountProductController : BaseApiAuthController
    {
        //
        // GET: /SupplierAmountProduct/
        private readonly SupplierAmountProductDA _da = new SupplierAmountProductDA();

        public ActionResult ListItems()
        {
            var obj = new SupplierAmountProductResponse() { ListItem = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(SupplierAmountProductItem request)
        {
            _da.Add(new Base.SupplierAmountProduct()
            {
                SupplierId = request.SupplierId,
                ProductID = request.ProductID,
                PublicationDate = request.PublicationDate,
                ExpireDate = request.ExpireDate,
                IsAlwayExist = request.IsAlwayExist,
                AmountEstimate = request.AmountEstimate,
                AmountPayed = request.AmountPayed,
                CallDate = request.CallDate,
                Note = request.Note,
                IsDelete = false,
                CreatedDate = DateTime.Now.TotalSeconds()
            });
            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(SupplierAmountProductItem request)
        {
            var model = _da.GetById(request.ID);
            model.AmountEstimate = request.AmountEstimate;
            model.AmountPayed = request.AmountPayed;
            model.CallDate = request.CallDate;
            model.IsAlwayExist = request.IsAlwayExist;
            model.ExpireDate = request.ExpireDate;
            model.Note = request.Note;
            model.ProductID = request.ProductID;
            model.SupplierId = request.SupplierId;
            model.UserActiveId = request.UserActiveId;
            model.PublicationDate = request.PublicationDate;
            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var model = _da.GetById(id);
            model.IsDelete = true;
            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(int id)
        {
            return Json(_da.GetItemById(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSupplierByCategoryId(int id)
        {
            return Json(_da.GetSupplierByCategoryId(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAmount(int productId, decimal todayCode)
        {
            return Json(_da.GetAmount(productId, todayCode));
        }
    }
}
