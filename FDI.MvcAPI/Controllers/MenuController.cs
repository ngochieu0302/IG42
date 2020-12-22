using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class MenuController : BaseApiController
    {
        //
        // GET: /Menus/
        private readonly MenuDA _da = new MenuDA();

        public ActionResult ListItems(int groupId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelMenusItem()
                : new ModelMenusItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid(), groupId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new MenusItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByParentId(string key, int groupId)
        {
            var obj = key != Keyapi ? new List<MenusItem>() : _da.GetListByParentId(groupId, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetListTree(string key, int groupId)
        {
            var obj = key != Keyapi ? new List<TreeViewItem>() : _da.GetListTree(groupId, Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Add(string key)
        {
            var model = new Menu();
            try
            {
                var cateId = Convert.ToInt32(Request["CateId"]);
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                if (cateId > 0)
                    model.CateId = cateId;
                else
                    model.CateId = null;
                model.AgencyID = Agencyid();
                model.LanguageId = "vi";
                model.IsDeleted = false;
                model.Name = HttpUtility.UrlDecode(model.Name);
                _da.Add(model);
                _da.Save();
                return Json(model.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListByArrId(lstInt);
                foreach (var item in lst)
                    _da.Delete(item);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
