using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ReceiptPaymentController : BaseController
    {
        // GET: /Admin/Order/
        private readonly ReceiptPaymentAPI _api = new ReceiptPaymentAPI();
        private readonly CostTypeAPI _costTypeApi = new CostTypeAPI();
        private readonly DNUserAPI _userApi = new DNUserAPI();
        public ActionResult Index()
        {
            return View(_costTypeApi.GetList(UserItem.AgencyID, (int)Vouchers.ReceiptPayment));
        }
        public ActionResult ListItems()
        {
            var model = IsAdmin ? _api.GetListByRequest(UserItem.AgencyID, Request.Url.Query) : _api.GetListByUserRequest(UserItem.AgencyID, UserItem.UserId, Request.Url.Query);
            model.UserActive = UserItem.UserId;
            model.IsAdmin = IsAdmin;
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetReceiptPaymentItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new FormReceiptPaymentItem
            {
                ObjItem = (DoAction == ActionType.Edit) ? _api.GetReceiptPaymentItem(ArrId.FirstOrDefault()) : new ReceiptPaymentItem(),
                Action = DoAction.ToString(),
                AgencyId = UserItem.AgencyID,
                UserId = UserItem.UserId,
                Users = _userApi.GetListAllKt(UserItem.AgencyID),
                CostTypeItems = _costTypeApi.GetList(UserItem.AgencyID, (int)Vouchers.ReceiptPayment)
            };
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
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
                    msg.ID = string.Join(",", ArrId);
                    msg = _api.Active(msg.ID, UserItem.UserId);
                    break;
                case ActionType.Delete:
                    msg.ID = string.Join(",", ArrId);
                    msg = _api.Delete(msg.ID);
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này!";
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}