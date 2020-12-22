using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using System;

namespace FDI.Areas.Admin.Controllers
{
    public class CustomerReviewsController : BaseController
    {
        //
        // GET: /Admin/CustomerContact/
        private readonly CustomerReviewsDA _da = new CustomerReviewsDA("#");

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var lstNews = _da.GetListRequest(Request);
            var model = new ModelCustomerReviewsItem
            {                
                ListItems = _da.GetListRequest(Request),
                PageHtml = _da.GridHtmlPage
            };
            return View(model);
            
        }

        public ActionResult AjaxView()
        {
            var model = _da.GetById(ArrId.FirstOrDefault());
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            var model = new CustomerReview();
            if (DoAction == ActionType.Edit)
            {
                model = _da.GetById(ArrId.FirstOrDefault());
            }           
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            List<CustomerReview> lst;
            CustomerReview model = new CustomerReview();
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(model);
                        _da.Add(model);
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,                            
                            Message = string.Format("Đã thêm mới: <b>{0}</b>", Server.HtmlEncode(model.Name))
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
                        model = _da.GetById(ArrId.FirstOrDefault());
                        UpdateModel(model);                        
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,                           
                            Message = string.Format("Đã cập nhật: <b>{0}</b>", model.Name)
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;                

                case ActionType.Delete:
                    lst = _da.GetListArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in lst)
                    {
                        _da.Delete(item);
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
