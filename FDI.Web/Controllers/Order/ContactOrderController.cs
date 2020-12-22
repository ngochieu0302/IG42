using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Utils;
namespace FDI.Web.Controllers
{
    public class ContactOrderController : BaseController
    {
        // GET: /Admin/Order/
        private readonly ContactOrderAPI _api = new ContactOrderAPI();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            return View(_api.ListItems(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult AjaxView()
        {
            var model = _api.GetContactOrderItem(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            if (DoAction == ActionType.Edit)
                ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        /// <summary>
        /// Hứng các giá trị, phục vụ cho thêm, sửa, xóa, ẩn, hiện
        /// </summary>
        /// <returns></returns>
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
                   
                    break;
                case ActionType.Active:
                    
                    break;
                case ActionType.Delete:
                    var i = _api.StopOrder(UserItem.AgencyID, ArrId.FirstOrDefault());
                    if (i == 0)
                    {
                        msg.Message = "Có lỗi xảy ra vui lòng kiểm tra lại!";
                        msg.Erros = true;
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
