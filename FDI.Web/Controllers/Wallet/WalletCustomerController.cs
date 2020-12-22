using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class WalletCustomerController : BaseController
    {
        //
        // GET: /WalletCustomer/
        WalletCustomerAPI _api = new WalletCustomerAPI();
        WalletCustomerDA _walletCustomerDa = new WalletCustomerDA("#");
        CustomerAPI _customerApi = new CustomerAPI();
        WalletCustomerDA _da = new WalletCustomerDA("#");
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var model = _walletCustomerDa.GetById(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new WalletCustomer();
            ViewBag.customer = _customerApi.GetList();
            if (DoAction == ActionType.Edit)
            {
                model = _walletCustomerDa.GetById(ArrId.FirstOrDefault());
            }
            ViewBag.Action = DoAction;
            ViewBag.AgencyId = UserItem.AgencyID;
            return View(model);
        }
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url);
                    break;
                case ActionType.Edit:
                    msg = _api.Update(url);
                    break;

                case ActionType.Delete:
                    var lst = string.Join(",", ArrId);
                    msg = _api.Delete(lst);
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
