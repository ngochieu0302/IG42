using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNUserCalendarUpdateController : BaseController
    {
        readonly DNCalendarAPI _calendarApi;
        readonly DNUserAPI _userApi;
        public DNUserCalendarUpdateController()
        {
            _calendarApi = new DNCalendarAPI();
            _userApi = new DNUserAPI();
        }
        public ActionResult Index()
        {
            return View(_userApi.GetAll(UserItem.AgencyID));
        }

        public ActionResult ListItems(string userId, int year = 0, int weekNumber = 0)
        {
            var model = new ModelDNCalendarItem
            {
                //ListItem = _calendarApi.GetListSimpleByRequest(UserItem.AgencyID)
                //PageHtml = _dnUserApi.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }


        public ActionResult NumberWeek(int year, int weekNumber)
        {
            var date = FDIUtils.WeekDays(year, weekNumber);
            var model = new ModelDateItem()
            {
                Error = false,
                FirstDayOfWeek = date[0].ToString("dd/MM/yyyy"),
                LastDayOfWeek = date[6].ToString("dd/MM/yyyy")
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxForm()
        {
            var calendar = new DNCalendarItem();
            if (DoAction == ActionType.Edit)
            {
                calendar = _calendarApi.GetItemById(UserItem.AgencyID, ArrId.FirstOrDefault());
            }
            ViewBag.UserId = _userApi.GetAll(UserItem.AgencyID);
            ViewData.Model = calendar;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var userCalendar = new ModelDnUserCalendarItem();
            var json = "";
            var lstId = Request["itemId"];
            switch (DoAction)
            {
                case ActionType.Edit:
                    try
                    {
                        UpdateModel(userCalendar);
                        userCalendar.DnUserId = Request["UserId"];
                        json = new JavaScriptSerializer().Serialize(userCalendar);
                        _calendarApi.AddUserCalendar(UserItem.AgencyID, json, ArrId.FirstOrDefault());
                        msg = new JsonMessage
                        {
                            Erros = false,
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(userCalendar.DnUserId))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;
            }

            if (string.IsNullOrEmpty(msg.Message))
            {
                msg.Message = "Không có hành động nào được thực hiện.";
                msg.Erros = true;
            }

            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
