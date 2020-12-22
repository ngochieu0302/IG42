using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Utils;
namespace FDI.Web.Controllers
{
    public class OrderUserController : BaseController
    {
        // GET: /Admin/Order/
        private readonly OrderAPI _api = new OrderAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListUserItems(Request.Url.Query,UserItem.UserId));
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
            var msg = new JsonMessage();
            switch (DoAction)
            {
                case ActionType.Add:

                    break;
                case ActionType.Edit:
                    if (!User.IsInRole("Admin"))
                    {
                        msg = new JsonMessage
                        {
                            Erros = true,
                            Message = "Bạn chưa được phân quyền cho chức năng này!"
                        };

                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    msg.Message = "Cập nhật thành công!";
                    break;
                case ActionType.Active:
                    msg.Message = "Đơn đặt hàng đã được duyệt!";
                    break;
                case ActionType.Delete:
                    var stbMessage = new StringBuilder();

                    msg.ID = string.Join(",", ArrId);
                    msg.Message = stbMessage.ToString();
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