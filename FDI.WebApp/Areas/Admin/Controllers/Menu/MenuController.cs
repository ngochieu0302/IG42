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
    public class MenuController : BaseController
    {
        private static int groupid = 1;
        private readonly MenuDA _da = new MenuDA();
        readonly MenuGroupsDa _menuGroupsDa = new MenuGroupsDa();
        readonly CategoryDA _categoryDa = new CategoryDA();
        readonly ModulePageDA _sysPageDa = new ModulePageDA();

        public ActionResult Index()
        {
            var model = _menuGroupsDa.GetListSimpleAll(Utility.AgencyId);
            return View(model);
        }

        public ActionResult AjaxSort()
        {          
            var ltsSourceCategory = _da.GetAllListSimpleByParentId(ArrId.FirstOrDefault(), groupid);
            ViewData.Model = ltsSourceCategory;
            return View();
        }

        public ActionResult JsonTreeCategorySelect(int groupId)
        {
            groupid = groupId;
            var ltsCategory = _da.GetListTree(groupId, Utility.AgencyId);
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems()
        {
            return View();
        }

        /// <summary>
        /// Trang xem chi tiết trong model
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxView()
        {
            var menuModel = _da.GetById(ArrId.FirstOrDefault());
            ViewData.Model = menuModel;
            return View();
        }

        /// <summary>
        /// Form dùng cho thêm mới, sửa. Load bằng Ajax dialog
        /// </summary>
        /// <returns></returns>
        public ActionResult AjaxForm()
        {
            var model = new Menu
            {
                IsShow = true,
                Sort = 0,
                ParentId = (ArrId.Any()) ? ArrId.FirstOrDefault() : 0
            };

            if (DoAction == ActionType.Edit)
                model = _da.GetById(ArrId.FirstOrDefault());
            ViewBag.ParentID = _da.GetListByParentId(groupid, Utility.AgencyId);
            ViewBag.CategoryID = _categoryDa.GetChildByParentId(false);
            ViewBag.PageID = _sysPageDa.GetChildByParentId();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(model);
        }

        // bind data vào text box
        public ActionResult GetCategoryById(int cateId)
        {
            var lstCategory = _categoryDa.GetCategoryById(cateId);
            return Json(lstCategory, JsonRequestBehavior.AllowGet);
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
            var model = new Menu();
            List<Menu> ltsMenuItems;
            StringBuilder stbMessage;
            MenusItem parent;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(model);
                        model.IsDeleted = false;
                        model.AgencyID = Utility.AgencyId;
                        model.LanguageId = Fdisystem.LanguageId;
                        parent = _da.GetItemById(model.ParentId ?? 0);
                        if (parent != null)
                            model.IsLevel = parent.IsLevel + 1;
                        else
                            model.IsLevel = 1;

                        _da.Add(model);
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.Id.ToString(),
                            Type = model.GroupId ?? 0,
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
                        parent = _da.GetItemById(model.ParentId ?? 0);
                        if (parent != null)
                            model.IsLevel = parent.IsLevel + 1;
                        else
                            model.IsLevel = 1;
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.Id.ToString(),
                            Type = model.GroupId ?? 0,
                            Message = string.Format("Đã cập nhật: <b>{0}</b>", model.Name)
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsMenuItems = _da.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsMenuItems)
                    {
                        item.IsDeleted = true;
                        stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _da.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsMenuItems = _da.GetListByArrId(ArrId); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsMenuItems)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _da.Save();
                    msg.ID = string.Join(",", ltsMenuItems.Select(o => o.Id));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsMenuItems = _da.GetListByArrId(ArrId); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsMenuItems)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    _da.Save();
                    msg.ID = string.Join(",", ltsMenuItems.Select(o => o.Id));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Order:
                    try
                    {
                        if (!string.IsNullOrEmpty(Request["OrderValues"]))
                        {
                            var orderValues = Request["OrderValues"];
                            if (orderValues.Contains("|"))
                            {
                                foreach (var keyValue in orderValues.Split('|'))
                                {
                                    if (keyValue.Contains("_"))
                                    {
                                        var tempCategory = _da.GetById(Convert.ToInt32(keyValue.Split('_')[0]));
                                        tempCategory.Sort = Convert.ToInt32(keyValue.Split('_')[1]);
                                        _da.Save();
                                    }
                                }
                            }
                            msg.ID = string.Join(",", orderValues);
                            msg.Message = "Đã cập nhật lại thứ tự";
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
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

        /// <summary>
        /// Dùng cho tra cứu nhanh
        /// </summary>
        /// <returns></returns>
        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _da.GetListSimpleByAutoComplete(term, 10, true);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }
    }
}
