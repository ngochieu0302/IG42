using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Document
{
    public class DNAgencyController : BaseController
    {
        //
        // GET: /DNAgency/
        readonly DNAgencyAPI _api = new DNAgencyAPI();
        readonly MarketAPI _marketApi = new MarketAPI();
        readonly DNEnterprisesAPI _dnEnterprisesApi = new DNEnterprisesAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = _api.ListItems(UserItem.EnterprisesID ?? 0, Request.Url.Query,UserItem.AreaID);
            ViewData.Model = model;
            return View();
        }
        public ActionResult AjaxView()
        {
            var dnUser = _api.GetItemById(ArrId.FirstOrDefault());
            return View(dnUser);
        }
        public ActionResult AjaxForm()
        {
            var agency = new AgencyItem();
            ViewBag.market = _marketApi.GetAll();
            ViewData.Model = agency;
            ViewBag.StGroup = _dnEnterprisesApi.GetListStGroupById(UserItem.EnterprisesID ?? 0);
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
            var lst = string.Join(",", ArrId);
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Addapp(url);
                    break;
                case ActionType.Hide:
                    msg = _api.LockAgency(lst);
                    break;
                case ActionType.Show:
                    msg = _api.UnLockAgency(lst);
                    break;
                default:
                    msg.Message = "Không có hành động nào được thực hiện.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
