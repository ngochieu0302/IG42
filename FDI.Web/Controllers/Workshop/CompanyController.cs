using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CompanyController : BaseController
    {
        //
        // GET: /Company/
        readonly CompanyAPI _api = new CompanyAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new CompanyItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetItemById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            var lstID = string.Join(",", ArrId);
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url, UserItem.UserId);
                    break;

                case ActionType.Edit:
                    msg = _api.Update(url, UserItem.UserId);
                    break;
                case ActionType.Delete:
                    msg= _api.Delete(lstID);
                    break;
            }
            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
