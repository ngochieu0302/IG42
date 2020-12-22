using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.DA;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Areas.Menus.Controllers
{
    public class MenusController : Controller
    {
        private readonly MenuBL _bl = new MenuBL();
        private readonly ModuleSettingBL _sysbl = new ModuleSettingBL();
        readonly CategoryBL _categoryBl = new CategoryBL();
        readonly NewsBL _newsBl = new NewsBL();
        public PartialViewResult MainMenu(int ctrId, string url, int cateId = 0, string slug = "Index")
        {
            var model = new ModelMenusItem { CtrId = ctrId };
            if (Request["doAction"] == "setting")
            {
                model.ListMenuGroupsItem = _bl.GetMenusGroup();
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                var urlpath = Request.Path.Split('/');
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 1;
                model.ListItem = _bl.GetListMenus(val);
                model.Slug = slug;
                model.Url = urlpath.Length > 1 ? urlpath[1] : urlpath[0];
                var codeCookie = HttpContext.Request.Cookies["addtocart"];
                if (codeCookie == null)
                {
                    model.CountCart = 0;
                }
                else
                {
                    var temp = codeCookie.Value;
                    var lst = new JavaScriptSerializer().Deserialize<List<ShopProductDetailCartItem>>(temp);
                    model.CountCart = lst.Count;
                }
                return PartialView(model);
            }
        }
        public PartialViewResult MenuFooter(int ctrId, string url)
        {
            var model = new ModelMenusItem { CtrId = ctrId };
            if (Request["doAction"] == "setting")
            {
                model.ListMenuGroupsItem = _bl.GetMenusGroup();
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                var urlpath = Request.Path.Split('/');
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 1;
                model.ListItem = _bl.GetListMenus(val);
                model.Url = urlpath.Length > 1 ? urlpath[1] : urlpath[0];

                return PartialView(model);
            }
        }
        public PartialViewResult MainMenuMobi()
        {
            var model = _bl.GetListMenus(1);
            return PartialView(model);

        }
       

        public PartialViewResult MenuLeft(int ctrId, string url, int cateId = 0)
        {
            var model = new ModelCategoryItem { CtrId = ctrId };
            if (Request["doAction"] == "setting")
            {
                model.ListItem = _bl.GetChildCategories((int)ModuleType.Product);
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 83;
                model.ParentId = val;
                model.ListItem = _categoryBl.GetlistCatebyParent(val);
                return PartialView(model);
            }
        }
        public PartialViewResult MenuPrice(int ctrId, string url, int cateId = 0)
        {
            var model = new ModelCategoryItem { CtrId = ctrId };
            var item = _sysbl.GetKey(ctrId);
            var id = item != null ? int.Parse(item.Value) : 0;
            model.PageId = id;
            if (Request["doAction"] == "setting")
            {
                model.ListItem = _bl.GetChildCategories();
                return PartialView(model);
            }
            model.CtrUrl = url;
            model.ListItem = _bl.GetCategories(id);
            ViewBag.cateId = cateId;
            return PartialView(model);
        }
        public PartialViewResult MenuNews(int ctrId, string url, string slug = "Index")
        {
            var model = new ModelMenusItem { CtrId = ctrId };
            if (Request["doAction"] == "setting")
            {
                model.ListItem = _bl.GetListMenusById(1);
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 0;
                model.ListItem = _bl.GetListMenusById(val);
                model.CategoryItem = _bl.CategoryItem(val);
                return PartialView(model);
            }
        }
        public PartialViewResult MenuService(int ctrId, string url, int cateId = 0)
        {
            var model = new ModelCategoryItem { CtrId = ctrId };
            var item = _sysbl.GetKey(ctrId);
            var id = item != null ? int.Parse(item.Value) : 0;
            if (Request["doAction"] == "setting")
            {
                model.ListItem = _bl.GetChildCategories();
                model.PageId = id;
                return PartialView(model);
            }
            model.CtrUrl = url;
            model.ListItem = _bl.GetCategories(id);
            model.PageId = cateId;
            return PartialView(model);
        }
        public JsonResult Changelanguage(string lang)
        {
            Utility.Setcookie(lang, "LanguageId", 1);
            return Json("0", JsonRequestBehavior.AllowGet);
        } 
    }
}
