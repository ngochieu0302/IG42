using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.GetAPI.Payments;

namespace FDI.Web.Controllers.Payment
{
    public class PaymentOrderController : BaseController
    {
        //
        // GET: /PaymentOrder/
        readonly PaymentOrderAPI _api = new PaymentOrderAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
    }
}
