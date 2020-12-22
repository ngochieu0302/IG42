using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using Resources;
namespace FDI.Web.Controllers
{
    public class BaseController : Controller
    {
        protected static SystemActionItem SystemActionItem { get; set; }
        protected static List<ActionActiveItem> LtsModuleActive { get; set; }
        readonly DNLoginAPI _dnLoginApi = new DNLoginAPI();
        readonly DNModuleAPI _dnModuleApi = new DNModuleAPI();
        readonly DNCalendarAPI _calendarApi = new DNCalendarAPI();
        readonly DNDayOffAPI _dnDayOffApi = new DNDayOffAPI();
        readonly DNWeeklyScheduleAPI _weeklyScheduleApi = new DNWeeklyScheduleAPI();
        public static string UrlG = ConfigurationManager.AppSettings["Url"];
        protected DNUserItem UserItem { get; set; }
        protected string UserName { get; set; }
        public static string Title { get; set; }
        public static string Mid { get; set; }
        protected Guid UserId { get; set; }
        protected bool IsAdmin { get; set; }
        protected string ParentId()
        {
            var codeCookie = HttpContext.Request.Cookies["ParentId"];
            return codeCookie == null ? "0" : codeCookie.Value;
        }
        protected string ModuleId()
        {
            var codeCookie = HttpContext.Request.Cookies["ModuleId"];
            return codeCookie == null ? null : codeCookie.Value;
        }
        public bool CheckAdmin(IEnumerable<string> listRole)
        {
            var lstAdmin = WebConfig.ListAdmin;
            var lstAdminArr = lstAdmin.Split(',');
            return lstAdminArr.Any(role => listRole.Any(m => m.ToLower() == role.ToLower()));
        }
        protected string Module()
        {
            var path = RawUrl();
            var module = path[1];
            return module;
        }
        public void GetTitle(int id)
        {
            var list = _dnModuleApi.GetAllListSimpleItems(UserItem.AgencyID);
            Title = list.Where(m => m.ID == id).Select(m => m.NameModule).FirstOrDefault();
        }
        protected string CodeLogin()
        {
            var codeCookie = HttpContext.Request.Cookies["CodeLogin"];
            return codeCookie == null ? "0" : codeCookie.Value;
        }
        private DNUserItem GetUser(string code)
        {
            return _dnLoginApi.GetUserItemByCode(code);
        }

        private string[] RawUrl()
        {
            var path = Request.Path;
            return path.Split('/');
        }

         //<summary>
         //1.  kiem tra phan quyen khi hien len view - object ltsPermissionrole
         //2.  kiem tra phan quyen khi thuc hien action - object ltsPermission
         //</summary>
         //<author> linhtx </author>
         //<datemodified> 15-Jan-2014 </datemodified>
         //<param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var code = CodeLogin();
            if (code == "0") filterContext.Result = new RedirectResult("/Account/Logon?url=" + Request["url"]);
            else
            {
                UserItem = GetUser(code);
                if (UserItem != null && UserItem.UserName != null && UserItem.RoleId != Guid.Empty)
                {
                    if (Mid != ModuleId())
                    {
                        Mid = ModuleId();
                        GetTitle(ConvertUtil.ToInt32(ModuleId()));
                    }
                    UserName = UserItem.UserName;
                    UserId = UserItem.UserId;
                    SystemActionItem = new SystemActionItem();
                    var moduleArr = RawUrl();
                    var module = moduleArr[1]; // ProductAttribute                    
                    if (moduleArr.Any(m => m.ToLower() == WebConfig.AdminUrl.ToLower()))
                    {
                        var keyCache = "ltsPermissionrole" + code; // ltsPermissionProductAttribute
                        if (HttpRuntime.Cache[keyCache] == null)
                            HttpRuntime.Cache[keyCache] = CheckAdmin(UserItem.listRole);
                        IsAdmin = (bool)HttpRuntime.Cache[keyCache];
                        SystemActionItem.IsAdmin = IsAdmin;
                        if (!SystemActionItem.IsAdmin)
                        {
                            var keyCacheModule = "ltsPermission" + code + "-" + module; // ltsPermissionProductAttribute
                            #region user module active
                            if (HttpRuntime.Cache[keyCacheModule] == null)
                            {
                                LtsModuleActive = UserRoleModule(module);
                                if (LtsModuleActive.Any()) HttpRuntime.Cache[keyCacheModule] = LtsModuleActive;
                                else if (!string.IsNullOrEmpty(module) && module.ToLower() != "admindn")
                                    filterContext.Result = new RedirectResult("/AdminDN/NotRoles");
                                else SystemActionItem.IsAdmin = true;
                            }
                            else LtsModuleActive = HttpRuntime.Cache[keyCacheModule] as List<ActionActiveItem>;
                            #endregion
                        }
                    }
                }
                else filterContext.Result = new RedirectResult("/Account/Logon?url=" + Request["url"]);
            }
        }
        protected string ActionText
        {
            get
            {
                string action = null;
                var doaction = Request["do"];
                if (!string.IsNullOrEmpty(doaction)) ListActionText().TryGetValue(doaction.Trim().ToLower(), out action);
                return action;
            }
        }
        protected ActionType DoAction
        {
            get
            {
                var action = new ActionType();
                var doaction = Request["do"];
                if (!string.IsNullOrEmpty(doaction))
                {
                    if (SystemActionItem.IsAdmin) ListActionType().TryGetValue(doaction.Trim().ToLower(), out action);
                    else
                    {
                        var obj = LtsModuleActive.FirstOrDefault(m => m.NameActive.Trim().ToLower() == doaction.Trim().ToLower());
                        if (obj != null) ListActionType().TryGetValue(doaction.Trim().ToLower(), out action);
                    }
                }
                return action;
            }
        }
        protected List<int> ArrId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["itemId"]))
                {
                    if (Request["itemId"].Contains(","))
                    {
                        return Request["itemId"].Trim().Split(',').Select(o => Convert.ToInt32(o)).ToList();
                    }
                    var ltsTemp = new List<int> { Convert.ToInt32(Request["itemId"]) };
                    return ltsTemp;
                }
                return new List<int>();
            }
        }
        // Dongdt 22/11/2013
        protected List<Guid> GuiId
        {
            get
            {
                if (!string.IsNullOrEmpty(Request["itemId"]))
                {
                    if (Request["itemId"].Contains(",")) return Request["itemId"].Trim().Split(',').Select(Guid.Parse).ToList();
                    var ltsTemp = new List<Guid> { Guid.Parse(Request["itemId"]) };
                    return ltsTemp;
                }
                return new List<Guid>();
            }
        }       
        
        protected List<ActionActiveItem> UserRoleModule(string module)
        {
            return !string.IsNullOrEmpty(module) ? _dnModuleApi.GetlistByTagUserId(module, UserId, UserItem.AgencyID, string.Join(",", UserItem.listRoleID ?? new List<int>()), ParentId(), ModuleId()) : new List<ActionActiveItem>();
        }

        public static Dictionary<string, string> ListActionText()
        {
            var list = new Dictionary<string, string>
                   {
                       {"add", CSResourceString.Add},
                       {"edit", CSResourceString.Edit},
                       {"view", CSResourceString.View},
                       {"delete", CSResourceString.Delete},
                       {"show", CSResourceString.Show},
                       {"hide", CSResourceString.Hide},
                       {"complete", CSResourceString.Complete},
                       {"order", CSResourceString.Order},
                       {"active", CSResourceString.Active},
                       {"notactive", CSResourceString.notActive},
                       {"usermodule", CSResourceString.UserModule},
                       {"rolemodule", CSResourceString.RoleModule}
                   };
            return list;
        }
        public static Dictionary<string, ActionType> ListActionType()
        {
            var list = new Dictionary<string, ActionType>
                   {
                       {"add", ActionType.Add},
                       {"edit", ActionType.Edit},
                       {"view", ActionType.View},
                       {"delete", ActionType.Delete},
                       {"show", ActionType.Show},
                       {"hide", ActionType.Hide},
                       {"complete", ActionType.Complete},
                       {"order", ActionType.Order},
                       {"active", ActionType.Active},
                       {"notactive", ActionType.NotActive},
                       {"usermodule", ActionType.UserModule},
                       {"rolemodule", ActionType.RoleModule},
                       {"excel", ActionType.Excel}
                   };
            return list;
        }
        public static bool CheckAction(string name)
        {
            if (SystemActionItem.IsAdmin) return true;
            var obj = LtsModuleActive.FirstOrDefault(m => m.NameActive.Trim().ToLower() == name);
            return obj != null;
        }
        #region Calendar
        public List<DayOffItem> GetListDayOffItem(DateTime toDate, DateTime endDate, string code)
        {
            var datestart = toDate.TotalSeconds();
            var dateend = endDate.TotalSeconds();
            var listdayoff = _dnDayOffApi.GetAll(UserItem.AgencyID);
            listdayoff = listdayoff.Where(m => (m.Date >= datestart && m.Date <= dateend) || (m.DateEnd >= datestart && m.DateEnd <= dateend) || (m.Date <= datestart && m.DateEnd >= dateend)).ToList();
            foreach (var dayOffItem in listdayoff)
            {
                if (dayOffItem.Date <= datestart) dayOffItem.Date = datestart;
                //if (dayOffItem.DateEnd >= dateend) dayOffItem.DateEnd = dateend;
            }
            return listdayoff;
        }
        public List<WeeklyScheduleItem> GetWeeklyScheduleday(Guid userId, DateTime date)
        {
            var thu = date.Thu();
            var datestart = date.TotalSeconds();
            var listItem = _calendarApi.GetItemByUserId(userId, UserItem.AgencyID);
            listItem = listItem.Where(m => m.DateStart <= datestart && m.DateEnd > datestart).ToList();
            var listWeeklyScheduleItem = _weeklyScheduleApi.GetAllByAgencyId(userId, UserItem.AgencyID);
            foreach (var weeklyScheduleItem in listItem.SelectMany(dnCalendarItem => listWeeklyScheduleItem.Where(m => m.WeeklyID == thu && dnCalendarItem.DNCalendarWeeklySchedule.Any(w => w.MWSID == m.ID))))
            {
                weeklyScheduleItem.IsCalender = true;
            }
            return listWeeklyScheduleItem.Where(m => m.IsCalender).ToList();
        }
        public List<WeeklyScheduleItem> GetListCalendar(DateTime toDate, DateTime endDate)
        {
            var datestart = toDate.TotalSeconds();
            var code = CodeLogin();
            var listWeeklyScheduleItem = _weeklyScheduleApi.GetAll(UserItem.AgencyID, 0);
            var listdayoff = GetListDayOffItem(toDate, endDate, code);
            foreach (var dayOffItem in listdayoff)
            {
                foreach (var weeklyScheduleItem in listWeeklyScheduleItem.Where(m => dayOffItem.Date <= (datestart + (m.WeeklyID - 1) * 86400) && dayOffItem.DateEnd >= (datestart + (m.WeeklyID - 1) * 86400) && !m.IsDayOff))
                {
                    weeklyScheduleItem.IsDayOff = true;
                    weeklyScheduleItem.NameDayOff = dayOffItem.Name;
                }
            }
            return listWeeklyScheduleItem;
        }
        public List<WeeklyScheduleItem> GetListCalendar(DateTime toDate, DateTime endDate, Guid userId)
        {
            var datestart = toDate.TotalSeconds();
            var dateend = endDate.TotalSeconds();
            var code = CodeLogin();
            var listItem = _calendarApi.GetItemByUserId(userId, UserItem.AgencyID);
            listItem = listItem.Where(m => (m.DateStart >= datestart && m.DateStart <= dateend) || (m.DateEnd >= datestart && m.DateEnd <= dateend) || (m.DateStart <= datestart && m.DateEnd >= dateend)).ToList();
            foreach (var dnCalendarItem in listItem.Where(dnCalendarItem => dnCalendarItem.DateEnd > dateend))
            {
                dnCalendarItem.DateEnd = dateend;
            }
            var listWeeklyScheduleItem = _weeklyScheduleApi.GetAllByAgencyId(userId, UserItem.AgencyID);
            foreach (var dnCalendarItem in listItem)
            {
                foreach (var weeklyScheduleItem in listWeeklyScheduleItem.Where(m => dnCalendarItem.DateStart <= (datestart + (m.WeeklyID - 1) * 86400) && dnCalendarItem.DateEnd >= (datestart + (m.WeeklyID - 1) * 86400)))
                {
                    weeklyScheduleItem.CalendarName = dnCalendarItem.Name + weeklyScheduleItem.CalendarName;
                }
            }

            foreach (var weeklyScheduleItem in listWeeklyScheduleItem.Where(m => listItem.Any(c => c.DNCalendarWeeklySchedule.Any(w => w.MWSID == m.ID) && c.DateStart <= (datestart + (m.WeeklyID - 1) * 86400) && c.DateEnd >= (datestart + (m.WeeklyID - 1) * 86400))))
            {
                weeklyScheduleItem.IsCalender = true;
            }

            var listdayoff = GetListDayOffItem(toDate, endDate, code);
            foreach (var dayOffItem in listdayoff)
            {
                foreach (var weeklyScheduleItem in listWeeklyScheduleItem.Where(m => dayOffItem.Date <= (datestart + (m.WeeklyID) * 86400) && dayOffItem.DateEnd >= (datestart + (m.WeeklyID) * 86400)))
                {
                    weeklyScheduleItem.IsDayOff = true;
                    weeklyScheduleItem.NameDayOff = dayOffItem.Name + weeklyScheduleItem.NameDayOff;
                }
            }
            return listWeeklyScheduleItem;
        }
        public List<WeeklyScheduleItem> GetListCalendarByRole(DateTime toDate, DateTime endDate, Guid roleId)
        {
            var datestart = toDate.TotalSeconds();
            var dateend = endDate.TotalSeconds();
            var code = UserItem.AgencyID;
            var listItem = _calendarApi.GetItemByRoleId(code, roleId);
            var listdayoff = _dnDayOffApi.GetAll(UserItem.AgencyID);
            listdayoff = listdayoff.Where(m => (m.Date >= datestart && m.Date < dateend) || (m.DateEnd >= datestart && m.DateEnd < dateend) || (m.Date <= datestart && m.DateEnd > dateend)).ToList();
            foreach (var dayOffItem in listdayoff)
            {
                if (dayOffItem.Date < datestart) dayOffItem.Date = datestart;
                if (dayOffItem.DateEnd > dateend) dayOffItem.DateEnd = dateend;
            }

            listItem = listItem.Where(m => (m.DateStart >= datestart && m.DateStart <= dateend) || (m.DateEnd >= datestart && m.DateEnd <= dateend) || (m.DateStart <= datestart && m.DateEnd >= dateend)).ToList();
            foreach (var dnCalendarItem in listItem.Where(dnCalendarItem => dnCalendarItem.DateEnd > dateend))
            {
                dnCalendarItem.DateEnd = dateend;
            }

            var listWeeklyScheduleItem = _weeklyScheduleApi.GetAllByAgencyId(roleId, UserItem.AgencyID);
            foreach (var dnCalendarItem in listItem)
            {
                foreach (var weeklyScheduleItem in listWeeklyScheduleItem.Where(m => dnCalendarItem.DateStart <= (datestart + (m.WeeklyID - 1) * 86400) && dnCalendarItem.DateEnd >= (datestart + (m.WeeklyID - 1) * 86400)))
                {
                    weeklyScheduleItem.CalendarName = dnCalendarItem.Name + weeklyScheduleItem.CalendarName;
                }
            }
            foreach (var weeklyScheduleItem in listWeeklyScheduleItem.Where(m => listItem.Any(c => c.DNCalendarWeeklySchedule.Any(w => w.MWSID == m.ID) && c.DateStart <= (datestart + (m.WeeklyID - 1) * 86400) && c.DateEnd >= (datestart + (m.WeeklyID - 1) * 86400))))
            {
                weeklyScheduleItem.IsCalender = true;
            }

            foreach (var dayOffItem in listdayoff)
            {
                foreach (var weeklyScheduleItem in listWeeklyScheduleItem.Where(m => dayOffItem.Date <= (datestart + (m.WeeklyID) * 86400) && dayOffItem.DateEnd >= (datestart + (m.WeeklyID) * 86400)))
                {
                    weeklyScheduleItem.IsDayOff = true;
                    weeklyScheduleItem.NameDayOff = dayOffItem.Name + weeklyScheduleItem.NameDayOff;
                }
            }
            return listWeeklyScheduleItem;
        }
        #endregion
    }
}