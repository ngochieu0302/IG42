using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class MenuDLController : BaseApiController
    {
        readonly MenuDL _dl = new MenuDL();
        public JsonResult GetListMenus(int groupId)
        {
            var obj = Request["key"] != Keyapi
                ? new List<MenusItem>() : _dl.GetListMenus(groupId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMenusGroup(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                 ? new List<MenuGroupsItem>() : _dl.GetMenusGroup(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategories(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                 ? new List<CategoryItem>() : _dl.GetCategories(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCategoriesFooter(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                        ? new List<CategoryItem>() : _dl.GetCategoriesFooter(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetChildCategories(int type, int agencyId)
        {
            var obj = Request["key"] != Keyapi
                        ? new List<CategoryItem>() : _dl.GetChildCategories(type, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}