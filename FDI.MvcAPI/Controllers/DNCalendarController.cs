using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNCalendarController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly DNCalendarDA _da = new DNCalendarDA();
        private readonly DNCalendarWeeklyScheduleDA _calendarWeeklyScheduleBl = new DNCalendarWeeklyScheduleDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNCalendarItem()
                : new ModelDNCalendarItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItemsByUser()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNCalendarItem()
                : new ModelDNCalendarItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid(), UserId()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemByUserId(string key, string code, Guid userid, int agencyid)
        {
            agencyid = agencyid > 0 ? agencyid : Agencyid();
            var obj = key != Keyapi ? new List<DNCalendarItem>() : _da.GetItemByUserId(agencyid, userid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemByUserIdDate(string key, Guid userid, int agencyid, decimal date)
        {
            var obj = key != Keyapi ? new List<CheckInItem>() : _da.GetItemByUserIdDate(agencyid, userid, date);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemByRoleId(string key, string code, Guid roleId)
        {
            var obj = key != Keyapi ? new List<DNCalendarItem>() : _da.GetItemByRoleId(Agencyid(), roleId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLisBedDesk(string key, string code)
        {
            var obj = key != Keyapi ? new List<BedDeskItem>() : _da.GetLisBedDesk(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<DNCalendarItem>() : _da.GetAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_Calendar() : _da.GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DNCalendarItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<DNCalendarItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json,string code)
        {
            if (key == Keyapi)
            {
                var calendar = JsonConvert.DeserializeObject<DNCalendarItem>(json);
                var obj = new DN_Calendar();
                calendar.AgencyId = Agencyid();
                UpdateBase(obj, calendar);

                var lstUser = _da.GetUserArrId(calendar.LstUserIds);
                foreach (var item in lstUser)
                {
                    obj.DN_Users.Add(item);
                }
                // add role
                var lstRoles = _da.GetRolesArrId(calendar.LstRoleIds);
                foreach (var item in lstRoles)
                {
                    obj.DN_Roles.Add(item);
                }
                _da.Add(obj);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUserCalendar(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var calendar = JsonConvert.DeserializeObject<ModelDnUserCalendarItem>(json);
                var obj = _da.GetById(id);
                var lstUser = _da.GetUserArrId(calendar.DnUserId);
                obj.DN_Users.Clear();
                foreach (var item in lstUser)
                {
                    obj.DN_Users.Add(item);
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRolesCalendar(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var calendar = JsonConvert.DeserializeObject<ModelDnUserCalendarItem>(json);
                var obj = _da.GetById(id);
                var lstRoles = _da.GetRolesArrId(calendar.DnRolesId);
                obj.DN_Roles.Clear();
                foreach (var item in lstRoles)
                {
                    obj.DN_Roles.Add(item);
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddCalendarWeeklySchedule(string key, int calenderId, string weeklySchedule)
        {

            if (key == Keyapi)
            {
                var obj = _da.GetByCalendarWeeklyScheduleItemId(calenderId);
                var lstweeklySchedule = FDIUtils.StringToListInt(weeklySchedule);
                foreach (var weeklyScheduleId in lstweeklySchedule.Where(m => obj.DNCalendarWeeklySchedule.All(c => c.MWSID != m)))
                {
                    var item = CalendarWeeklySchedule(weeklyScheduleId, calenderId, Agencyid());
                    _calendarWeeklyScheduleBl.Add(item);
                }
                var listint = obj.DNCalendarWeeklySchedule.Where(m => lstweeklySchedule.All(c => c != m.MWSID)).Select(n => n.ID).ToList();

                if (listint.Any())
                {
                    var list = _calendarWeeklyScheduleBl.GetListByArrId(listint);
                    foreach (var weeklyScheduleId in list)
                    {
                        _calendarWeeklyScheduleBl.Delete(weeklyScheduleId);
                    }
                }
                _calendarWeeklyScheduleBl.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id,string code)
        {
            if (key == Keyapi)
            {
                var dayOff = JsonConvert.DeserializeObject<DNCalendarItem>(json);
                var obj = _da.GetById(id);
                dayOff.AgencyId = Agencyid();
                UpdateBase(obj, dayOff);
                var lstUser = _da.GetUserArrId(dayOff.LstUserIds);
                obj.DN_Users.Clear();
                foreach (var item in lstUser)
                {
                    obj.DN_Users.Add(item);
                }
                // add role
                obj.DN_Roles.Clear();
                var lstRoles = _da.GetRolesArrId(dayOff.LstRoleIds);
                foreach (var item in lstRoles)
                {
                    obj.DN_Roles.Add(item);
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult Delete(string key, List<int> listint)
        //{
        //    if (key == Keyapi)
        //    {
        //        var list = _dl.GetListByArrId(listint);
        //        foreach (var item in list)
        //        {
        //            _dl.Delete(item);
        //        }
        //        _dl.Save();
        //        return Json(1, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(0, JsonRequestBehavior.AllowGet);
        //}

        public DN_Calendar UpdateBase(DN_Calendar calendar, DNCalendarItem calendarItem)
        {
            calendar.AgencyID = calendarItem.AgencyId;
            calendar.Name = calendarItem.Name;
            calendar.DateCreated = calendarItem.DateCreated;
            calendar.DateStart = calendarItem.DateStart;
            calendar.DateEnd = calendarItem.DateEnd;
            calendar.Sort = calendarItem.Sort;
            calendar.IsShow = calendarItem.IsShow;
            calendar.IsDelete = calendarItem.IsDelete;
            return calendar;
        }

        public DN_Calendar_Weekly_Schedule CalendarWeeklySchedule(int weeklyScheduleId, int calenderId, int agencyId)
        {
            var dnCalendarWeeklySchedule = new DN_Calendar_Weekly_Schedule
            {
                MWSID = weeklyScheduleId,
                CalenderID = calenderId,
                AgencyID = agencyId
            };
            return dnCalendarWeeklySchedule;
        }
    }
}
