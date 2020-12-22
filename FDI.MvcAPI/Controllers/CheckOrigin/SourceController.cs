using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA.CheckOrigin;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.CheckOrigin
{
    public class SourceController : BaseApiController
    {
        //
        // GET: /Source/
        readonly SourceDA _da = new SourceDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelSourceItem()
                : new ModelSourceItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new SourceItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetList(string key)
        {
            var obj = key != Keyapi ? new List<SourceItem>() : _da.GetList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var model = new Source();
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                var pictureId = Request["Value_DefaultImages"];
                UpdateModel(model);
                if (!string.IsNullOrEmpty(pictureId))
                {
                    model.PictureID = int.Parse(pictureId);
                }
                model.DateCreate = DateTime.Now.TotalSeconds();
                model.IsDeleted = false;
                model.IsShow = true;
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string json)
        {
            var model = _da.GetById(ItemId);
            
            var msg = new JsonMessage(false, "cập nhật dữ liệu thành công.");
            try
            {
                UpdateModel(model);
                var pictureId = Request["Value_DefaultImages"];
                if (!string.IsNullOrEmpty(pictureId))
                {
                    model.PictureID = int.Parse(pictureId);
                }
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var lst = _da.GetListByArrId(lstArrId);
                    foreach (var item in lst)
                    {
                        item.IsDeleted = true;
                    }
                    _da.Save();
                }
                else
                {
                    msg.Erros = true;
                    msg.Message = "Truy cập thất bại.";
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

    }
}
