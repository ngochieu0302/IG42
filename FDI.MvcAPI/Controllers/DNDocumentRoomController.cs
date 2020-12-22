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
    public class DNDocumentRoomController : BaseApiController
    {
        //
        // GET: /ContactOrder/
        private readonly DNDocumentRoomDA _da = new DNDocumentRoomDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNDocumentRoomItem()
                : new ModelDNDocumentRoomItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListSimple(string key)
        {
            var obj = key != Keyapi ? new List<DNDocumentRoomItem>() : _da.GetListSimple(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemsByID(string key, int id)
        {
            var obj = key != Keyapi ? new DNDocumentRoomItem() : _da.GetItemsByID(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRoomByLevelId(string key, int levelId)
        {
            var obj = key != Keyapi ? new List<DNDocumentRoomItem>() : _da.GetRoomByLevelId(levelId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key)
        {
            var model = new DN_DocumentRoom();
            try
            {
                var documentLevelId = Request["DocumentLevelID"];
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                if (!string.IsNullOrEmpty(documentLevelId))
                    model.DocumentLevelID = Convert.ToInt32(documentLevelId);
                model.AgencyID = Agencyid();
                model.IsDeleted = false;
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
                var documentLevelId = Request["DocumentLevelID"];
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                if (!string.IsNullOrEmpty(documentLevelId))
                    model.DocumentLevelID = Convert.ToInt32(documentLevelId);
                model.AgencyID = Agencyid();
                model.IsDeleted = false;
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
                var lstRoom = _da.GetListByArrId(lstInt);
                foreach (var item in lstRoom)
                    _da.Delete(item);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
    }
}
