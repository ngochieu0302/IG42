using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class OrderByUserController : BaseController
    {
        //
        // GET: /OrdeByUser/

        private readonly OrderAPI _api = new OrderAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.OrderByUserRequest(UserItem.AgencyID, Request.Url.Query));
        }

        //public ActionResult AjaxView()
        //{
        //    var model = _api.GetOrderItem(ArrId.FirstOrDefault());
        //    ViewBag.Agency = _agencyApi.GetItemById(UserItem.AgencyID);
        //    return View(model);
        //}
    }
}
