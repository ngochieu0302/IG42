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
    public class DNCabinetDocumentController : BaseApiController
    {
        //
        // GET: /ContactOrder/
        private readonly DNCabinetDocumentDA _da = new DNCabinetDocumentDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNCabinetDocumentItem()
                : new ModelDNCabinetDocumentItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListSimple(string key)
        {
            var obj = key != Keyapi ? new List<DNCabinetDocumentItem>() : _da.GetListSimple(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemsByID(string key, int id)
        {
            var obj = key != Keyapi ? new DNCabinetDocumentItem() : _da.GetItemsByID(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemsByRoomID(string key, int roomId)
        {
            var obj = key != Keyapi ? new List<DNCabinetDocumentItem>() : _da.GetItemsByRoomID(roomId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key)
        {
            var model = new DN_CabinetDocument();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Description = HttpUtility.UrlDecode(model.Description);
                model.AgencyID = Agencyid();
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
                var lstCabinetDocument = _da.GetListByArrId(lstInt);
                foreach (var item in lstCabinetDocument)
                    _da.Delete(item);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
