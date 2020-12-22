using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Product
{
    public class SimController : BaseController
    {
        //
        // GET: /Sim/
        readonly ProductAPI _api = new ProductAPI();

        public ActionResult Index()
        {
            return View(_api.GetModelHomeSupplierItem(UserItem.AgencyID));
        }
        public ActionResult ListItems()
        {
            return View(_api.ListSimItems(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            var model = _api.GetProductItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult Auto()
        {
            var query = Request["query"];
            var type = Request["type"] ?? "3";
            var ltsResults = _api.GetListAuto(query, 10, UserItem.AgencyID, int.Parse(type));
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
    }
}
