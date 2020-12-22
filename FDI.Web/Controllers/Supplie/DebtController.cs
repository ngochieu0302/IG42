using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Supplie
{
    public class DebtController : BaseController
    {
        //
        // GET: /Debt/
        private readonly DebtAPI _api = new DebtAPI();
        private readonly SupplieAPI supplieAPI = new SupplieAPI();
        public ActionResult Index()
        {
            return View(supplieAPI.GetList(UserItem.AgencyID));
        }
        public ActionResult ListItems()
        {
            var model = _api.GetListByRequest(UserItem.AgencyID, Request.Url.Query);
            model.IsAdmin = IsAdmin;
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new FormDebtItem
            {
                ObjItem = (DoAction == ActionType.Edit) ? _api.GetItemByID(ArrId.FirstOrDefault()) : new DebtItem(),
                Action = DoAction.ToString(),
                AgencyId = UserItem.AgencyID,
                UserId = UserItem.UserId,
                DNSuppliers = supplieAPI.GetList(UserItem.AgencyID)
            };
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetItemByID(ArrId.FirstOrDefault());
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url, UserId);
                    break;
                case ActionType.Edit:
                    msg = _api.Update(url);
                    break;
                case ActionType.Delete:
                    msg.ID = string.Join(",", ArrId);
                    msg = _api.Delete(msg.ID);
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này!";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
