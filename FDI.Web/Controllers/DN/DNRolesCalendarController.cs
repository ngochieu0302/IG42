using System;
using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNRolesCalendarController : BaseController
    {
        readonly DNCalendarAPI _calendarApi;
        readonly DNRoleAPI _roleApi;
        readonly DNWeeklyScheduleAPI _weeklyScheduleDa;
        public DNRolesCalendarController()
        {
            _calendarApi = new DNCalendarAPI();
            _roleApi = new DNRoleAPI();
            _weeklyScheduleDa = new DNWeeklyScheduleAPI();
        }

        public ActionResult Index()
        {
            var roleid = Request["roleid"];
            var model = new ModelDNRolesItem
            {
                RoleId = !string.IsNullOrEmpty(roleid) ? Guid.Parse(roleid) : UserItem.RoleId,
                ListWeekItems = FDIUtils.ListWeekByYear(DateTime.Now.Year),
                ListItem = _roleApi.GetAll(UserItem.AgencyID)
            };
            return View(model);
        }

        public ActionResult ListItems(Guid roleId, int year = 0, int weekNumber = 0)
        {
            roleId = roleId != Guid.Empty ? roleId : UserItem.RoleId;
            var dateA = year > 0 && weekNumber > 0 ? FDIUtils.WeekDays(year, weekNumber) : FDIUtils.WeekDays(DateTime.Now.Year, FDIUtils.GetWeekNumber(DateTime.Now) - 1);
            var toDate = dateA[0];
            var endDate = dateA[6];
            var listWeeklyScheduleItem = GetListCalendarByRole(toDate, endDate, roleId);
            var model = new ModeWeeklyScheduleItem
            {
                ListItem = listWeeklyScheduleItem,
                DateStart = toDate,
                UserID = roleId
            };
            return View(model);
        }
	    public ActionResult ListWeekByYear(int year)
	    {
		    var model = FDIUtils.ListWeekByYear(year);
			return PartialView(model);
	    }

		public ActionResult AjaxView()
        {
            //var calendar = _weeklyScheduleDa.GetAllRole(ArrId.FirstOrDefault(), 1, UserId);
            return View();
        }
       
        public ActionResult AjaxForm()
        {
            var calendar = new DNCalendarItem();
            if (DoAction == ActionType.Edit)
            {
                calendar = _calendarApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewBag.RoleId = _roleApi.GetAll(UserItem.AgencyID);
            ViewData.Model = calendar;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
    }
}
