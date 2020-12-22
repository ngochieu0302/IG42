using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class OrderDebtController : BaseApiController
    {
        //
        // GET: /OrderDebt/
        private readonly OrderDebtDA _da = new OrderDebtDA();
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelOrderDebtItem()
                : new ModelOrderDebtItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new OrderDebtItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key)
        {
            var model = new Order_Debt();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.IsDeleted = false;
                model.Datecreate = DateTime.Now.TotalSeconds();
                model.Note = HttpUtility.UrlDecode(model.Note);
                _da.Add(model);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
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
                model.Note = HttpUtility.UrlDecode(model.Note);
                //if (model.IsLook.HasValue && model.IsLook.Value) _da.DeleteAll(ItemId);
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
            var lst = _da.GetListByArrId(lstArrId);
            foreach (var item in lst)
                item.IsDeleted = true;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}
