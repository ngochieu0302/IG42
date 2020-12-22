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
    public class DNWeeklyController : BaseController
    {
       readonly DNWeeklyAPI _weeklyApi;
       readonly  DNScheduleAPI _dnScheduleApi;
       public DNWeeklyController()
        {
            _weeklyApi = new DNWeeklyAPI();
           _dnScheduleApi = new DNScheduleAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelWeeklyItem
            {
                ListItem = _weeklyApi.GetListSimpleByRequest(UserItem.AgencyID),
                //PageHtml = _weeklyApi.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }
        public ActionResult AjaxView()
        {
            var dayOff = _weeklyApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewData.Model = dayOff;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var weekly = new WeeklyItem();
            if (DoAction == ActionType.Edit)
            {
                weekly = _weeklyApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewData.Model = weekly;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var weekly = new WeeklyItem();
            List<WeeklyItem> lstWeekly;
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        weekly.AgencyID = 1;
                        UpdateModel(weekly);
                        json = new JavaScriptSerializer().Serialize(weekly);
                        _weeklyApi.Add(UserItem.AgencyID, json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = weekly.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(weekly.Name))
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
                        UpdateModel(weekly);
                        weekly.AgencyID = 1;
                        json = new JavaScriptSerializer().Serialize(weekly);
                        _weeklyApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = weekly.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(weekly.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    lstWeekly = _weeklyApi.GetListByArrId(UserItem.AgencyID, lstId);
                    foreach (var item in lstWeekly)
                    {
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _weeklyApi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = weekly.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", lstWeekly.Select(c => c.Name))))
                    };
                    break;
                case ActionType.Show:
                    lstWeekly = _weeklyApi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == false).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    foreach (var item in lstWeekly)
                    {
                        item.IsShow = true;
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _weeklyApi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", lstWeekly.Select(c => c.Name))))
                    };
                    break;

                case ActionType.Hide:
                    lstWeekly = _weeklyApi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    foreach (var item in lstWeekly)
                    {
                        item.IsShow = false;
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _weeklyApi.Update(UserItem.AgencyID, json, item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", lstWeekly.Select(c => c.Name))))
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
