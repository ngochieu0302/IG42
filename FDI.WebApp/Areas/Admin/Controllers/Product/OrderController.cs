using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;

using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: /Admin/Order/
        private readonly OrdersDA _orderDa;
        public OrderController()
        {
            _orderDa = new OrdersDA("#");
        }

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
            decimal? total;
            decimal? totalpay;
            decimal? totaldiscount;
            var model = new ModelOrderItem
            {
                ListOrderItem = _orderDa.GetListSimpleByRequest(Request, 2010, out total, out totalpay, out totaldiscount),
                Total = total,
                TotalPay = totalpay,
                PageHtml = _orderDa.GridHtmlPage
            };
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var order = new Shop_Orders();
            if (DoAction == ActionType.Edit)
                order = _orderDa.GetById(ArrId.FirstOrDefault());
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(order);
        }

        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var model = _orderDa.GetById(ArrId.FirstOrDefault());
            return View(model);
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
            var order = new Shop_Orders();
            switch (DoAction)
            {
                case ActionType.Edit:
                    order = _orderDa.GetById(ArrId.FirstOrDefault());
                    UpdateModel(order);
                    _orderDa.Save();
                    msg = new JsonMessage
                    {
                        Erros = false,
                        ID = order.CustomerName,
                        Message = string.Format("Đã xử lý đơn hàng số: <b>#{0}</b>", Server.HtmlEncode(order.CustomerName))
                    };
                    break;

                case ActionType.Delete:
                    var ltsOrder = _orderDa.GetListByArrId(ArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in ltsOrder)
                    {
                        item.IsDelete = true;
                        stbMessage.AppendFormat("Đã xóa đơn hàng <b>{0}</b>.<br />", Server.HtmlEncode(item.CustomerName));
                    }
                    _orderDa.Save();
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
