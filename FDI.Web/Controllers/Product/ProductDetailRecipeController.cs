using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ProductDetailRecipeController : BaseController
    {
        //
        // GET: /ProductDetailRecipe/
        readonly ProductDetailRecipeAPI _api = new ProductDetailRecipeAPI();
        readonly ShopProductDetailAPI _productDetailApi = new ShopProductDetailAPI();
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
            var productSize = new ProductDetailRecipeItem();
            if (DoAction == ActionType.Edit)
            {
                productSize = _api.GetItembyId(ArrId.FirstOrDefault());
            }

            ViewBag.lstproduct = _productDetailApi.GetAll(UserItem.AgencyID);
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
