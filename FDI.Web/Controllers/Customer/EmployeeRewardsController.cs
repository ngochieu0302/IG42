using System;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using System.Linq;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class EmployeeRewardsController : BaseController
    {
        // GET: /Admin/Order/       
        readonly CustomerRewardAPI _api = new CustomerRewardAPI();
        readonly RewardHistoryAPI _rewardapi = new RewardHistoryAPI();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = _api.GetListUserRequest(UserItem.AgencyID, Request.Url.Query);           
            return View(model);
        }

        public ActionResult AjaxView()
        {
            var date = DateTime.Now;
            var model = _rewardapi.GetList(UserItem.AgencyID, date.Month, date.Year, ArrId.FirstOrDefault());
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var date = DateTime.Now;
            ViewBag.UserRecive = UserItem.LoweredUserName;
            var model = _api.GetCustomerRewardItem(UserItem.AgencyID, ArrId.FirstOrDefault(), date.Month, date.Year,UserItem.UserId);
            return View(model);
        }

        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url, UserItem.AgencyID);
                    break;
                default:
                    msg.Message = "Bạn không được phần quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
