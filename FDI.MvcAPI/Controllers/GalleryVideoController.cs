using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class GalleryVideoController : BaseApiController
    {
        //
        // GET: /GalleryVideo/
        private readonly Gallery_VideoDA _da = new Gallery_VideoDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelVideoItem()
                : new ModelVideoItem { ListItem = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new VideoItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult Add(string key)
        {
            var model = new Gallery_Video();
            try
            {
                var pictureId = Request["Value_DefaultImages"];
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                if (!string.IsNullOrEmpty(pictureId))
                {
                    model.PictureID = int.Parse(pictureId);
                }
                model.IsDeleted = false;
                model.IsShow = true;
                model.LanguageId = "vi";
                model.DateCreated = DateTime.Now.TotalSeconds();
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
                var pictureId = Request["Value_DefaultImages"];
                if (!string.IsNullOrEmpty(pictureId))
                {
                    model.PictureID = int.Parse(pictureId);
                }
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
            var lst = _da.GetListByArrID(lstInt);
            foreach (var item in lst)
                item.IsDeleted = true;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Show(string key, string lstArrId)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lstInt = FDIUtils.StringToListInt(lstArrId);
            var lstId = _da.GetListByArrID(lstInt);
            foreach (var item in lstId)
                item.IsShow = true;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Hide(string key, string lstArrId)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lstInt = FDIUtils.StringToListInt(lstArrId);
            var lstId = _da.GetListByArrID(lstInt);
            foreach (var item in lstId)
                item.IsShow = false;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}
