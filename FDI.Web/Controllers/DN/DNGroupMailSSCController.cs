using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNGroupMailSSCController : BaseController
    {
        readonly DNGroupMailSSCAPI _dnMailSscapi;
        readonly DNUserAPI _dnUserApi;
    
        public DNGroupMailSSCController()
        {
            _dnMailSscapi = new DNGroupMailSSCAPI();
            _dnUserApi =  new DNUserAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelDNGroupMailSSCItem
            {
                ListItem = _dnMailSscapi.GetListSimpleByRequest(UserItem.AgencyID),
                //PageHtml = _dnMailSscapi.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }
       
        public ActionResult AjaxForm()
        {
            var mailSscItem = new DNGroupMailSSCItem();
            if (DoAction == ActionType.Edit)
            {
                mailSscItem = _dnMailSscapi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewBag.UserId = _dnUserApi.GetAll(UserItem.AgencyID);
            ViewData.Model = mailSscItem;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var mailSscItem = new DNGroupMailSSCItem();
            List<DNGroupMailSSCItem> ltsDnMailSscItem;
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(mailSscItem);
                        mailSscItem.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                        mailSscItem.LstUserIds = Request["UserId"];
                        mailSscItem.Slug = FomatString.Slug(mailSscItem.Name);
                        json = new JavaScriptSerializer().Serialize(mailSscItem);
                        _dnMailSscapi.Add(UserItem.AgencyID, json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = mailSscItem.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(mailSscItem.Name))
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
                        UpdateModel(mailSscItem);
                        mailSscItem.LstUserIds = Request["UserId"];
                        mailSscItem.Slug = FomatString.Slug(mailSscItem.Name);
                        json = new JavaScriptSerializer().Serialize(mailSscItem);
                        _dnMailSscapi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = mailSscItem.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(mailSscItem.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsDnMailSscItem = _dnMailSscapi.GetListByArrId(UserItem.AgencyID, lstId);
                    foreach (var item in ltsDnMailSscItem)
                    {
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _dnMailSscapi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = mailSscItem.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsDnMailSscItem.Select(c => c.Name))))
                    };
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
