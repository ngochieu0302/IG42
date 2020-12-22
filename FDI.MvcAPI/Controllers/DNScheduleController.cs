using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using System.Text;
using FDI.CORE;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
	public class DNScheduleController : BaseApiController
	{
		//
		// GET: /DNSchedule/

		private readonly ScheduleDA _da = new ScheduleDA();
		private readonly WeeklyScheduleDA _BLWeeklySchedule = new WeeklyScheduleDA();
		private readonly DNWeeklyDA _weeklyBl = new DNWeeklyDA();
        public ActionResult ListItems()
		{
			var obj = Request["key"] != Keyapi
				? new ModelScheduleItem()
                : new ModelScheduleItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetAll(string key)
		{
			var obj = key != Keyapi ? new List<ScheduleItem>() : _da.GetAll(); ;
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetById(string key, int id)
		{
			var obj = key != Keyapi ? new DN_Schedule() : _da.GetById(id); ;
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetItemById(string key, int id)
		{
			var obj = key != Keyapi ? new ScheduleItem() : _da.GetItemById(id); ;
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetListByArrId(string key, string lstId)
		{
			var obj = key != Keyapi ? new List<ScheduleItem>() : _da.GetListByArrId(lstId);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetAllByUserId(string key, Guid userId)
		{
			var obj = key != Keyapi ? new List<ScheduleItem>() : _da.GetAllByUserId(userId);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}


		public ActionResult Add(string key, string json)
		{
			if (key == Keyapi)
			{
				var scheduleItem = JsonConvert.DeserializeObject<ScheduleItem>(json);
				var obj = new DN_Schedule();
                scheduleItem.AgencyID = Agencyid();
				UpdateBase(obj, scheduleItem);
				_da.Add(obj);
				_da.Save();

				// insert bảng mapping
				if (!obj.DN_Weekly_Schedule.Any())
				{
					var lstWeekly = _weeklyBl.GetAll();
					foreach (var weeklyItem in lstWeekly)
					{
                        _BLWeeklySchedule.Add(UpdateMapping(weeklyItem.ID, obj.ID, Agencyid()));
					}
					_BLWeeklySchedule.Save();
				}
				return Json(1, JsonRequestBehavior.AllowGet);
			}
			return Json(0, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Update(string key, string json, int id)
		{
			if (key == Keyapi)
			{
				var scheduleItem = JsonConvert.DeserializeObject<ScheduleItem>(json);
				var obj = _da.GetById(id);
                scheduleItem.AgencyID = Agencyid();
				UpdateBase(obj, scheduleItem);
				_da.Save();
				// insert bảng mapping
				if (!obj.DN_Weekly_Schedule.Any())
				{
					var lstWeekly = _weeklyBl.GetAll();
					foreach (var weeklyItem in lstWeekly)
					{
                        _BLWeeklySchedule.Add(UpdateMapping(weeklyItem.ID, obj.ID, Agencyid()));
					}
					_BLWeeklySchedule.Save();
				}
				return Json(1, JsonRequestBehavior.AllowGet);
			}

			return Json(0, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Delete(string key, string lstArrId)
		{
            var msg = new JsonMessage
            {
                Erros = true,
                Message = "Không có hành động nào được thực hiện."
            };
            if (key == Keyapi)
			{
				var lstInt = FDIUtils.StringToListInt(lstArrId);
				var list = _da.GetListByArrId(lstInt);
                var stbMessage = new StringBuilder();
                foreach (var item in list.Where(item => item.DN_Weekly_Schedule.Any()))
                {
                    item.IsDelete = true;
                    stbMessage.AppendFormat("Đã xóa  <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                }
                _da.Save();
                msg.Erros = false;
                msg.ID = lstArrId;
                msg.Message = stbMessage.ToString();
				return Json(msg, JsonRequestBehavior.AllowGet);
			}
			return Json(msg, JsonRequestBehavior.AllowGet);
		}

        public ActionResult Show(string key, string lstArrId)
        {
            var msg = new JsonMessage
            {
                Erros = true,
                Message = "Không có hành động nào được thực hiện."
            };
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var list = _da.GetListByArrId(lstInt);
                var stbMessage = new StringBuilder();
                foreach (var item in list)
                {
                    item.IsShow = true;
                    stbMessage.AppendFormat("Đã hiển thị <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                }
                _da.Save();
                msg.Erros = false;
                msg.ID = lstArrId;
                msg.Message = stbMessage.ToString();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Hide(string key, string lstArrId)
        {
            var msg = new JsonMessage
            {
                Erros = true,
                Message = "Không có hành động nào được thực hiện."
            };
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var list = _da.GetListByArrId(lstInt);
                var stbMessage = new StringBuilder();
                foreach (var item in list)
                {
                    item.IsShow = false;
                    stbMessage.AppendFormat("Đã ẩn <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                }
                _da.Save();
                msg.Erros = false;
                msg.ID = lstArrId;
                msg.Message = stbMessage.ToString();
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public DN_Schedule UpdateBase(DN_Schedule schedule, ScheduleItem scheduleItem)
		{
			schedule.Name = scheduleItem.Name;
			schedule.HoursStart = scheduleItem.HoursStart;
			schedule.MinuteStart = scheduleItem.MinuteStart;
			schedule.HoursEnd = scheduleItem.HoursEnd;
			schedule.MinuteEnd = scheduleItem.MinuteEnd;
			schedule.AgencyID = scheduleItem.AgencyID;
			schedule.IsShow = scheduleItem.IsShow;
			return schedule;
		}

		public DN_Weekly_Schedule UpdateMapping(int weeklyId, int scheduleId, int agencyId)
		{
			var weeklySchedule = new DN_Weekly_Schedule
			{
				WeeklyID = weeklyId,
				ScheduleID = scheduleId,
				AgencyID = agencyId,
				DateCreated = ConvertDate.TotalSeconds(DateTime.Now)
			};
			return weeklySchedule;
		}
	}
}
