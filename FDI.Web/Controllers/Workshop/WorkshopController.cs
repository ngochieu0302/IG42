using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class WorkshopController : BaseController
    {
        //
        // GET: /Workshop/
        readonly WorkshopAPI _api = new WorkshopAPI();
        readonly CompanyAPI _companyApi = new CompanyAPI();
        readonly ProductDetailRecipeAPI _detailRecipeApi = new ProductDetailRecipeAPI();
        readonly CateRecipeAPI _cateRecipeApi = new CateRecipeAPI();
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
            var model = new ModelWorkShopItem
            {
                WorkShopItem = new WorkShopItem(),
                LstCateRecipeItems = new List<CateRecipeItem>(),
                LstRecipeItems = new List<ProductDetailRecipeItem>(),
                LstCompanyItems = new List<CompanyItem>(),
            };
            if (DoAction == ActionType.Edit)
                model.WorkShopItem = _api.GetItemById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            model.LstCompanyItems = _companyApi.GetAll();
            model.LstCateRecipeItems = _cateRecipeApi.GetAll();
            model.LstRecipeItems = _detailRecipeApi.GetAll();
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
                    msg = _api.Delete(lstID);
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
