using System.Text;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
namespace FDI.Web.Controllers
{
    public class ProcessContactOrderController : BaseController
    {
        // GET: /Admin/Order/
        private readonly DNLevelRoomAPI _levelRoomApi = new DNLevelRoomAPI();
        private readonly DNBedDeskAPI _dnBedDeskApi = new DNBedDeskAPI();
        private readonly ContactOrderAPI _contactOrderApi = new ContactOrderAPI();

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
            var model = new ModelContactOrderItem
            {
                ListItem = _contactOrderApi.ListItemByDay(UserItem.AgencyID),
            };
            return View(model);
        }

        public ActionResult AjaxView()
        {
           return View();
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
