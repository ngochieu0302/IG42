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
    public class MenuGroupsController : BaseController
    {
        private readonly MenuGroupsDa _menuGroupsDa;

        public MenuGroupsController()
        {
            _menuGroupsDa = new MenuGroupsDa("#");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var model = new ModelMenuGroupsItem
            {
                ListItem = _menuGroupsDa.GetListSimpleByRequest(Request,Utility.AgencyId),
                PageHtml = _menuGroupsDa.GridHtmlPage
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxView()
        {
            var customerType = _menuGroupsDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = customerType;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var menuGroup = new MenuGroup();
            if (DoAction == ActionType.Edit)
            {
                menuGroup = _menuGroupsDa.GetById(ArrId.FirstOrDefault());
            }
            ViewData.Model = menuGroup;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var model = new MenuGroup();
            List<MenuGroup> ltsMenuGroups;
            StringBuilder stbMessage;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(model);
                        model.CreatedDate = DateTime.Now;
                        model.AgencyID = Utility.AgencyId;
                        _menuGroupsDa.Add(model);
                        _menuGroupsDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.Id.ToString(),
                            Message = string.Format("Đã thêm mới vị trí : <b>{0}</b>", Server.HtmlEncode(model.Name))
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
                        model = _menuGroupsDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(model);
                        _menuGroupsDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.Id.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(model.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsMenuGroups = _menuGroupsDa.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsMenuGroups)
                    {
                        _menuGroupsDa.Delete(item);
                        stbMessage.AppendFormat("Đã xóa <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _menuGroupsDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;
                case ActionType.Show:
                    ltsMenuGroups = _menuGroupsDa.GetListByArrId(ArrId).Where(o => o.IsShow == false).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsMenuGroups)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _menuGroupsDa.Save();
                    msg.ID = string.Join(",", ltsMenuGroups.Select(o => o.Id));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsMenuGroups = _menuGroupsDa.GetListByArrId(ArrId).Where(o => o.IsShow != null && o.IsShow == true).ToList();
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsMenuGroups)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _menuGroupsDa.Save();
                    msg.ID = string.Join(",", ltsMenuGroups.Select(o => o.Id));
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
