using System;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.DN
{
    public class CalenderByUserController : BaseController
    {
        //
        // GET: /CalenderByUser/
        readonly DNUserAPI _userApi;
        public CalenderByUserController()
        {
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
                ListItems = _userApi.GetListCalendarUser(UserItem.AgencyID, dates, datee, UserId),
                WeeklyScheduleItems = GetListCalendar(toDate, endDate),
                DateStart = toDate
            };
            return View(model);
        }
    }
}
