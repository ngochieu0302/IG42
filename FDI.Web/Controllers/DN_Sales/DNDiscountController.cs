using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.GetAPI.DN_Sales;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.DN_Sales
{
    public class DNDiscountController : BaseController
    {
        //
        // GET: /DNDiscount/
        readonly DNDiscountAPI _api = new DNDiscountAPI();
        readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        public ActionResult Index()
        {
            ViewBag.agency = _agencyApi.GetAll(UserItem.EnterprisesID ?? 0);
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(0, Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new DNDiscountItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetDNDiscountItem(ArrId.FirstOrDefault());
            ViewBag.UserID = UserItem.UserId;
            ViewBag.User = UserItem.UserName;
            ViewBag.Action = DoAction;
            ViewBag.agent= _agencyApi.GetAll(UserItem.EnterprisesID ?? 0);
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công." };
            var url = Request.Form.ToString();
            url = HttpUtility.UrlDecode(url);
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url,  UserItem.UserId);
                    break;
                case ActionType.Edit:
                    msg = _api.Update(url, UserItem.UserId);
                    break;
                case ActionType.Delete:
                    var lst1 = string.Join(",", ArrId);
                    msg = _api.Delete(lst1);
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không có quyền thực hiện chứ năng này.";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
