using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Utils;
using FDI.Simple;
using Newtonsoft.Json;


namespace FDI.MvcAPI.Controllers
{
    public class GalleryPictureController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly Gallery_PictureDA _da = new Gallery_PictureDA();

        public ActionResult ListItems(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelPictureItem()
                : new ModelPictureItem { ListItem = _da.GetListSimpleByRequest(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetPictureItem(string key, int id)
        {
            var obj = key != Keyapi ? new PictureItem() : _da.GetPictureItem(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var msg = new JsonMessage(false, "Cập nhât dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = JsonConvert.DeserializeObject<Gallery_Picture>(json);                                
                _da.Add(model);
                _da.Save();
            }
            catch (Exception)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key)
        {
            var msg = new JsonMessage(false, "Cập nhât dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.Name = HttpUtility.UrlDecode(model.Name);
                model.Description = HttpUtility.UrlDecode(model.Description);
                _da.Save();
            }
            catch (Exception)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowHide(string key, string lstArrId, bool showhide)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListArrId(lstInt);
                foreach (var item in lst)
                {
                    item.IsShow = showhide;
                }
                _da.Save();
            }
            catch (Exception ex)
            {
                Log2File.LogExceptionToFile(ex);
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
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
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsDeleted = true;
                    }
                    _da.Save();
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
