using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.Product
{
    public class ProductController : BaseApiAppIG4Controller
    {
        //
        // GET: /Product/
        readonly Shop_ProductAppIG4DA _da = new Shop_ProductAppIG4DA();
        public ActionResult ListItems(string key, int agencyId)
        {
            var obj = key != Keyapi
                ? new ModelProductAppIG4Item()
                : new ModelProductAppIG4Item { ListItem = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage, Quantity = 0 };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListAuto(string key, string keword, int showLimit, int type = 0)
        {
            var obj = Request["key"] != Keyapi ? new List<SuggestionsProduct>() : _da.GetListAuto(keword, showLimit, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new Shop_Product();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.IsDelete = false;
                model.IsShow = false;
                model.DateCreated = DateTime.Now;
                model.NameAscii = FDIUtils.Slug(model.Name);
                model.Quantity = 0;
                model.QuantityOut = 0;
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
        public ActionResult Update(string key)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                _da.Save();

            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductItem(string key, int id)
        {
            var obj = key != Keyapi ? new ProductAppIG4Item() : _da.GetProductItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Hide(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Ẩn dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsShow = false;
                    }
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
        public ActionResult Show(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Hiển thị dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsShow = true;
                    }
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
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsDelete = true;
                    }
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
    }
}
