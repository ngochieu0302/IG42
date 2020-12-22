using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class CustomerTypeController : BaseController
    {
        //
        // GET: /Admin/CustomerType/

        private readonly CustomerTypeDA _customerTypeDa;

        public CustomerTypeController()
        {
            _customerTypeDa = new CustomerTypeDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var listactiveRoleItem = _customerTypeDa.GetListRequest(Request);
            var model = new ModelCustomerTypeItem
            {
                ListItem = listactiveRoleItem,
                PageHtml = _customerTypeDa.GridHtmlPage
            };
            return View(model);
        }
        
        public ActionResult AjaxForm()
        {
            var customerType = new Customer_Type();           
            if (DoAction == ActionType.Edit)
                customerType = _customerTypeDa.GetById(ArrId.FirstOrDefault());

            ViewData.Model = customerType;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var customerType = new Customer_Type();

            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        customerType.IsDelete = false;
                        UpdateModel(customerType);
                        _customerTypeDa.Add(customerType);
                        _customerTypeDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = customerType.ID.ToString(),
                            Message =
                                string.Format("Đã thêm mới hành động: <b>{0}</b>",
                                              Server.HtmlEncode(customerType.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        customerType = _customerTypeDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(customerType);
                        _customerTypeDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = customerType.ID.ToString(),
                            Message =
                                string.Format("Đã cập nhật chuyên mục: <b>{0}</b>",
                                              Server.HtmlEncode(customerType.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    var ltsCustomerTypeItems = _customerTypeDa.GetListByArrId(ArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in ltsCustomerTypeItems)
                    {
                        item.IsDelete = true;
                    }
                    msg.ID = string.Join(",", ArrId);
                    _customerTypeDa.Save();
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
