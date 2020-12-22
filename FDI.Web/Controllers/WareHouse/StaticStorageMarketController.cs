using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers.WareHouse
{
    public class StaticStorageMarketController : BaseController
    {
        //
        // GET: /StaticStorageMarket/
        readonly MarketAPI _api = new MarketAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItemsStatic(Request.Url.Query, UserItem.AreaID));
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetMarketItem(ArrId.FirstOrDefault());
            return View(model);
        }
    }
}
