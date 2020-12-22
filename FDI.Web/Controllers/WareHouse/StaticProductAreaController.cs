using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.WareHouse
{
    public class StaticProductAreaController : BaseController
    {
        //
        // GET: /StaticProductArea/
        readonly MarketAPI _marketApi = new MarketAPI();
        readonly AreaAPI _areaApi = new AreaAPI();
        readonly DNAgencyAPI _api = new DNAgencyAPI();
        public ActionResult Index()
        {
            ViewBag.market = _marketApi.GetAll();
            ViewBag.area = _areaApi.GetAll();
            ViewBag.isadmin = IsAdmin;
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItemsStatic(UserItem.EnterprisesID ?? 0, Request.Url.Query, UserItem.AreaID));
        }
        public ActionResult AjaxView()
        {
            var model = new AgencyItem();
            if (DoAction == ActionType.Edit)
            {
                model = _api.GetItemByStatic(ArrId.FirstOrDefault());
            }
            ViewBag.Action = DoAction;
            return View(model);
        }
    }
}
