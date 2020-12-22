using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Utils;

namespace FDI.Web.Controllers.Sales
{
    public class TherapyHistoryController : BaseController
    {
        //
        // GET: /TherapyHistory/
        private readonly OrderAPI _api = new OrderAPI();
        readonly TherapyHistoryAPI _therapyHistoryApi = new TherapyHistoryAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = _api.GetListbyDate(UserItem.AgencyID, Request.Url.Query);
            return View(model);
        }

        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            msg = _therapyHistoryApi.Add(UserItem.AgencyID, url);
            return Json(msg,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetlistbyCustomerID(int id,string phone)
        {
            var model = _therapyHistoryApi.GetListByCustomerID(id,phone);
            return View(model);
        }
    }
}
