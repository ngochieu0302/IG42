using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class CustomerContactController : BaseController
    {
        //
        // GET: /Admin/CustomerContact/
        private readonly CustomerContactDA _da = new CustomerContactDA("#");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var lstNews = _da.GetListRequest(Request);
            var model = new ModelCustomerContactItem
            {
                Container = Request["Container"],
                ListItem = lstNews,
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
            
        }

        public ActionResult AjaxView()
        {
            var model = _da.GetById(ArrId.FirstOrDefault());
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            List<CustomerContact> lst;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Active:
                    lst = _da.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lst)
                    {
                        item.Status = true;
                        msg.Type = item.TypeContact.HasValue ? item.TypeContact.Value : 0;
                        stbMessage.AppendFormat("Đã liên hệ <b>{0}</b><br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _da.Save();

                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Delete:
                    lst = _da.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lst)
                    {
                        item.IsDelete = true;
                        stbMessage.AppendFormat("Đã xóa liên hệ <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _da.Save();
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
