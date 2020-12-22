using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class PaymentVouchersController : BaseController
    {
        // GET: /Admin/Order/
        private readonly ReceiptPaymentAPI _api = new ReceiptPaymentAPI();
        private readonly CostTypeAPI _costTypeApi = new CostTypeAPI();
        private readonly DNUserAPI _userApi = new DNUserAPI();
        public ActionResult Index()
        {
            return View(_costTypeApi.GetList(UserItem.AgencyID, (int)Vouchers.Payment));
        }
        public ActionResult ListItems()
        {
            var model = _api.GetListPayment(UserItem.AgencyID, Request.Url.Query);
            model.UserActive = UserItem.UserId;
            model.IsAdmin = IsAdmin;
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetPaymentItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new FormReceiptPaymentItem
            {
                ObjItem = (DoAction == ActionType.Edit) ? _api.GetPaymentItem(ArrId.FirstOrDefault()) : new ReceiptPaymentItem(),
                Action = DoAction.ToString(),
                AgencyId = UserItem.AgencyID,
                UserId = UserItem.UserId,
                Users = _userApi.GetListAllAgency(UserItem.AgencyID),
                CostTypeItems = _costTypeApi.GetList(UserItem.AgencyID, (int)Vouchers.Payment)
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
                    msg = _api.AddPayment(url);
                    break;
                case ActionType.Edit:
                    msg = _api.UpdatePayment(url);
                    break;

                case ActionType.Active:
                    msg.ID = string.Join(",", ArrId);
                    msg = _api.ActivePayment(msg.ID, UserItem.UserId);
                    break;
                case ActionType.Delete:
                    msg.ID = string.Join(",", ArrId);
                    msg = _api.DeletePayment(msg.ID);
                    break;
                default:
                    msg.Erros = true;
                    msg.Message = "Bạn không được phân quyền cho chức năng này !";
                    break;
            }            
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
