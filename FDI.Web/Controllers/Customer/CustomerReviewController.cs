using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.GetAPI;
using FDI.Utils;

namespace FDI.Web.Controllers.Customer
{
    public class CustomerReviewController : BaseController
    {
        //
        // GET: /CustomerReview/
        readonly CustomerReviewAPI _api = new CustomerReviewAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = new Customer_Review();
            ViewBag.agencyId = UserItem.AgencyID;
            return View(model);
        }

        public ActionResult htmlPart2()
        {
            return View();
        }
        public ActionResult htmlPart4()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url,UserItem.AgencyID);
                    break;
                default:
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
