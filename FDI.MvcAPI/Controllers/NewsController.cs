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
    public class NewsController : BaseApiController
    {
        //
        // GET: /MenuGroups/
        private readonly NewsDA _da = new NewsDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelNewsItem()
                : new ModelNewsItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new NewsItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListAuto(string key, string keword, int showLimit)
        {
            var obj = key != Keyapi ? new List<SuggestionsTMNews>() : _da.GetListSimpleByAutoComplete(keword, showLimit);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
      
        public ActionResult Add(string key)
        {
            var model = new News_News();
            try
            {
                var pictureId = Convert.ToInt32(Request["Value_DefaultImages"]);
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.IsDeleted = false;
                model.LanguageId = "vi";
                model.DateCreated = DateTime.Now;
                model.DateUpdated = DateTime.Now;
                if (pictureId > 0)
                    model.PictureID = pictureId;
                model.Title = HttpUtility.UrlDecode(model.Title);
                model.Description = HttpUtility.UrlDecode(model.Description);
                model.Details = HttpUtility.UrlDecode(model.Details);
                model.SEOTitle = HttpUtility.UrlDecode(model.SEOTitle);
                model.SEODescription = HttpUtility.UrlDecode(model.SEODescription);
                model.SEOKeyword = HttpUtility.UrlDecode(model.SEOKeyword);
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
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                if (pictureId > 0)
                    model.PictureID = pictureId;
                model.Title = HttpUtility.UrlDecode(model.Title);
                model.Description = HttpUtility.UrlDecode(model.Description);
                model.Details = HttpUtility.UrlDecode(model.Details);
                model.SEOTitle = HttpUtility.UrlDecode(model.SEOTitle);
                model.SEODescription = HttpUtility.UrlDecode(model.SEODescription);
                model.SEOKeyword = HttpUtility.UrlDecode(model.SEOKeyword);
                model.DateUpdated = DateTime.Now;
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
            var lst = _da.GetListByArrID(lstInt);
            foreach (var item in lst)
                item.IsShow = true;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Hide(string key, string lstArrId)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lstInt = FDIUtils.StringToListInt(lstArrId);
            var lst = _da.GetListByArrID(lstInt);
            foreach (var item in lst)
                item.IsShow = false;
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}
