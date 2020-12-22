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
    public class CalendarByUserController : BaseController
    {
        readonly DNEDITScheduleAPI _dneditScheduleApi;
        readonly DNUserAPI _userApi;
        public CalendarByUserController()
        {
            _dneditScheduleApi = new DNEDITScheduleAPI();
            _userApi = new DNUserAPI();
        }

        public ActionResult Index()
        {
            var year = !string.IsNullOrEmpty(Request["year"]) ? ConvertUtil.ToInt32(Request["year"]) : DateTime.Now.Year;
            var Model = new ModelDNUserItem
            {
                Year = year,
                //ListItem = _userApi.GetAll(UserItem.AgencyID),
                ListWeekItems = FDIUtils.ListWeekByYear(year)
            };
            return View(Model);
        }

        public ActionResult ListItems(int year, int weekNumber = 1)
        {
            var dateA =  FDIUtils.WeekDays(year, weekNumber);
            var toDate = dateA[0];
            var endDate = dateA[6];
            var dates = toDate.TotalSeconds();
            var datee = endDate.TotalSeconds();
            var model = new ModelDNUserCalendarItem
            {
                ListItems = _userApi.GetListCalendar(UserItem.AgencyID, dates, datee),
                WeeklyScheduleItems = GetListCalendar(toDate, endDate),
                DateStart = toDate
            };
            return View(model);
        }
		
		public ActionResult AjaxForm(Guid userid, int sid, decimal date, int id = 0)
        {
            var doaction = "Add";
            var editSchedule = new EditScheduleItem
            {
                UserId = userid,
                ScheduleID = sid,
                Date = date,
            };
            if (id != 0)
            {
                editSchedule = _dneditScheduleApi.GetItemById(UserItem.AgencyID, id);
                doaction = "Edit";
            }
            ViewBag.UserChangeId = _userApi.ListUserNotSId(UserItem.AgencyID, userid, sid, date);
            ViewData.Model = editSchedule;
            ViewBag.Action = doaction;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var editSchedule = new EditScheduleItem
            {
                Date = Convert.ToDecimal(Request["Date"]),
                Type = Convert.ToInt32(Request["Type"]),
                ScheduleID = Convert.ToInt32(Request["ScheduleID"]),
                UserId = Guid.Parse(Request["UserId"]),
                Name = Request["Name"],
                AgencyID = UserItem.AgencyID
            };
            var UserId = Request["UserChangeId"];
            var json = "";
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        editSchedule.Datecreated = DateTime.Now.TotalSeconds();
                        if (!string.IsNullOrEmpty(UserId))
                            editSchedule.UserChangeId = Guid.Parse(UserId);
                        json = new JavaScriptSerializer().Serialize(editSchedule);
                        _dneditScheduleApi.Add(UserItem.AgencyID, json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = editSchedule.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>",Server.HtmlEncode(editSchedule.Name))
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
                        if (!string.IsNullOrEmpty(UserId))
                            editSchedule.UserChangeId = Guid.Parse(UserId);
                        json = new JavaScriptSerializer().Serialize(editSchedule);
                        _dneditScheduleApi.Update(UserItem.AgencyID, json, ArrId.FirstOrDefault());
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = editSchedule.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(editSchedule.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
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
