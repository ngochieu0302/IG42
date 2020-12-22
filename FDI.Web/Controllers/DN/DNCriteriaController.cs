using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNCriteriaController : BaseController
    {
        readonly DNCriteriaAPI _dnCriteriaApi;
        readonly DNUserAPI _dnUserApi;
        readonly DNRoleAPI _dnRoleApi;
       public DNCriteriaController()
        {
            _dnCriteriaApi = new DNCriteriaAPI();
           _dnUserApi = new DNUserAPI();
           _dnRoleApi =  new DNRoleAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            return View(_dnCriteriaApi.ListItems(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            var dnCriteriaItem = _dnCriteriaApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = dnCriteriaItem;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var dnCriteriaItem = new DNCriteriaItem
            {
                IsAll = true
            };
            if (DoAction == ActionType.Edit)
            {
                dnCriteriaItem = _dnCriteriaApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewBag.UserId = _dnUserApi.GetAll(UserItem.AgencyID);
            ViewBag.Type = _dnCriteriaApi.GetTypeAll();
            ViewBag.RoleId = _dnRoleApi.GetAll(UserItem.AgencyID);
            ViewData.Model = dnCriteriaItem;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var dnCriteriaItem = new DNCriteriaItem();
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(dnCriteriaItem);
                        dnCriteriaItem.LstUserIds = Request["UserId"];
                        dnCriteriaItem.LstRoleIds = Request["RoleId"];
                        json = new JavaScriptSerializer().Serialize(dnCriteriaItem);
                        _dnCriteriaApi.Add(UserItem.AgencyID, json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dnCriteriaItem.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(dnCriteriaItem.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        UpdateModel(dnCriteriaItem);
                        dnCriteriaItem.LstUserIds = Request["UserId"];
                        dnCriteriaItem.LstRoleIds = Request["RoleId"];
                        json = new JavaScriptSerializer().Serialize(dnCriteriaItem);
                        _dnCriteriaApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = dnCriteriaItem.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(dnCriteriaItem.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    msg = _dnCriteriaApi.Delete(lstId);
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
