using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class ReceiptVouchersController : BaseController
    {
        // GET: /Admin/Order/
        private readonly ReceiptPaymentAPI _api = new ReceiptPaymentAPI();
        private readonly CostTypeAPI _costTypeApi = new CostTypeAPI();
        private readonly DNUserAPI _userApi = new DNUserAPI();
        public ActionResult Index()
        {
            return View(_costTypeApi.GetList(UserItem.AgencyID, (int)Vouchers.Receipt));
        }
        public ActionResult ListItems()
        {
            var model = _api.GetListReceipt(UserItem.AgencyID, Request.Url.Query);
            model.UserActive = UserItem.UserId;
            model.IsAdmin = IsAdmin;
            return View(model);
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetReceiptItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var model = new ReceiptPaymentItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetReceiptItem(ArrId.FirstOrDefault());
            ViewBag.CostType = _costTypeApi.GetList(UserItem.AgencyID,(int)Vouchers.Receipt);
            ViewBag.Action = DoAction;
            ViewBag.AgencyId = UserItem.AgencyID;
            ViewBag.UserCreate = UserItem.UserId;
            ViewBag.User = _userApi.GetListAllAgency(UserItem.AgencyID);
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
                    msg = _api.AddReceipt(url);
                    break;

                case ActionType.Edit:
                    msg = _api.UpdateReceipt(url);
                    break;
                case ActionType.Delete:
                    msg.ID = string.Join(",", ArrId);
                    msg = _api.DeleteReceipt(msg.ID);
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