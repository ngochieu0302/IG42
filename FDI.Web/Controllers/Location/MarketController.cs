using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Location
{
    public class MarketController : BaseController
    {
        //
        // GET: /Market/
        readonly WardsAPI _wardsApi = new WardsAPI();
        readonly AreaAPI _areaApi = new AreaAPI();
        readonly MarketAPI _api = new MarketAPI();
        public ActionResult Index()
        {
            ViewBag.listCity = _wardsApi.GetAll();
            ViewBag.area = _areaApi.GetAll();
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItems(Request.Url.Query));
        }
        public ActionResult AjaxForm()
        {
            var model = new MarketItem();
            if (DoAction == ActionType.Edit)
            {
                model = _api.GetMarketItem(ArrId.FirstOrDefault());
            }
            ViewBag.listCity = _wardsApi.GetAll();
            ViewBag.area = _areaApi.GetAll();
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
