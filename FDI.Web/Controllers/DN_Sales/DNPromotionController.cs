using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI.DN_Sales;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.DN_Sales
{
    public class DNPromotionController : BaseController
    {
        //
        // GET: /DNPromotion/
        readonly DNPromotionAPI _api = new DNPromotionAPI();
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
            var model = _api.GetDNPromotionItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new DNPromotionItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetDNPromotionItem(ArrId.FirstOrDefault());
            ViewBag.UserID = UserItem.UserId;
            ViewBag.User = UserItem.UserName;
            ViewBag.Action = DoAction;
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
                    msg = _api.Add(url, UserItem.AgencyID, UserItem.UserId,CodeLogin());
                    break;
                case ActionType.Edit:
                    msg = _api.Update(url, UserItem.UserId, CodeLogin());
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
