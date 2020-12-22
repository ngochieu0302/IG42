using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using Newtonsoft.Json;

namespace FDI.MvcAPI.Controllers
{
    public class DNDocumentFilesController : BaseApiController
    {
        //
        // GET: /ContactOrder/
        private readonly DocumentFilesDA _da = new DocumentFilesDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDocumentFilesItem()
                : new ModelDocumentFilesItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListSimple(string key)
        {
            var obj = key != Keyapi ? new List<DocumentFilesItem>() : _da.GetListSimpleAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemsByID(string key, int id)
        {
            var obj = key != Keyapi ? new DocumentFilesItem() : _da.GetItemsByID(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key)
        {
            var model = new DocumentFile();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AddList(string key, string json)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lst = JsonConvert.DeserializeObject<List<DocumentFile>>(json);
                foreach (var file in lst)
                {
                    _da.Add(file);
                }
                _da.Save();
                return Json(lst.Select(c=>c.ID), JsonRequestBehavior.AllowGet);
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
       
    }
}
