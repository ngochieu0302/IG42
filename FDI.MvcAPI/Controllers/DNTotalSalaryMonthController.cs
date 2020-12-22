using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
	public class DNTotalSalaryMonthController : BaseApiController
	{
		private readonly DNTotalSalaryMonthDA _da = new DNTotalSalaryMonthDA();
		public ActionResult GetListByMonthYear(string key, int month, int year)
		{
			var obj = Request["key"] != Keyapi ? new List<DNTotalSalaryMonthItem>() : _da.GetListByMonthYear(month, year);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetByUseridMonthYear(string key, Guid userId, int month, int year)
		{
			var obj = Request["key"] != Keyapi ? new DNTotalSalaryMonthItem() : _da.GetByUseridMonthYear(userId, month, year);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetByUseridMonthYearDefault(string key, Guid userId, int month, int year)
		{
			var obj = Request["key"] != Keyapi ? new DN_Total_SalaryMonth() : _da.GetByUseridMonthYearDefault(userId, month, year);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetSimpleListUserSalary(string key, int agencyId)
		{
			var obj = Request["key"] != Keyapi ? new List<DNUserSimpleItem>() : _da.GetSimpleListUserSalary(agencyId);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetItemByID(string key, int id)
		{
			var obj = Request["key"] != Keyapi
                ? new DNTotalSalaryMonthItem()
                : _da.GetItemByID(id);
			return Json(obj, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Update(string key, string json)
		{
			try
			{
				if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
				var model = _da.GetById(ItemId);
			    var user = model.UserId;
                UpdateModel(model);
			    model.UserId = user;
				model.Note = HttpUtility.UrlDecode(Request["Note"]);
				model.DateUpdate = ConvertDate.TotalSeconds(DateTime.Now);
				model.UserUpdate = UserId();
				_da.Save();
				return Json(1, JsonRequestBehavior.AllowGet);
			}
			catch (Exception)
			{
				return Json(0, JsonRequestBehavior.AllowGet);
			}
		}
	}
}
