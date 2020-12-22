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
    public class DNDrawerController : BaseApiController
    {
        //
        // GET: /ContactOrder/
        private readonly DNDrawerDA _da = new DNDrawerDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNDrawerItem()
                : new ModelDNDrawerItem { ListItem = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListSimple(string key)
        {
            var obj = key != Keyapi ? new List<DNDrawerItem>() : _da.GetListSimple();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemsByID(string key, int id)
        {
            var obj = key != Keyapi ? new DNDrawerItem() : _da.GetItemsByID(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemsCabinetId(string key, int cabinetId)
        {
            var obj = key != Keyapi ? new List<DNDrawerItem>() : _da.GetItemsCabinetId(cabinetId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key)
        {
            var model = new DN_Drawer();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.IsDelete = false;
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Description = HttpUtility.UrlDecode(model.Description);
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
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
                model.IsDelete = false;
                model.Description = HttpUtility.UrlDecode(model.Description);
                model.Name = HttpUtility.UrlDecode(model.Name);
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
                var lstDrawer = _da.GetListByArrId(lstInt);
                foreach (var item in lstDrawer)
                    _da.Delete(item);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
