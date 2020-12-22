using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.Simple;
using FDI.DA;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class CustomerGroupController : BaseApiController
    {
        private readonly CustomerGroupDA _da = new CustomerGroupDA("#");

        public ActionResult GetList(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<CustomerGroupItem>() : _da.GetList(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListItems(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCustomerGroupItem()
                : new ModelCustomerGroupItem { ListItem = _da.GetListRequest(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerGroupItem(string key, int id)
        {
            var obj = key != Keyapi ? new CustomerGroupItem() : _da.GetCustomerGroupItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key)
        {
            var model = new Customer_Groups();
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                UpdateModel(model);
                model.IsDelete = false;
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
                msg.Message = "Dữ liệu chưa được cập nhật";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateDiscount(string key,int cusId)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                var cus = _da.GetCustomerId(cusId);
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
                    var lst = _da.GetListArrId(lstInt);
                    foreach (var item in lst)
                    {
                        item.IsDelete = true;
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
