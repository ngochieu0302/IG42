using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class WorkshopController : BaseApiAuthController
    {
        //
        // GET: /Workshop/
        readonly WorkShopDA _da = new WorkShopDA("#");

        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelWorkShopItem()
                : new ModelWorkShopItem { ListItems = _da.GetListSimpleByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new WorkShopItem() : _da.GetItembyId(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, Guid userId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = new P_Workshop();
                    UpdateModel(model);
                    var lst = Request["lstCate"];
                    var lsttmp = Request["lstDetail"];
                    var lstInt = FDIUtils.StringToListInt(lst);
                    if (!string.IsNullOrEmpty(lst))
                    {
                        model.Category_Recipe = _da.GetListArrIdCateRecipe(lst);
                    }
                    if (!string.IsNullOrEmpty(lsttmp))
                    {
                        model.ProductDetail_Recipe = _da.GetListArrIdDetailRecipe(lsttmp);
                    }
                    var date = DateTime.Now.TotalSeconds();
                    model.UserID = userId;
                    model.DateCreated = date;
                    _da.Add(model);
                    _da.Save();
                }
                else
                {
                    msg = new JsonMessage(true, "Có lỗi xảy ra!.");
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, Guid userId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key == Keyapi)
                {
                    var model = _da.GetbyId(ItemId);
                    UpdateModel(model);
                    var lsttmp = Request["lstDetail"];
                    var lst = Request["lstCate"];
                    model.Category_Recipe.Clear();
                    if (!string.IsNullOrEmpty(lst))
                    {
                        model.Category_Recipe = _da.GetListArrIdCateRecipe(lst);
                    }
                    model.ProductDetail_Recipe.Clear();
                    if (!string.IsNullOrEmpty(lsttmp))
                    {
                        model.ProductDetail_Recipe = _da.GetListArrIdDetailRecipe(lsttmp);
                    }
                    _da.Save();
                }
                else
                {
                    msg = new JsonMessage(true, "Có lỗi xảy ra!.");
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            if (key != Keyapi) return Json(msg, JsonRequestBehavior.AllowGet);
            var lst = _da.GetListArrId(lstArrId);
            foreach (var item in lst)
                item.IsDeleted = true;
            _da.Save();
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAll()
        {
            return Json(_da.GetAll());
        }
    }
}
