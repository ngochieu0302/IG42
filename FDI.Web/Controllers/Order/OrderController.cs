using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Utils;
namespace FDI.Web.Controllers
{
    public class OrderController : BaseController
    {
        // GET: /Admin/Order/
        private readonly OrderAPI _api = new OrderAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        public ActionResult Index()
        {
            ViewBag.IsAdmin = IsAdmin;
            return View();
        }
        public ActionResult ListItems()
        {            
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query, UserItem.UserId, IsAdmin));
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetOrderItem(ArrId.FirstOrDefault());
            ViewBag.Agency = _agencyApi.GetItemById(UserItem.AgencyID);
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            if (DoAction == ActionType.Edit)
                ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage { Erros = false, Message = "Cập nhật dữ liệu thành công." };
            switch (DoAction)
            {
                case ActionType.Delete:
                    var i = _api.StopOrder(UserItem.AgencyID, ArrId.FirstOrDefault());
                    if (i == 0)
                    {
                        msg.Message = "Có lỗi xảy ra vui lòng kiểm tra lại!";
                        msg.Erros = true;
                    }
                    break;
                default:
                    msg.Message = "Bạn không được phần quyển cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}