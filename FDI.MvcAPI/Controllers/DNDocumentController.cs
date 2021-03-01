using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.Messaging;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class DNDocumentController : BaseApiController
    {
        //
        // GET: /ContactOrder/
        private readonly DNDocumentDA _da = new DNDocumentDA();
        private readonly DocumentFilesDA _documentFilesDa = new DocumentFilesDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDocumentItem()
                : new ModelDocumentItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListItemsWarning()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDocumentItem()
                : new ModelDocumentItem { ListItem = _da.GetListWarningSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExcelDocument()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDocumentItem()
                : new ModelDocumentItem { ListItem = _da.ExcelGetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListSimple(string key)
        {
            var obj = key != Keyapi ? new List<DocumentItem>() : _da.GetListSimple(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemsByID(string key, int id)
        {
            var obj = key != Keyapi ? new DocumentItem() : _da.GetItemsByID(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GeneralListStatic(string key, int year, int areaId)
        {
            var obj = key != Keyapi ? new List<StaticDocumentItem>() : _da.GeneralListStatic(year, areaId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var model = new Document();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                var ds = Request["_DateStart"];
                var de = Request["_DateEnd"];

                DateTime Dates = DateTime.ParseExact(ds, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                DateTime Datee = DateTime.ParseExact(ds, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
            

                model.IsShow = true;
                model.IsCancel = false;
                model.Status = false;
                model.CreatedDate = DateTime.Now.TotalSeconds();
                model.DateStart = Dates.TotalSeconds();
                model.DateEnd = Datee.TotalSeconds();
                var lst = FDIUtils.StringToListInt(json);
                if (lst.Count > 0)
                {
                    model.DocumentFiles = _da.GetListFileByArrID(lst);
                }
                _da.Add(model);
                _da.Save();
                return Json(model.ID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(string key, string json)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                var ds = Request["_DateStart"];
                var de = Request["_DateEnd"];

                DateTime Dates = DateTime.ParseExact(ds, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);
                DateTime Datee = DateTime.ParseExact(ds, "dd/MM/yyyy",
                                       System.Globalization.CultureInfo.InvariantCulture);

                model.DateStart = Dates.TotalSeconds();
                model.DateEnd = Datee.TotalSeconds();
                var lstRemove = Request["lstRemove"];
                if (!string.IsNullOrEmpty(lstRemove))
                {
                    var newsListFileRemove = _da.GetListFileByArrID(FDIUtils.StringToListInt(lstRemove));
                    foreach (var file in newsListFileRemove)
                    {
                        model.DocumentFiles.Remove(file);
                    }
                }
                var lst = FDIUtils.StringToListInt(json);
                if (lst.Count > 0)
                {
                    var lstD = _da.GetListFileByArrID(lst);
                    model.DocumentFiles.AddRange(lstD);
                }
                _da.Save();


                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(string key, string lstArrId, string usercan)
        {
            if (key == Keyapi)
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lstDocument = _da.GetListByArrId(lstInt);
                foreach (var item in lstDocument)
                {
                    item.IsCancel = true;
                    item.DateCancel = DateTime.Now.TotalSeconds();
                    item.UserCancel = usercan;
                }
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Active(string key, Guid userId, string lstArrId)
        {
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var model = _da.GetListByArrId(lstInt);
                foreach (var item in model)
                {
                    item.Status = true;
                    item.DateApproval = DateTime.Now.TotalSeconds();
                }
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
