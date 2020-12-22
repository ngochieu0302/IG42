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
    public class AdvertisingController : BaseApiController
    {
        //
        // GET: /MenuGroups/
        private readonly AdvertisingDA _da = new AdvertisingDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelAdvertisingItem()
                : new ModelAdvertisingItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new AdvertisingItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult Add(string key)
        {
            var model = new Advertising();
            try
            {
                var pictureId = Convert.ToInt32(Request["Value_DefaultImages"]);
                var startDate = Request["_StartDate"];
                var endDate = Request["_EndDate"];
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.IsDeleted = false;
                model.LanguageId = "vi";
                model.CreateOnUtc = DateTime.Now.TotalSeconds();
                model.AgencyID = Agencyid();
                if (pictureId > 0)
                    model.PictureID = pictureId;
                if (!string.IsNullOrEmpty(startDate))
                    model.StartDate = Convert.ToDateTime(startDate).TotalSeconds();
                if (!string.IsNullOrEmpty(endDate))
                    model.EndDate = Convert.ToDateTime(endDate).TotalSeconds();
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
                var pictureId = Convert.ToInt32(Request["Value_DefaultImages"]);
                var startDate = Request["_StartDate"];
                var endDate = Request["_EndDate"];
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                if (pictureId > 0)
                    model.PictureID = pictureId;
                if (!string.IsNullOrEmpty(startDate))
                    model.StartDate = Convert.ToDateTime(startDate).TotalSeconds();
                if (!string.IsNullOrEmpty(endDate))
                    model.EndDate = Convert.ToDateTime(endDate).TotalSeconds();
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
                item.Show = true;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Hide(string key, string lstArrId)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lstInt = FDIUtils.StringToListInt(lstArrId);
            var lst = _da.GetListByArrId(lstInt);
            foreach (var item in lst)
                item.Show = false;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}
