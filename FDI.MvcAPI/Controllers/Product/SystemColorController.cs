using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;


namespace FDI.MvcAPI.Controllers
{
    public class SystemColorController : BaseApiController
    {
        //
        // GET: /DNActive/

        private readonly System_ColorDA _da = new System_ColorDA();

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelColorItem()
                : new ModelColorItem { ListItem = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll(string key, string code)
        {
            var obj = key != Keyapi ? new List<ColorItem>() : _da.GetAll(Agencyid());
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetById(string key, int id)
        {
            var obj = key != Keyapi ? new System_Color() : _da.GetById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new ColorItem() : _da.GetItemById(id); ;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListByArrId(string key, string lstId)
        {
            var obj = key != Keyapi ? new List<ColorItem>() : _da.GetListByArrId(lstId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Add(string key, string json)
        {
            if (key == Keyapi)
            {
                var obj = new System_Color();
                UpdateModel(obj);
                obj.Name = HttpUtility.UrlDecode(obj.Name);
                _da.Add(obj);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(string key, string json, int id)
        {
            if (key == Keyapi)
            {
                var obj = _da.GetById(ItemId);
                UpdateModel(obj);
                obj.Name = HttpUtility.UrlDecode(obj.Name);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }

            return Json(0, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lst = _da.GetListByArrID(lstArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in lst)
                    {
                        _da.Delete(item);
                        stbMessage.AppendFormat("Đã xóa chuyên mục <b>{0}</b>.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = lstArrId;
                    msg.Message = stbMessage.ToString();
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Show(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Hiển thị dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lst = _da.GetListByArrID(lstArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in lst.Where(m => !m.IsShow))
                    {
                        item.IsShow = true;
                        stbMessage.AppendFormat("Đã hiển thị <b>{0}</b> thành công.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = lstArrId;
                    msg.Message = stbMessage.ToString();
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Hiển thị dữ liệu chưa thành công.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Hide(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Ẩn dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lst = _da.GetListByArrID(lstArrId);
                    var stbMessage = new StringBuilder();
                    foreach (var item in lst.Where(m=>m.IsShow))
                    {
                        item.IsShow = false;
                        stbMessage.AppendFormat("Đã ẩn <b>{0}</b> thành công.<br />", Server.HtmlEncode(item.Name));
                    }
                    msg.ID = lstArrId;
                    msg.Message = stbMessage.ToString();
                    _da.Save();
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Ẩn dữ liệu chưa thành công.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public System_Color UpdateBase(System_Color systemColor, ColorItem ColorItem)
        {
            systemColor.Name = ColorItem.Name;
            systemColor.Value = ColorItem.Value;
            systemColor.Description = ColorItem.Description;
            systemColor.IsShow = ColorItem.IsShow;
            return systemColor;
        }
    }
}
