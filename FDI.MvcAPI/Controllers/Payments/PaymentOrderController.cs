using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers.Payments
{
    public class PaymentOrderController : BaseApiController
    {
        //
        // GET: /PaymentOrder/
        readonly PaymentOrderDA _da = new PaymentOrderDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelPaymentOrderItem()
                : new ModelPaymentOrderItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new PaymentOrderItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
