using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class OrderByLeverController : BaseController
    {
        //
        // GET: /OrderByLever/

        private readonly OrderAPI _api = new OrderAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.OrderByLevelRequest(UserItem.AgencyID, Request.Url.Query));
        }

    }
}
