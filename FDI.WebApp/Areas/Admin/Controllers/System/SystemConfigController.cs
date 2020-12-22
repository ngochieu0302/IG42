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
    public class SystemConfigController : BaseController
    {
        private readonly SystemConfigDA _systemConfigDa;
        public SystemConfigController()
        {
            _systemConfigDa = new SystemConfigDA("#");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelSystemConfigItem
            {
                ListItem = _systemConfigDa.GetListSimpleByRequest(Request),
                PageHtml = _systemConfigDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxView()
        {
            var customerType = _systemConfigDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = customerType;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var systemConfig = new System_Config();
            if (DoAction == ActionType.Edit)
            {
                systemConfig = _systemConfigDa.GetById(ArrId.FirstOrDefault());
            }
            ViewData.Model = systemConfig;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }
       
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var systemConfig = new System_Config();
            List<System_Config> ltsSystemConfigs;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(systemConfig);
                        systemConfig.LanguageId = Fdisystem.LanguageId;
                        systemConfig.DateCreate = DateTime.Now;
                        _systemConfigDa.Add(systemConfig);
                        _systemConfigDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = systemConfig.ID.ToString(),
                            Message = string.Format("Đã thêm mới hành động: <b>{0}</b>", Server.HtmlEncode(systemConfig.Name))
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
                        systemConfig = _systemConfigDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(systemConfig);
                        _systemConfigDa.Save();

                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = systemConfig.ID.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(systemConfig.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsSystemConfigs = _systemConfigDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsSystemConfigs)
                    {
                        _systemConfigDa.Delete(item);
                        stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));

                    }
                    msg.ID = string.Join(",", ArrId);
                    _systemConfigDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    ltsSystemConfigs = _systemConfigDa.GetListByArrId(ArrId).Where(o => o.IsShow != null && !o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsSystemConfigs)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _systemConfigDa.Save();
                    msg.ID = string.Join(",", ltsSystemConfigs.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsSystemConfigs = _systemConfigDa.GetListByArrId(ArrId).Where(o => o.IsShow != null && o.IsShow.Value).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsSystemConfigs)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _systemConfigDa.Save();
                    msg.ID = string.Join(",", ltsSystemConfigs.Select(o => o.ID));
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
