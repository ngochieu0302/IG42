using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.DN
{
    public class CalendarController : BaseController
    {
        //
        // GET: /Calendar/
        readonly DNUserAPI _userApi = new DNUserAPI();
        private readonly DNCalendarAPI _calendarApi = new DNCalendarAPI();
        //private readonly DNUserBedDeskAPI _dnUserBed = new DNUserBedDeskAPI();

        public ActionResult Index()
        {
            var Model = new ModelDNUserItem
            {
                ListItem = _userApi.GetAll(UserItem.AgencyID),
                ListWeekItems = FDIUtils.ListWeekByYear(DateTime.Now.Year)
            };
            return View(Model);
        }

        public ActionResult ListItems(int year = 0, int weekNumber = 0)
        {
            var dateA = year > 0 && weekNumber > 0 ? FDIUtils.WeekDays(year, weekNumber) : FDIUtils.WeekDays(DateTime.Now.Year, FDIUtils.GetWeekNumber(DateTime.Now));
            var toDate = dateA[0];
            var endDate = dateA[6];
            var Model = new ModeWeeklyScheduleItem
            {
                ListItem = GetListCalendar(toDate, endDate, UserId),
                DateStart = toDate
            };
            return View(Model);
        }

        public ActionResult ListCalenderItems()
        {
            var model = _calendarApi.ListItems(UserItem.AgencyID, Request.Url.Query);
            ViewData.Model = model;
            return View();
        }

        public ActionResult WeekByYear(int year)
        {
            var listWeekItems = FDIUtils.ListWeekByYear(year);
            return Json(listWeekItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NumberWeek(int year, int weekNumber)
        {
            var date = FDIUtils.WeekDays(year, weekNumber);
            var model = new ModelDateItem
            {
                Error = false,
                FirstDayOfWeek = date[0].ToString("dd/MM/yyyy"),
                LastDayOfWeek = date[6].ToString("dd/MM/yyyy")
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}
