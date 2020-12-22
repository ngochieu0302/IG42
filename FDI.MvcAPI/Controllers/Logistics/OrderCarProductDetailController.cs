using FDI.DA.DA.Supplier;
using FDI.Simple.Supplier;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA.Logistics;
using FDI.Simple.Logistics;

namespace FDI.MvcAPI.Controllers.Supplier
{
    public class OrderCarProductDetailController : BaseApiAuthController
    {
        //
        // GET: /SupplierAmountProduct/
        private readonly OrderCarProductDetailDA _da = new OrderCarProductDetailDA();

        public ActionResult ListItems(int ordercarId)
        {
            var obj = new OrderCarProductDetailResponse() { ListItem = _da.GetListSimpleByRequest(Request, ordercarId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(OrderCarProductDetailItem request)
        {
            _da.Add(new OrderCarProductDetail()
            {
                Code = request.Code,
                OrderCarId = request.OrderCarID,
                IsDelete = false,
                Quantity = request.Quantity
            });

            _da.Save();
            return Json(new JsonMessage() { Erros = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAmountRecevied(int ordercarId)
        {
            return Json(_da.GetAmountRecevied(ordercarId)??0);
        }
        public ActionResult CountRecevied(int ordercarId)
        {
            return Json(_da.CountRecevied(ordercarId));
        }
        public ActionResult Update(OrderCarProductDetail request)
        {
            var model = _da.GetById(request.ID);
            model.Quantity = request.Quantity;
            model.Code = request.Code;

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

        public ActionResult GetAllByOrderCarId(int ordercarId)
        {
            var obj = new OrderCarProductDetailResponse() { ListItem = _da.GetListSimpleByRequest(Request, ordercarId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}
