using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Enterprises.Controllers
{
    public class DNAgencyController : BaseController
    {
        private readonly DNAgencyAPI _agencyApi;
        readonly MarketAPI _marketApi = new MarketAPI();
        private readonly DNEnterprisesAPI _dnEnterprisesApi;
        public DNAgencyController()
        {
            _agencyApi = new DNAgencyAPI();
            _dnEnterprisesApi = new DNEnterprisesAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = _agencyApi.ListItems(EnterprisesItem.ID, Request.Url.Query,0);
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxView()
        {
            var dnUser = _agencyApi.GetItemById(ArrId.FirstOrDefault());
            ViewData.Model = dnUser;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var agency = new AgencyItem();
            if (DoAction == ActionType.Edit)
            {
                agency = _agencyApi.GetItemById(ArrId.FirstOrDefault());
            }
            ViewBag.market = _marketApi.GetAll();
            ViewData.Model = agency;
            ViewBag.StGroup = _dnEnterprisesApi.GetListStGroupById(EnterprisesItem.ID);
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var obj = new AgencyItem();
            var json = "";
            switch (DoAction)
            {
                case ActionType.Add:
                    UpdateModel(obj);
                    json = new JavaScriptSerializer().Serialize(obj);
                    msg = _agencyApi.Add(EnterprisesItem.ID, json);
                    break;
                case ActionType.Edit:

                    UpdateModel(obj);
                    obj.ID = ArrId.FirstOrDefault();
                    json = new JavaScriptSerializer().Serialize(obj);
                    msg = _agencyApi.Update(EnterprisesItem.ID, json);
                    break;
                case ActionType.Delete:
                    msg = _agencyApi.Delete(EnterprisesItem.ID, Request["itemID"]);
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
