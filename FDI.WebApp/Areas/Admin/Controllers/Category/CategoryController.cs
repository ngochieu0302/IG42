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
    public class CategoryController : BaseController
    {
        readonly CategoryDA _categoryDa;

        public CategoryController()
        {
            _categoryDa = new CategoryDA();
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult JsonTreeCategorySelect(int type = 0)
        {
            var ltsCategory = _categoryDa.GetListTree(type);
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxTreeSelect()
        {
            var model = new ModelCategoryItem
             {
                 Container = Request["Container"],
                 SelectMutil = Convert.ToBoolean(Request["SelectMutil"])
             };
            ViewData.Model = model;
            return View();
        }

        public ActionResult JsonGetListTreeByType(int pageId, string listid)
        {
            var ltsCategory = _categoryDa.GetListTreeByType(pageId, listid, Utility.AgencyId);
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxTreeCategorySelect(int pageId, string valuesSelected)
        {
            var model = new ModelCategoryItem
            {
                Listid = valuesSelected,
                PageId = pageId
            };            
            return View(model);
        }       

        public ActionResult AjaxView()
        {
            var categoryModel = _categoryDa.GetById(ArrId.FirstOrDefault());
            ViewData.Model = categoryModel;
            return View();
        }
        public ActionResult AjaxSort()
        {
            var listCategory = _categoryDa.GetAllListSimpleByParentId(ArrId.FirstOrDefault(), Utility.AgencyId);
            ViewData.Model = listCategory;
            return View();
        }
        public ActionResult AjaxForm()
        {
            var model = new Category
            {
                IsShow = true,
                Sort = 0,
                ParentId = (ArrId.Any()) ? ArrId.FirstOrDefault() : 0,
            };
            var lstCate = _categoryDa.GetChildByParentId(false);
            ViewBag.lstCate = lstCate;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;
            var item = lstCate.FirstOrDefault(c => c.ID == model.ParentId);
            if (DoAction == ActionType.Edit)
            {
                model = _categoryDa.GetById(ArrId.FirstOrDefault());
                ViewBag.ParentId = item != null ? item.IsLevel : 0;
            }
            else
            {
                ViewBag.ParentId = item != null ? item.IsLevel + 1 : 1;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var category = new Category();
            var parent = new Category();
            List<Category> ltsCategoryItems;
            StringBuilder stbMessage;            
            switch (DoAction)
            {
                case ActionType.Add:
                    try
                    {
                        UpdateModel(category);                        
                        category.LanguageId = Fdisystem.LanguageId;
                        category.IsDeleted = false;
                        category.CreatedDate = DateTime.Now;
                        //category.AgencyId = int.Parse(WebConfig.AgencyId);
                        parent = _categoryDa.GetById(category.ParentId??0);
                        if (parent!=null)
                        {
                            category.IsLevel = parent.IsLevel + 1;
                        }
                        _categoryDa.Add(category);
                        _categoryDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = category.Id.ToString(),
                            Message = string.Format("Đã thêm mới chuyên mục: <b>{0}</b>", Server.HtmlEncode(category.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        Log2File.LogExceptionToFile(ex);
                    }
                    break;

                case ActionType.Edit:
                    try
                    {
                        category = _categoryDa.GetById(ArrId.FirstOrDefault());
                        UpdateModel(category);
                        //category.AgencyId = int.Parse(WebConfig.AgencyId);
                        parent = _categoryDa.GetById(category.ParentId ?? 0);
                        if (parent != null)
                        {
                            category.IsLevel = parent.IsLevel + 1;
                        }
                        category.Type = !string.IsNullOrEmpty(Request["Type"]) ? Convert.ToInt32(Request["Type"]) : 0;                        
                        _categoryDa.Save();
                        msg = new JsonMessage
                        {
                            Erros = false,
                            ID = category.Id.ToString(),
                            Message = string.Format("Đã cập nhật chuyên mục: <b>{0}</b>", Server.HtmlEncode(category.Name))
                        };
                    }
                    catch (Exception ex)
                    {
                        Log2File.LogExceptionToFile(ex);
                    }
                    break;

                case ActionType.Delete:
                    ltsCategoryItems = _categoryDa.GetListByArrId1(ArrId);
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCategoryItems)
                    {
                        try
                        {
                            item.IsDeleted = true;
                            stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {
                            Log2File.LogExceptionToFile(ex);
                        }
                    }
                    msg.ID = string.Join(",", ArrId);
                    _categoryDa.Save();
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Show:
                    ltsCategoryItems = _categoryDa.GetListByArrId1(ArrId).Where(o => o.IsShow == false).ToList(); //Chỉ lấy những đối tượng ko được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCategoryItems)
                    {
                        try
                        {
                            item.IsShow = true;
                            stbMessage.AppendFormat("Đã hiển thị chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {
                            Log2File.LogExceptionToFile(ex);
                        }
                    }
                    _categoryDa.Save();
                    msg.ID = string.Join(",", ltsCategoryItems.Select(o => o.Id));
                    msg.Message = stbMessage.ToString();
                    break;

                case ActionType.Hide:
                    ltsCategoryItems = _categoryDa.GetListByArrId1(ArrId).Where(o => o.IsShow == true).ToList(); //Chỉ lấy những đối tượng được hiển thị
                    stbMessage = new StringBuilder();
                    foreach (var item in ltsCategoryItems)
                    {
                        try
                        {
                            item.IsShow = false;
                            stbMessage.AppendFormat("Đã ẩn chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                        }
                        catch (Exception ex)
                        {
                            Log2File.LogExceptionToFile(ex);
                        }
                    }
                    _categoryDa.Save();
                    msg.ID = string.Join(",", ltsCategoryItems.Select(o => o.Id));
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
                                        var tempCategory = _categoryDa.GetById(Convert.ToInt32(keyValue.Split('_')[0]));
                                        tempCategory.Sort = Convert.ToInt32(keyValue.Split('_')[1]);
                                        _categoryDa.Save();
                                    }
                                }
                            }
                            msg.ID = string.Join(",", orderValues);
                            msg.Message = "Đã cập nhật lại thứ tự chuyên mục";
                        }
                    }
                    catch (Exception ex)
                    {
                        Log2File.LogExceptionToFile(ex);
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
    }
}
