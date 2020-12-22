using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class GeneralDebtController : BaseController
    {
        //
        // GET: /GeneralDebt/
        readonly SupplieAPI _api = new SupplieAPI();
        public ActionResult Index()
        {
            var model = _api.GetList(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItemsGeneralDebt(UserItem.AgencyID, Request.Url.Query));
        }

    }
}
