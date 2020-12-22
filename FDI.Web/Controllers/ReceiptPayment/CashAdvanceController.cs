using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CashAdvanceController : BaseController
    {
        // GET: /Admin/Order/
        private readonly CashAdvanceAPI _api = new CashAdvanceAPI();
        private readonly DNUserAPI _userApi = new DNUserAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var model = _api.GetListByRequest(UserItem.AgencyID, Request.Url.Query);
            model.UserActive = UserItem.UserId;
            model.IsAdmin = IsAdmin;
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetCashAdvanceItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new FormCashAdvanceItem
            {
                ObjItem = (DoAction == ActionType.Edit) ? _api.GetCashAdvanceItem(ArrId.FirstOrDefault()) : new CashAdvanceItem(),
                Action = DoAction.ToString(),
                AgencyId = UserItem.AgencyID,
                UserId = UserItem.UserId,
                Users = _userApi.GetListAllAgency(UserItem.AgencyID),
                Users1 = _userApi.GetAll(UserItem.AgencyID)
            };
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            JsonMessage msg;
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _api.Add(url);
                    break;
                case ActionType.Edit:
                    msg = _api.Update(url);
                    break;

                case ActionType.Active:
                    msg = _api.Active(string.Join(",", ArrId));
                    msg.ID = string.Join(",", ArrId);
                    break;
                case ActionType.Delete:
                    msg = _api.Delete(string.Join(",", ArrId));
                    msg.ID = string.Join(",", ArrId);
                    break;
                default:
                    msg = new JsonMessage(true, "Bạn chưa được phân quyền cho chức năng này.");
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
