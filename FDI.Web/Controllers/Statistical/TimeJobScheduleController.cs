using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Statistical
{
	public class TimeJobScheduleController : BaseController
	{
		private readonly DNTimeJobAPI _timeJob = new DNTimeJobAPI();
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ListItems(int month = 0, int year = 0)
		{
			if (month == 0 || year == 0)
			{
				month = DateTime.Now.Month;
				year = DateTime.Now.Year;
			}
			var model = new ModelDNUserTimeJobItem
			{
				ListItems = _timeJob.GetAllByMonth(month, year, UserItem.AgencyID),
				DayItems = ConvertDateItem.ListDayInMonth(new DateTime(year, month, 1))
			};
			return View(model);
		}
	}
}