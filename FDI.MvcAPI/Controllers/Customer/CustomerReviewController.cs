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
    public class CustomerReviewController : BaseApiController
    {
        //
        // GET: /CustomerReview/
        readonly CustomerReviewDA _da = new CustomerReviewDA("#");
        public ActionResult ListItems(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCustomerReviewItem()
                : new ModelCustomerReviewItem { ListItems = _da.GetListSimpleByRequest(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerReviewItem(string key, int id)
        {
            var obj = key != Keyapi ? new CustomerReviewItem() : _da.GetCustomerReviewItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, int agencyId)
        {
            var model = new Customer_Review();
            var msg = new JsonMessage(false, "Cảm ơn bạn đã đánh giá dịch vụ của chúng tôi.!");
            try
            {
                UpdateModel(model);
                model.IsDelete = false;
                model.DateCreate = DateTime.Now.TotalSeconds();
                model.AgencyID = agencyId;
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
