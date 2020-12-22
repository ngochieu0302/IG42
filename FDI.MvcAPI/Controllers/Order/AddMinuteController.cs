using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.Order
{
    public class AddMinuteController : BaseApiController
    {
        //
        // GET: /MenuGroups/
        private readonly AddMinuteDA _da = new AddMinuteDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelAddMinuteItem()
                : new ModelAddMinuteItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListAllItems()
        {
            var obj = Request["key"] != Keyapi ? new List<AddMinuteItem>() :  _da.ListAllItems();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new AddMinuteItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key)
        {
            var model = new AddMinute();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.IsDeleted = false;
                model.DateCreated = DateTime.Now.TotalSeconds();
                model.Name = HttpUtility.UrlDecode(model.Name);
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
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lstInt = FDIUtils.StringToListInt(lstArrId);
            var lst = _da.GetListByArrId(lstInt);
            foreach (var item in lst)
                item.IsDeleted = true;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Show(string key, string lstArrId)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lstInt = FDIUtils.StringToListInt(lstArrId);
            var lst = _da.GetListByArrId(lstInt);
            foreach (var item in lst)
                item.IsShow = true;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Hide(string key, string lstArrId)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lstInt = FDIUtils.StringToListInt(lstArrId);
            var lst = _da.GetListByArrId(lstInt);
            foreach (var item in lst)
                item.IsShow = false;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}
