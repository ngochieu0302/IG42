using FDI.Base;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class DiscountController : BaseApiController
    {
        private readonly DiscountDA _da = new DiscountDA();

        public ActionResult ListItems(int agencyid)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDiscountItem()
                : new ModelDiscountItem { ListItem = _da.GetListSimple(Request,agencyid), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetByAllKHDiscount()
        {
            var obj = Request["key"] != Keyapi
                ? new List<DiscountItem>()
                : _da.GetByAllKHDiscount();
            return Json(obj, JsonRequestBehavior.AllowGet);            
        }
        public ActionResult GetItemById(string key, int id)
        {
            var obj = key != Keyapi ? new DiscountItem() : _da.GetItemById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDiscountItem(string key, int type, int agencyid)
        {
            var obj = key != Keyapi ? new List<DiscountItem>() : _da.GetDiscountItem(type, agencyid);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, int agencyid)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new Discount();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                var dates = Request["DateStart_"];
                var datee = Request["DateEnd_"];
                model.AgencyID = agencyid;
                model.Type = 1;
                //var supplierID = Request["SupplierID"];               
                //if (!string.IsNullOrEmpty(supplierID))
                //{
                //    var lstInt = FDIUtils.StringToListInt(supplierID);                   
                //    model.DN_Supplier = _da.GetListSupplier(lstInt);
                //}
                model.DateStart = dates.StringToDecimal();
                model.DateEnd = datee.StringToDecimal();
                _da.Add(model);
                _da.Save();               
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
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
                var dates = Request["DateStart_"];
                var datee = Request["DateEnd_"];
                model.DateStart = dates.StringToDecimal();
                model.DateEnd = datee.StringToDecimal();
                //var supplierID = Request["SupplierID"];
                //model.DN_Supplier.Clear();
                //if (!string.IsNullOrEmpty(supplierID))
                //{
                //    var lstInt = FDIUtils.StringToListInt(supplierID);
                //    model.DN_Supplier = _da.GetListSupplier(lstInt);
                //}
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lst = _da.GetListByArrId(lstArrId);            
            _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được xóa.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Show(string key, string lstArrId)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lst = _da.GetListByArrId(lstArrId);
            
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Hide(string key, string lstArrId)
        {
            if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
            var lst = _da.GetListByArrId(lstArrId);
            
            _da.Save();
            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}