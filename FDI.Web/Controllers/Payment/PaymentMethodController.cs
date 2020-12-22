using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Payment
{
    public class PaymentMethodController : BaseController
    {
        //
        // GET: /PaymentMethod/
        readonly PaymentMethodAPI _api = new PaymentMethodAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var productSize = new PaymentMethodItem();
            if (DoAction == ActionType.Edit)
            {
                productSize = _api.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewData.Model = productSize;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            var lst1 = string.Join(",", ArrId);
            url = HttpUtility.UrlDecode(url);
            switch (DoAction)
            {
                case ActionType.Add:
                   msg = _api.Add(url);
                    break;
                case ActionType.Edit:
                msg = _api.Update(url,ArrId.FirstOrDefault());
                    break;
                case ActionType.Delete:
                msg = _api.Delete(lst1);
                    break;
                default:
                msg.Message = "Bạn không được phần quyền cho chức năng này.";
                msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);

        }

    }
}
