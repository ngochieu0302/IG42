using System;
using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CategoryController : BaseController
    {
        readonly CategoryAPI _categoryApi;
        readonly DNUnitAPI _dnUnitApi = new DNUnitAPI();

        public CategoryController()
        {
            _categoryApi = new CategoryAPI();
        }

        public ActionResult Index()
        {
            return View();
        }
        public JsonResult JsonTreeCategorySelect()
        {
            var ltsCategory = _categoryApi.GetListTree(UserItem.AgencyID);
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxTreeSelect()
        {
            var model = new ModelCategoryItem
            {
                Container = Request["Container"],
                SelectMutil = Convert.ToBoolean(Request["SelectMutil"])
            };            
            return View(model);
        }

        public ActionResult JsonGetListTreeByType(int pageId, string listid)
        {
            var ltsCategory = _categoryApi.GetListTreeByTypeListId(pageId, listid, UserItem.AgencyID);
            return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxTreeCategorySelect(int pageId, string valuesSelected)
        {
            var model = new ModelCategoryItem
            {
                Listid = valuesSelected,
                PageId = pageId
            };
            ViewData.Model = model;
            return View();
        }

        public ActionResult AjaxSort()
        {
            var listCategory = _categoryApi.GetAllListSimpleByParentId(ArrId.FirstOrDefault(), UserItem.AgencyID);
            ViewData.Model = listCategory;
            return View();
        }

        public ActionResult AjaxView()
        {
            var categoryModel = _categoryApi.GetItemById(ArrId.FirstOrDefault());
            ViewData.Model = categoryModel;
            return View();
        }

        public ActionResult AjaxForm()
        {
            var model = new CategoryItem();
            var lstCate = _categoryApi.GetChildByParentId(false);
            ViewBag.lstCate = lstCate;
            ViewBag.Action = DoAction;
            ViewBag.ActionText = ActionText;           
            if (DoAction == ActionType.Edit)
            {
                model = _categoryApi.GetItemById(ArrId.FirstOrDefault());
            }
            model.AgencyId = UserItem.AgencyID;
            ViewBag.lstUnit = _dnUnitApi.GetAllList();
            return View(model);
        }

        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _categoryApi.Add(url,CodeLogin());
                    break;
                case ActionType.Edit:
                    msg = _categoryApi.Update(url,CodeLogin());
                    break;
                case ActionType.Show:
                    msg = _categoryApi.ShowHide(url, true);
                    break;
                case ActionType.Hide:
                    msg = _categoryApi.ShowHide(url, false);
                    break;
                case ActionType.Delete:
                    msg = _categoryApi.Delete(url);
                    break;
                default:
                    msg.Message = "Bạn chưa được phân quyền cho chức năng này.";
                    msg.Erros = true;
                    break;
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
            var ltsResults = _categoryApi.GetListSimpleByAutoComplete(term, 10, true, UserItem.AgencyID);
            return Json(ltsResults, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string CheckNameAsciiExits(string slug, int id)
        {
            var ascii = !string.IsNullOrEmpty(slug) ? slug : string.Empty;
            var result = _categoryApi.CheckTitleAsciiExits(ascii, id);
            return result ? "false" : "true";
        }
    }
}
