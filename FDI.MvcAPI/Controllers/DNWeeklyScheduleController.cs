using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;


namespace FDI.MvcAPI.Controllers
{
    public class DNWeeklyScheduleController : BaseApiController
    {
        //
        // GET: /DNWeeklySchedule/

        //private readonly DNWeeklyScheduleBL _da = new DNWeeklyScheduleBL();
        private readonly WeeklyScheduleDA _da = new WeeklyScheduleDA();

        public ActionResult GetListSimpleByRequest(string code)
        {
            var obj = Request["key"] != Keyapi ? new List<WeeklyScheduleItem>() : _da.GetListSimpleByRequest(Request);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, int caledarid)
        {
            var obj = key != Keyapi ? new List<WeeklyScheduleItem>() : caledarid == 0 ? _da.GetAll(Agencyid()) : _da.GetAll(Agencyid(), caledarid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetAllByAgencyId(string key, string code, Guid userid, int agencyid)
        {
            agencyid = agencyid > 0 ? agencyid : Agencyid();
            var obj = key != Keyapi ? new List<WeeklyScheduleItem>() : _da.GetAllByAgencyId(agencyid, userid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllUserSchedule(string key, string code, Guid userId, decimal date, int scheduleId)
        {
            var obj = key != Keyapi ? new List<DNUserItem>() : _da.GetAllUserSchedule(Agencyid(), userId, date, scheduleId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_Weekly_Schedule() : _da.GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, WeeklyScheduleItem weeklyScheduleItem)
        {
            if (key == Keyapi)
            {
                var obj = new DN_Weekly_Schedule();
                Update(obj, weeklyScheduleItem);
                _da.Save();
                _da.Add(obj);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, WeeklyScheduleItem weeklyScheduleItem)
        {
            if (key == Keyapi)
            {
                var obj = _da.GetById(weeklyScheduleItem.ID);
                Update(obj, weeklyScheduleItem);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, List<int> listint)
        {
            if (key == Keyapi)
            {
                var list = _da.GetListByArrId(listint);
                foreach (var item in list)
                {
                    _da.Delete(item);
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public DN_Weekly_Schedule Update(DN_Weekly_Schedule weeklySchedule, WeeklyScheduleItem weeklyScheduleItem)
        {
            weeklySchedule.AgencyID = weeklyScheduleItem.AgencyID;
            weeklySchedule.ScheduleID = weeklyScheduleItem.ScheduleID;
            weeklySchedule.WeeklyID = weeklyScheduleItem.WeeklyID;
            weeklySchedule.DateCreated = weeklyScheduleItem.DateCreated;
            return weeklySchedule;
        }
    }
}
