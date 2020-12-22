using System;
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
    public class CustomerController : BaseController
    {
        private readonly CustomerDA _customerDa;
        private readonly CustomerTypeDA _customerTypeDa;
        private readonly System_CityDA _systemCityDa;
        public CustomerController()
        {
            _customerDa = new CustomerDA("#");
            _customerTypeDa = new CustomerTypeDA("#");
            _systemCityDa = new System_CityDA("#");
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var listcustome = _customerDa.GetListSimpleByRequest(Request);
            var model = new ModelCustomerItem
            {
                ListItem = listcustome,
                PageHtml = _customerDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }
        public ActionResult AjaxView()
        {
            var model = _customerDa.GetById(ArrId.FirstOrDefault());
            return View(model);
        }
        public ActionResult AjaxForm()
        {
            var customer = new Customer();

            ViewBag.ListCustomerType = _customerTypeDa.GetAll();
            ViewBag.ListCity = _systemCityDa.GetListSimpleAll(true);
            ViewBag.ListDistrict = new List<DistrictItem>();
            if (DoAction == ActionType.Edit)
            {
                customer = _customerDa.GetById(ArrId.FirstOrDefault());
            }
            ViewData.Model = customer;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var customer = new Customer();
            List<Customer> ltsCustomerItems;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(customer);
                        var birthday = Request["Birthday_"];
                        customer.Birthday = ConvertUtil.ToDate(birthday);
                        customer.DateCreated = DateTime.Now;
                        customer.IsDelete = false;
                        _customerDa.Add(customer);
                        _customerDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = customer.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", customer.FullName)
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
                        customer = _customerDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(customer);
                        var birthday = Request["Birthday_"];
                        customer.Birthday = ConvertUtil.ToDate(birthday);
                        _customerDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = customer.ID.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(customer.FullName))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsCustomerItems = _customerDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCustomerItems)
                    {
                        item.IsDelete = true;
                        stbMessage.AppendFormat("Đã xóa khách hàng <b>{0}</b>.<br />", item.FullName);
                    }
                    _customerDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    ltsCustomerItems = _customerDa.GetListByArrId(ArrId).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCustomerItems)
                    {
                        item.IsActive = true;
                        stbMessage.AppendFormat("Đã kích hoạt tài khoản <b>{0}</b>.<br />", item.FullName);
                    }
                    _customerDa.Save();
                    msg.ID = string.Join(",", ltsCustomerItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsCustomerItems = _customerDa.GetListByArrId(ArrId).Where(o => o.IsActive != null && o.IsActive.Value).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCustomerItems)
                    {
                        item.IsActive = false;
                        stbMessage.AppendFormat("Đã khóa tài khoản <b>{0}</b>.<br />", Server.HtmlEncode(item.FullName));
                    }
                    _customerDa.Save();
                    msg.ID = string.Join(",", ltsCustomerItems.Select(o => o.ID));
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
        [HttpGet]
        public string CheckByEmail(string email, int customer)
        {
            var result = _customerDa.CheckExitsByEmail(email, customer);
            return result ? "false" : "true";
        }
    }
}
