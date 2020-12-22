using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class SystemMessageTemplateController : BaseController
    {
        //
        // GET: /Admin/SystemMessageTemplate/
        private readonly SystemMessageTemplateDA _systemMessageTemplateDa;
        private readonly DepartmentDA _departmentDA;


        public SystemMessageTemplateController()
        {
            _systemMessageTemplateDa = new SystemMessageTemplateDA("#");
            _departmentDA = new DepartmentDA("#");

        }
        public ActionResult Index()
        {
            var model = new ModelSystemMessageTemplateItem()
            {
                ListItem = _systemMessageTemplateDa.GetListSimpleByRequest(Request),
                PageHtml = _systemMessageTemplateDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        /// <summary>
        /// Load danh sách bản ghi dưới dạng bảng
        /// </summary>
        /// <returns></returns>
        public ActionResult ListItems()
        {
            var model = new ModelSystemMessageTemplateItem()
            {
                ListItem = _systemMessageTemplateDa.GetListSimpleByRequest(Request),
                PageHtml = _systemMessageTemplateDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var customerType = _systemMessageTemplateDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = customerType;
            return View();
        }


        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var systemConfig = new System_MessageTemplate();
            if (DoAction == ActionType.Edit)
            {
                systemConfig = _systemMessageTemplateDa.GetById(ArrId.FirstOrDefault());
            }
            ViewBag.LstDepartment = _departmentDA.GetAll();
            ViewData.Model = systemConfig;
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
            var systemMessageTemplate = new System_MessageTemplate();
            List<System_MessageTemplate> ltsSystemMessageTemplate;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(systemMessageTemplate);
                        systemMessageTemplate.IsDeleted = false;
                        _systemMessageTemplateDa.Add(systemMessageTemplate);
                        _systemMessageTemplateDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = systemMessageTemplate.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(systemMessageTemplate.Name))
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
                        systemMessageTemplate = _systemMessageTemplateDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(systemMessageTemplate);
                        systemMessageTemplate.IsDeleted = false;
                        _systemMessageTemplateDa.Save();

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = systemMessageTemplate.ID.ToString(),
                            Message = string.Format("Đã cập nhật: <b>{0}</b>", Server.HtmlEncode(systemMessageTemplate.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsSystemMessageTemplate = _systemMessageTemplateDa.GetListByArrID(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsSystemMessageTemplate)
                    {
                        _systemMessageTemplateDa.Delete(item);
                        stbMessage.AppendFormat("Đã xóa <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));

                    }
                    msg.ID = string.Join(",", ArrId);
                    _systemMessageTemplateDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    ltsSystemMessageTemplate = _systemMessageTemplateDa.GetListByArrID(ArrId).Where(o => o.IsActive != null && o.IsActive == false && o.IsDeleted == false).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsSystemMessageTemplate)
                    {
                        item.IsActive = true;
                        stbMessage.AppendFormat("Đã hiển thị <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _systemMessageTemplateDa.Save();
                    msg.ID = string.Join(",", ltsSystemMessageTemplate.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsSystemMessageTemplate = _systemMessageTemplateDa.GetListByArrID(ArrId).Where(o => o.IsActive != null && o.IsActive == true && o.IsDeleted == false).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsSystemMessageTemplate)
                    {
                        item.IsActive = false;
                        stbMessage.AppendFormat("Đã ẩn <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _systemMessageTemplateDa.Save();
                    msg.ID = string.Join(",", ltsSystemMessageTemplate.Select(o => o.ID));
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
