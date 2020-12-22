using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class DNEditScheduleController : BaseApiController
    {
        //
        // GET: /DNSchedule/       

        private readonly EditScheduleDA _da = new EditScheduleDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelEditScheduleItem()
                : new ModelEditScheduleItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAll(string key, decimal dates, decimal datee, int agencyid)
        {
            var obj = key != Keyapi ? new List<EditScheduleItem>() : _da.GetAll(dates, datee, agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key)
        {
            var obj = key != Keyapi ? new List<EditScheduleItem>() : _da.GetAll();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new DN_EDIT_Schedule() : _da.GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new EditScheduleItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var editSchedule = JsonConvert.DeserializeObject<EditScheduleItem>(json);
                var obj = new DN_EDIT_Schedule();
                obj = UpdateBase(obj, editSchedule);
                _da.Add(obj);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var editSchedule = JsonConvert.DeserializeObject<EditScheduleItem>(json);
                var obj = _da.GetById(id);
                obj = UpdateBase(obj, editSchedule);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, List<int> listInt)
        {
            if (key == Keyapi)
            {
                var list = _da.GetListByArrId(listInt);
                foreach (var item in list)
                {
                    _da.Delete(item);
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public DN_EDIT_Schedule UpdateBase(DN_EDIT_Schedule editSchedule, EditScheduleItem editScheduleItem)
        {
            editSchedule.AgencyID = editScheduleItem.AgencyID;
            editSchedule.Date = editScheduleItem.Date;
            editSchedule.Name = editScheduleItem.Name;
            editSchedule.Datecreated = DateTime.Now.TotalSeconds();
            editSchedule.ScheduleID = editScheduleItem.ScheduleID;
            editSchedule.UserChangeId = editScheduleItem.UserChangeId;
            editSchedule.UserId = editScheduleItem.UserId;
            editSchedule.Type = editScheduleItem.Type;
            editSchedule.DayOffId = editScheduleItem.DayOffId;
            return editSchedule;
        }
    }
}
