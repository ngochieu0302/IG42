using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Statistical
{
    public class StaticOrderAllController : BaseController
    {
        //
        // GET: /StaticOrderAll/
        private readonly OrderAPI _api = new OrderAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        public ActionResult Index()
        {
            ViewBag.listagency = _agencyApi.GetAll(UserItem.EnterprisesID ?? 0);
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query, UserItem.UserId, IsAdmin));
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetOrderItem(ArrId.FirstOrDefault());
            ViewBag.Agency = _agencyApi.GetItemById(UserItem.AgencyID);
            return View(model);
        }
    }
}
