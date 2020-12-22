using System;
using System.Linq;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Calendar
{
    public class TotalPersonnelController : BaseController
    {
        readonly DNDayOffAPI _dnDayOffApi;
        readonly DNUserAPI _userApi;
        private readonly DNTotalSalaryMonthAPI _totalSalaryMonthApi;
        public TotalPersonnelController()
        {
            _dnDayOffApi = new DNDayOffAPI();
            _userApi = new DNUserAPI();
            _totalSalaryMonthApi = new DNTotalSalaryMonthAPI();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems(int year = 0, int month = 0)
        {
            var date = DateTime.Now;
            if (year == 0 || month == 0) date = new DateTime(date.Year, date.Month, 1);
            else date = new DateTime(year, month, 1);
            var model = new ModelDNUserCalendarItem
            {
                ListItems = _userApi.GetListTotalMonth(UserItem.AgencyID, date.TotalSeconds(), date.AddMonths(1).TotalSeconds()),
                DayOffItems = _dnDayOffApi.GetAll(UserItem.AgencyID),
                DateStart = date
            };
            return View(model);
        }
        
        public ActionResult AjaxForm()
        {
            ViewBag.Action = "Edit";
            ViewBag.ItemId = ArrId.FirstOrDefault();
            var model = _totalSalaryMonthApi.GetItemById(ArrId.FirstOrDefault());
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false };
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Edit:
                    if (_totalSalaryMonthApi.Update(url, UserItem.UserId) == 0)
                    {
                        msg.Erros = true;
                        msg.Message = "Có lỗi xảy ra!";
                    }
                    else msg.Message = "Cập nhật dữ liệu thành công !";
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không được phần quyển cho chức năng này.";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}