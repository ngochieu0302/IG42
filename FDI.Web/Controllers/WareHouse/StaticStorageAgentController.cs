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
    public class StaticStorageAgentController : BaseController
    {
        //
        // GET: /StaticStorage/
        readonly SupplieAPI _api = new SupplieAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItemsStatic(UserItem.AreaID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
             var   model = _api.GetItemById(UserItem.AgencyID,ArrId.FirstOrDefault());
            return View(model);
        }
        

    }
}
