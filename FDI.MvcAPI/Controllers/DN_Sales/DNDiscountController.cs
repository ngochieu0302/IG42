using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA.DA.DN_Sales;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.DN_Sales
{
    public class DNDiscountController : BaseApiController
    {
        //
        // GET: /DNDiscount/
        readonly DNDiscountDA _da = new DNDiscountDA("#");
        public ActionResult ListItems(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNDiscountItem()
                : new ModelDNDiscountItem { ListItems = _da.GetListSimpleByRequest(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDNDiscountItem(string key, int id)
        {
            var obj = key != Keyapi ? new DNDiscountItem() : _da.GetDNDiscountItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, Guid userId)
        {
            var model = new DN_Discount();
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                UpdateModel(model);
                model.IsShow = true;
                model.IsDeleted = false;
                model.UserCreate = userId;
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
        public ActionResult Update(string key, Guid userId)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                UpdateModel(model);
                model.UserUpdate = userId;
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật";
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
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListByArrId(lstInt);
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
