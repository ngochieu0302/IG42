using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class ModulePageController : BaseApiController
    {
        readonly ModulePageDL _dl = new ModulePageDL();
        public JsonResult GetListChildByParentId(int parentId, int moduleType, int root)
        {
            var obj = Request["key"] != Keyapi
                ? new List<SysPageItem>() : _dl.GetChildByParentId(parentId, moduleType, root, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChildByParentId(int parentId, int moduleType, int root, int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new List<SysPageItem>() : _dl.GetChildByParentId(parentId, moduleType, root, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListTree()
        {
            var obj = Request["key"] != Keyapi
                ? new List<TreeViewItem>() : _dl.GetListTree(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllListSimpleByParentId(int parentId)
        {
            var obj = Request["key"] != Keyapi
                ? new List<SysPageItem>() : _dl.GetAllListSimpleByParentId(parentId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var obj = Request["key"] != Keyapi
                ? new List<SysPageItem>() : _dl.GetListSimpleByAutoComplete(keyword, showLimit, isShow);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckTitleAsciiExits(string keyword, int id)
        {
            var obj = Request["key"] == Keyapi && _dl.CheckTitleAsciiExits(keyword, id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBykey(string keyword, int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new SysPageItem() : _dl.GetBykey(keyword, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetById(int id)
        {
            var obj = Request["key"] != Keyapi
                 ? new SysPageItem() : _dl.GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}
