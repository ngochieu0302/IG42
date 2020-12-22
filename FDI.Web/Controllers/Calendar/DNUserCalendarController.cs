using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class DNUserCalendarController : BaseController
    {
        readonly DNUserAPI _userApi;
        private readonly DNBedDeskAPI _deskApi;
        private readonly DNUserBedDeskAPI _dnUserBed;
        public DNUserCalendarController()
        {
            _userApi = new DNUserAPI();
            _deskApi = new DNBedDeskAPI();
            _dnUserBed = new DNUserBedDeskAPI();
        }
        public ActionResult Index()
        {
            var userId = Request["userid"];
            var Model = new ModelDNUserItem
            {
                UserID = !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : UserId,
                ListItem = _userApi.GetAll(UserItem.AgencyID),
            };
            return View(Model);
        }
		public ActionResult ListItems(string userId)
        {
            var dateA = FDIUtils.WeekDays(DateTime.Now.Year, FDIUtils.GetWeekNumber(DateTime.Now));
            var toDate = dateA[0];
            var endDate = dateA[6];
            var uId = !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : UserId;
            var model = new ModeWeeklyScheduleItem
            {
                UserID = uId,
                ListItem = GetListCalendar(toDate, endDate, uId),
                DateStart = toDate
            };
            return View(model);
        }
        
        public ActionResult AjaxForm()
        {
            DNUserBedDeskItem calendar;
            List<BedDeskItem> list;
            ActionType Do;
            var userid = Request["userid"];
            if (string.IsNullOrEmpty(userid))
            {
                calendar = _dnUserBed.GetById(ArrId.FirstOrDefault());
                list = _deskApi.GetListItemByMWSID(UserItem.AgencyID, calendar.MWSID.Value, ArrId.FirstOrDefault());
                Do = DoAction;
            }
            else
            {
                calendar = new DNUserBedDeskItem
                {
                    UserID = Guid.Parse(userid),
                    MWSID = ArrId.FirstOrDefault()
                };
                list = _deskApi.GetListItemByMWSID(UserItem.AgencyID, ArrId.FirstOrDefault());
                Do = ActionType.Add;
            }

            ViewBag.DNBedDeskItems = list;
            ViewData.Model = calendar;
            ViewBag.Action = Do;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var userCalendar = new DNUserBedDeskItem();

            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(userCalendar);
                        var json = new JavaScriptSerializer().Serialize(userCalendar);
                        if (userCalendar.BedDeskID != 0)
                            _dnUserBed.Add(json);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = userCalendar.UserID.ToString(),
                            Message = string.Format("Đã thêm mới thành công: <b>{0}</b>", Server.HtmlEncode(userCalendar.NameBed))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;
                case ActionType.Edit:
                    try
                    {
                        UpdateModel(userCalendar);
                        if (userCalendar.BedDeskID != 0)
                            _dnUserBed.UpdateBedId(userCalendar.BedDeskID.Value, ArrId.FirstOrDefault());
                        else
                        {
                            _dnUserBed.Delete(ArrId.FirstOrDefault());
                        }
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = userCalendar.UserID.ToString(),
                            Message = string.Format("Đã cập nhật thành công: <b>{0}</b>", Server.HtmlEncode(userCalendar.NameBed))
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
