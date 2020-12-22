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
    public class DNCalendarController : BaseController
    {
        readonly DNCalendarAPI _calendarApi;
        readonly DNUserAPI _userApi;
        readonly DNWeeklyScheduleAPI _weeklyScheduleApi;
        //private readonly Shop_ProductDA _shopProductDa;
        private readonly DNRoleAPI _dnRoleApi;
       public DNCalendarController()
        {
            _calendarApi = new DNCalendarAPI();
           _userApi = new DNUserAPI();
           _weeklyScheduleApi = new DNWeeklyScheduleAPI();
           //_shopProductDa = new Shop_ProductDA();
           _dnRoleApi = new DNRoleAPI();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WeekByYear(int year)
        {
            var listWeekItems = FDIUtils.ListWeekByYear(year);
            return Json(listWeekItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems()
        {
            var model = _calendarApi.ListItems(UserItem.AgencyID, Request.Url.Query);
            ViewData.Model = model;
            return View();
        }
        public ActionResult AjaxView()
        {
            var calendar = _weeklyScheduleApi.GetAll(UserItem.AgencyID, ArrId.FirstOrDefault());
            ViewBag.ID = ArrId.FirstOrDefault();
            return View(calendar);
        }

        public ActionResult AjaxForm()
        {
            var calendar = new DNCalendarItem();
            if (DoAction == ActionType.Edit)
            {
                calendar = _calendarApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewBag.UserId = _userApi.GetAll(UserItem.AgencyID);
            ViewBag.RoleId = _dnRoleApi.GetAll(UserItem.AgencyID);
            ViewData.Model = calendar;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var calendar = new DNCalendarItem();
            List<DNCalendarItem> ltsDnCalendarItem;
            //var date = Request["DateCreated"];
            var dateStart = Request["_DateStart"];
            var dateEnd = Request["_DateEnd"];
            var lstProductId = Request["BedDeskID"];
            var json = "";
            var lstId = Request["itemId"];
            var lstUserIds = Request["UserId"];
            var lstRoleIds = Request["RoleId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(calendar);
                        calendar.IsDelete = false;
                        calendar.IsShow = true;
                        calendar.LstUserIds = lstUserIds;
                        calendar.LstRoleIds = lstRoleIds;
                        calendar.DateCreated = ConvertDate.TotalSeconds(DateTime.Now);
                        if (!string.IsNullOrEmpty(dateStart))
                            calendar.DateStart = dateStart.StringToDecimal();
                        if (!string.IsNullOrEmpty(dateEnd))
                            calendar.DateEnd = dateEnd.StringToDecimal();
                        if (!string.IsNullOrEmpty(lstProductId))
                            calendar.ListProductId = lstProductId;
                        json = new JavaScriptSerializer().Serialize(calendar);
                        _calendarApi.Add(UserItem.AgencyID, json,CodeLogin());
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = calendar.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(calendar.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.View:
                    try
                    {
                        UpdateModel(calendar);
                        calendar.IsShow = true;
                        calendar.ID = ArrId.FirstOrDefault();
                        calendar.WeeklySchedule = Request["WeeklySchedule"];
                        _calendarApi.AddCalendarWeeklySchedule(UserItem.AgencyID, calendar.ID, calendar.WeeklySchedule);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = calendar.ID.ToString(),
                            Message = string.Format("Đã thêm mới: <b>{0}</b>", Server.HtmlEncode(calendar.Name))
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
                        UpdateModel(calendar);
                        calendar.IsDelete = false;
                        if (!string.IsNullOrEmpty(dateStart))
                            calendar.DateStart = dateStart.StringToDecimal();
                        if (!string.IsNullOrEmpty(dateEnd))
                            calendar.DateEnd = dateEnd.StringToDecimal();
                        if (!string.IsNullOrEmpty(lstProductId))
                            calendar.ListProductId = lstProductId;
                        calendar.DNCalendarWeeklySchedule = null;
                        calendar.ListDnUserItem = null;
                        calendar.ListDnRolesItem = null;
                        calendar.LstUserIds = lstUserIds;
                        calendar.LstRoleIds = lstRoleIds;
                        json = new JavaScriptSerializer().Serialize(calendar);
                        _calendarApi.Update(UserItem.AgencyID, json, CodeLogin(), ArrId.FirstOrDefault());
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = calendar.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(calendar.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsDnCalendarItem = _calendarApi.GetListByArrId(UserItem.AgencyID, lstId);
                    foreach (var item in ltsDnCalendarItem)
                    {
                        item.IsDelete = true;
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _calendarApi.Update(UserItem.AgencyID, json,CodeLogin(), item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = calendar.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsDnCalendarItem.Select(c => c.Name))))
                    };
                    break;
                case ActionType.Show:
                    ltsDnCalendarItem = _calendarApi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == false).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    foreach (var item in ltsDnCalendarItem)
                    {
                        item.IsDelete = false;
                        item.IsShow = true;
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _calendarApi.Update(UserItem.AgencyID, json,CodeLogin(), item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = calendar.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsDnCalendarItem.Select(c => c.Name))))
                    };
                    break;

                case ActionType.Hide:
                    ltsDnCalendarItem = _calendarApi.GetListByArrId(UserItem.AgencyID, lstId).Where(o => o.IsShow == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    foreach (var item in ltsDnCalendarItem)
                    {
                        item.IsDelete = false;
                        item.IsShow = false;
                        UpdateModel(item);
                        json = new JavaScriptSerializer().Serialize(item);
                        _calendarApi.Update(UserItem.AgencyID, json,CodeLogin(), item.ID);
                    }
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = calendar.ID.ToString(),
                        Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(string.Join(", ", ltsDnCalendarItem.Select(c => c.Name))))
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
