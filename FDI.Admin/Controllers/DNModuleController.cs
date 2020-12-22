using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Utils;

namespace FDI.Admin.Controllers
{
    public class DNModuleController : BaseController
    {
        private readonly DNModuleDA _moduleDa;
        private readonly STGroupDA _groupDa;

        public DNModuleController()
        {
            _groupDa = new STGroupDA("#");
            _moduleDa = new DNModuleDA("#");
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult JsonTreeCategorySelect()
        {
            var ltsCategory = _moduleDa.GetListAdminTree();
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxTreeSelect()
        {
            return View();
        }

        public ActionResult AjaxSort()
        {
            var ltsSourceCategory = _moduleDa.ListItemByParentID(ArrId.FirstOrDefault());
            ViewData.Model = ltsSourceCategory;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var moduleModel = new DN_Module
            {
                IsShow = true,
                IsAll = true,
                Ord = 0,
                ParentID = (ArrId.Any()) ? ArrId.FirstOrDefault() : 1
            };
            if (DoAction == ActionType.Edit)
            {
                moduleModel = _moduleDa.GetByID(ArrId.FirstOrDefault());
            }
            ViewBag.Group = _groupDa.GetAll();
            ViewBag.PrarentID = _moduleDa.GetListItemByParentID();
            ViewData.Model = moduleModel;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var module = new DN_Module();
            List<DN_Module> ltsModuleItems;
            StringBuilder stbMessage;
            var list = Request["GroupId"];
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(module);
                        module.IsDelete = false;
                        _moduleDa.Add(module);
                        module.ST_Group = _moduleDa.ListST_GroupByArrID(FDIUtils.StringToListInt(list));
                        _moduleDa.Save();
                        if (module.IsAll == true)
                        {
                            _moduleDa.AddItemByIAll(module.ID, list);
                        }
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = module.ID.ToString(),
                            Message = string.Format("Đã thêm mới: <b>{0}</b>", Server.HtmlEncode(module.NameModule))
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
                        module = _moduleDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(module);
                        module.ST_Group.Clear();
                        module.ST_Group = _moduleDa.ListST_GroupByArrID(FDIUtils.StringToListInt(list));
                        _moduleDa.Save();
                        if (module.IsAll == true) _moduleDa.AddItemByIAll(module.ID, list);
                        else _moduleDa.DeleteItemByIAll(module.ID, list);
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = module.ID.ToString(),
                            Message = string.Format("Đã cập nhật : <b>{0}</b>", Server.HtmlEncode(module.NameModule))
                        };
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Instance.LogError(GetType(), ex);
                    }
                    break;


                case ActionType.Delete:
                    ltsModuleItems = _moduleDa.GetListByArrID(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsModuleItems)
                    {
                        item.IsDelete = true;
                        stbMessage.AppendFormat("Đã xóa  <b>{0}</b>.<br />", Server.HtmlEncode(item.NameModule));
                    }
                    msg.ID = string.Join(",", ArrId);
                    _moduleDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsModuleItems = _moduleDa.GetListByArrID(ArrId).Where(o => o.IsShow != true).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsModuleItems)
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị <b>{0}</b>.<br />", Server.HtmlEncode(item.NameModule));
                    }
                    _moduleDa.Save();
                    msg.ID = string.Join(",", ltsModuleItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsModuleItems = _moduleDa.GetListByArrID(ArrId).Where(o => o.IsShow == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsModuleItems)
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn <b>{0}</b>.<br />", Server.HtmlEncode(item.NameModule));
                    }
                    _moduleDa.Save();
                    msg.ID = string.Join(",", ltsModuleItems.Select(o => o.ID));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Order:
                    if (!string.IsNullOrEmpty(Request["OrderValues"]))
                    {
                        var orderValues = Request["OrderValues"];
                        if (orderValues.Contains("|"))
                        {
                            foreach (var keyValue in orderValues.Split('|'))
                            {
                                if (keyValue.Contains("_"))
                                {
                                    var tempCategory = _moduleDa.GetById(Convert.ToInt32(keyValue.Split('_')[0]));
                                    tempCategory.Ord = Convert.ToInt32(keyValue.Split('_')[1]);
                                    _moduleDa.Save();
                                }
                            }
                        }
                        msg.ID = string.Join(",", orderValues);
                        msg.Message = "Đã cập nhật lại thứ tự ";
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

        public ActionResult AutoComplete()
        {
            var term = Request["term"];
            var ltsResults = _moduleDa.GetListSimpleByAutoComplete(term, 10, true, AgencyId);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }
    }
}
