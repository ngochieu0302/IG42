using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Utils;
using FDI.Simple;

namespace FDI.Areas.Admin.Controllers
{
    public class SysPageController : BaseController
    {
        readonly ModulePageDA _da = new ModulePageDA();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AjaxSort()
        {
            var listCategory = _da.GetAllListSimpleByParentId(ArrId.FirstOrDefault());
            return View(listCategory);
        }

        public ActionResult JsonTreeCategorySelect()
        {
            var ltsCategory = _da.GetListTree();
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems()
        {           
            return View();
        }

        public ActionResult AjaxForm()
        {
            var sysPage = new ModulePage
            {
                Sort = 0,
                ParentId = (ArrId.Any()) ? ArrId.FirstOrDefault() : 0,
            };
            if (DoAction == ActionType.Edit)
            {
                sysPage = _da.Get(ArrId.FirstOrDefault());
            }
            ViewBag.ParentID = _da.GetChildByParentId();
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View(sysPage);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var model = new ModulePage();
            List<ModulePage> ltsSysPageItems;
            StringBuilder stbMessage;
            SysPageItem parent;
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(model);
                        model.Type = 0;
                        model.AgencyID = Utility.AgencyId;
                        model.CreateDate = DateTime.Now.TotalSeconds();
                        parent = _da.GetSysPageItem(model.ParentId ?? 0);
                        if (parent != null)
                            model.Level = parent.Level + 1;
                        else
                            model.Level = 1;
                        _da.Add(model);
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.Id.ToString(),
                            Message = string.Format("Đã thêm mới chuyên mục: <b>{0}</b>", Server.HtmlEncode(model.Name))
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
                        model = _da.Get(ArrId.FirstOrDefault());
                        UpdateModel(model);
                        parent = _da.GetSysPageItem(model.ParentId ?? 0);
                        if (parent != null)
                            model.Level = parent.Level + 1;
                        else
                            model.Level = 1;
                        model.Type = !string.IsNullOrEmpty(Request["Type"]) ? Convert.ToInt32(Request["Type"]) : 0;
                        _da.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = model.Id.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(model.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsSysPageItems = _da.GetListByArrId(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsSysPageItems)
                    {
                        try
                        {
                            _da.Delete(item);
                            stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Instance.LogError(GetType(), ex);
                        }
                    }
                    msg.ID = string.Join(",", ArrId);
                    _da.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsSysPageItems = _da.GetListByArrId(ArrId).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsSysPageItems)
                    {
                        try
                        {
                            stbMessage.AppendFormat("Đã hiển thị chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Instance.LogError(GetType(), ex);
                        }
                    }
                    _da.Save();
                    msg.ID = string.Join(",", ltsSysPageItems.Select(o => o.Id));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsSysPageItems = _da.GetListByArrId(ArrId).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsSysPageItems)
                    {
                        try
                        {
                            
                            stbMessage.AppendFormat("Đã ẩn chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Instance.LogError(GetType(), ex);
                        }
                    }
                    _da.Save();
                    msg.ID = string.Join(",", ltsSysPageItems.Select(o => o.Id));
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
                            msg.Message = "Đã cập nhật lại thứ tự chuyên mục";
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

        [HttpGet]
        public string CheckNameAsciiExits(string slug, int id)
        {
            var ascii = !string.IsNullOrEmpty(slug) ? slug : string.Empty;
            var result = _da.CheckTitleAsciiExits(ascii, id);
            return result ? "false" : "true";
        }
    }
}
