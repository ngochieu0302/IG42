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
    public class DNDocumentLevelController : BaseApiController
    {
        //
        // GET: /ContactOrder/
        private readonly DNDocumentLevelDA _da = new DNDocumentLevelDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNDocumentLevelItem()
                : new ModelDNDocumentLevelItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListSimple(string key)
        {
            var obj = key != Keyapi ? new List<DNDocumentLevelItem>() : _da.GetListSimple(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListItemByParentID(string key)
        {
            var obj = key != Keyapi ? new List<DNDocumentLevelItem>() : _da.GetListItemByParentID(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemsByID(string key, int id)
        {
            var obj = key != Keyapi ? new DNDocumentLevelItem() : _da.GetItemsByID(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key)
        {
            var model = new DN_DocumentLevel();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.AgencyId = Agencyid();
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.IsDeleted = false;
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
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.IsDeleted = false;
                model.AgencyId = Agencyid();
                model.Level = model.ParentID;
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
                var lstLevel = _da.GetListByArrId(lstInt);
                foreach (var item in lstLevel)
                    _da.Delete(item);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
