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
    public class MenuGroupsController : BaseApiController
    {
        //
        // GET: /MenuGroups/
        private readonly MenuGroupsDa _da = new MenuGroupsDa();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelMenuGroupsItem()
                : new ModelMenuGroupsItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListSimpleAll(string key)
        {
            var obj = key != Keyapi ? new List<MenuGroupsItem>() : _da.GetListSimpleAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new MenuGroupsItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult Add(string key)
        {
            var model = new MenuGroup();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.AgencyID = Agencyid();
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Des = HttpUtility.UrlDecode(model.Des);
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
