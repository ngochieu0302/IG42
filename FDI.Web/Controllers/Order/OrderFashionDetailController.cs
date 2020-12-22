using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
namespace FDI.Web.Controllers
{
    public class OrderFashionDetailController : BaseController
    {
        // GET: /Admin/Order/
        private readonly OrderAPI _api = new OrderAPI();
        private readonly SetupProductionAPI _setupAPI = new SetupProductionAPI();
        readonly DNUserAPI _dnUserApi = new DNUserAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListOrderFashionDetail(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            var model = _api.GetOrderItem(ArrId.FirstOrDefault());
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var model = new OrderDetailItem();
            if (DoAction == ActionType.Edit)
                model = _api.GetOrderDetailItem(ArrId.FirstOrDefault());
                ViewBag.Action = DoAction;
            model.SetupProductionItems = _setupAPI.GetList(UserItem.AgencyID);
            ViewBag.AgencyID = UserItem.AgencyID;
            ViewBag.ActionText = ActionText;
            return View(model);
        }

        public ActionResult AutoUser()
        {
            var query = Request["query"];
            var ltsResults = _dnUserApi.GetListAuto(query, 10, UserItem.AgencyID);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
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