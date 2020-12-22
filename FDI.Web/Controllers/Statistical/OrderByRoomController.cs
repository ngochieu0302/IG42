using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers
{
    public class OrderByRoomController : BaseController
    {
        //
        // GET: /OrderByRoom/

        private readonly OrderAPI _api = new OrderAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.OrderByRoomRequest(UserItem.AgencyID, Request.Url.Query));
        }

    }
}
