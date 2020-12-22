using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CateRecipeController : BaseController
    {
        //
        // GET: /CateRecipe/
        readonly CateRecipeAPI _api = new CateRecipeAPI();
        readonly CategoryDA _daCategory = new CategoryDA("#");
        public ActionResult Index()
        {
            var lstCate = _daCategory.GetChildByParentId(false);
            ViewBag.lstCate = lstCate;
            return View();

        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var productSize = new CateRecipeItem();
            if (DoAction == ActionType.Edit)
            {
                productSize = _api.GetItembyId(ArrId.FirstOrDefault());
            }
            ViewData.Model = productSize;
            var lstCate = _daCategory.GetChildByParentId(false);
            ViewBag.lstCate = lstCate;
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
            url = HttpUtility.UrlDecode(url);
            var lstArrId = "";
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url, CodeLogin(), UserItem.UserId);
                    break;
                case ActionType.Edit:
                    msg = _api.Update(url, CodeLogin(), UserItem.UserId);
                    break;
                case ActionType.Active:
                    lstArrId = string.Join(",", ArrId);
                    msg = _api.Active(lstArrId, UserItem.UserId);
                    break;
                case ActionType.Delete:
                    lstArrId = string.Join(",", ArrId);
                    msg = _api.Delete(lstArrId);
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
