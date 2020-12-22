using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Web.Controllers.Statistical
{
    public class OrderByUserPVController : BaseController
    {
        //
        // GET: /OrderByUserPV/

        private readonly OrderAPI _api = new OrderAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.OrderByUserPVRequest(UserItem.AgencyID, Request.Url.Query));
        }

    }
}
