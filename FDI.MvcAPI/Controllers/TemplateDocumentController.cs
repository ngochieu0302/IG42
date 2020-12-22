using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA.DA;
using FDI.MvcAPI.Common;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class TemplateDocumentController : BaseApiController
    {
        //
        // GET: /TemplateDocument/
        readonly TemplateDocumentDA _da = new TemplateDocumentDA("#");
        public JsonResult GetListSimpleByRequest()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelTemplateDocumentItem()
                : new ModelTemplateDocumentItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetList(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new List<TemplateDocumentItem>() : _da.GetList(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTemplateDocItem(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new TemplateDocumentItem()
                : _da.GetTemplateDocItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetItemByIDAndIDDoc(int id, int iddoc)
        {
            var obj = Request["key"] != Keyapi
                ? ""
                : _da.GetItemByIDAndIDDoc(id, iddoc);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new TemplateDocument();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu Chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(TemplateDocument request)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                var model = _da.GetById(request.ID);
                UpdateModel(model);
                _da.Save();

                //  //reupdate template cache
                RenderTemplate.Instance.Remove($"TemplateDocument{model.ID}");
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
