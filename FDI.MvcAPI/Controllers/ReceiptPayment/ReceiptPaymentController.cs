using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using System.Linq;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class ReceiptPaymentController : BaseApiController
    {
        //
        // GET: /CostType/
        readonly ReceiptPaymentDA _da = new ReceiptPaymentDA();

        public ActionResult GetListByRequest(string key, int agencyId)
        {
            decimal totalprice;
            decimal totalactive;
            decimal totaldelete;
            var obj = key != Keyapi
                ? new ModelReceiptPaymentItem()
                : new ModelReceiptPaymentItem { ListItem = _da.GetListByRequest(Request, agencyId, out totalprice, out totalactive, out totaldelete), TotalPrice = totalprice, TotalActive = totalactive, TotalDelete = totaldelete, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByUserRequest(string key, int agencyId, Guid user)
        {
            decimal totalprice;
            decimal totalactive;
            decimal totaldelete;
            var obj = key != Keyapi
                ? new ModelReceiptPaymentItem()
                : new ModelReceiptPaymentItem { ListItem = _da.GetListByUserRequest(Request, agencyId, user, out totalprice, out totalactive, out totaldelete), TotalPrice = totalprice, TotalActive = totalactive, TotalDelete = totaldelete, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListReceipt(string key, int agencyId)
        {
            decimal totalprice;
            var obj = key != Keyapi
                ? new ModelReceiptPaymentItem()
                : new ModelReceiptPaymentItem { ListItem = _da.GetListReceipt(Request, agencyId, out totalprice), TotalPrice = totalprice, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListPayment(string key, int agencyId)
        {
            decimal totalprice;
            decimal totalactive;
            decimal totaldelete;
            var obj = key != Keyapi
                ? new ModelReceiptPaymentItem()
                : new ModelReceiptPaymentItem { ListItem = _da.GetListPayment(Request, agencyId, out totalprice, out totalactive, out totaldelete), TotalPrice = totalprice, TotalActive = totalactive, TotalDelete = totaldelete, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GeneralListTotal(string key, int year, int agencyId)
        {
            var obj = key != Keyapi ? new List<GeneralTotalItem>() : _da.GeneralListTotal(year, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetListTransferSlip(string key,int agencyId)
        //{
        //    const int status = (int)Vouchers.ReceiptPayment;
        //    var obj = key != Keyapi
        //        ? new ModelReceiptPaymentItem()
        //        : new ModelReceiptPaymentItem { ListItem = _da.GetListByRequest(Request, agencyId, status), PageHtml = _da.GridHtmlPage };
        //    return Json(obj, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult GetListUserRp(string key, int agencyId)
        {
            var obj = key != Keyapi
                ? new List<ReceiptPaymentItem>()
                : _da.GetListUserRp(Request, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListGeneralTrip(string key, int agencyId)
        {
            var obj = key != Keyapi
                ? new List<ReceiptPaymentItem>()
                : _da.GetListGeneralTrip(Request, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetListGeneralOrder(string key, int agencyId)
        {
            var obj = key != Keyapi
                ? new List<ReceiptPaymentItem>()
                : _da.GetListGeneralOrder(Request, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPaymentItem(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new ReceiptPaymentItem() : _da.GetPaymentItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReceiptItem(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new ReceiptPaymentItem() : _da.GetReceiptItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReceiptPaymentItem(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new ReceiptPaymentItem() : _da.GetReceiptPaymentItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPayment(string key)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                var model = new PaymentVoucher();
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                model.IsDelete = false;
                model.IsActive = false;
                model.Code = DateTime.Now.ToString("yyMMddHHmmss");
                var startDate = Request["DateReturn_"];
                model.DateReturn = startDate.StringToDecimal();
                model.DateCreated = DateTime.Now.TotalSeconds();
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddReceipt(string key)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new ReceiptVoucher();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                model.IsDelete = false;
                model.IsActive = true;
                model.Code = DateTime.Now.ToString("yyMMddHHmmss");
                var startDate = Request["DateReturn_"];
                model.DateReturn = startDate.StringToDecimal();
                model.DateCreated = DateTime.Now.TotalSeconds();
                _da.Add(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new ReceiptPayment();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                model.IsDelete = false;
                model.IsActive = false;
                model.Code = DateTime.Now.ToString("yyMMddHHmmss");
                var startDate = Request["DateReturn_"];
                model.DateReturn = startDate.StringToDecimal();
                model.DateCreated = DateTime.Now.TotalSeconds();
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

        public ActionResult UpdatePayment(string key, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetPaymentById(ItemId);
                if (model != null)
                {
                    UpdateModel(model);
                    model.Note = HttpUtility.UrlDecode(model.Note);
                    var startDate = Request["DateReturn_"];
                    model.DateReturn = startDate.StringToDecimal();
                    _da.Save();
                }
                else
                {
                    msg.Erros = true;
                    msg.Message = "Dữ liệu không được phép chỉnh sửa.";
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu không được phép chỉnh sửa.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateReceipt(string key, string json)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetReceiptById(ItemId);
                if (model != null)
                {
                    UpdateModel(model);
                    model.Note = HttpUtility.UrlDecode(model.Note);
                    var startDate = Request["DateReturn_"];
                    model.DateReturn = startDate.StringToDecimal();
                    _da.Save();
                }
                else
                {
                    msg.Erros = true;
                    msg.Message = "Dữ liệu không được phép chỉnh sửa.";
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu không được phép chỉnh sửa.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(string key, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetById(ItemId);
                if (model != null)
                {
                    UpdateModel(model);
                    model.Note = HttpUtility.UrlDecode(model.Note);
                    var startDate = Request["DateReturn_"];
                    model.DateReturn = startDate.StringToDecimal();
                    _da.Save();
                }
                else
                {
                    msg.Erros = true;
                    msg.Message = "Dữ liệu không được phép chỉnh sửa.";
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Active(string key, string lstArrId, Guid userid)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListArrId(lstInt, userid);
                foreach (var item in lst.Where(c => c.IsDelete == false))
                {
                    item.IsActive = true;
                    item.DateActive = DateTime.Now.TotalSeconds();
                }
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Message = "Phiếu chưa được duyệt.";

            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActivePayment(string key, string lstArrId, Guid userid)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListPaymentArrId(lstInt, userid);
                foreach (var item in lst.Where(c => c.IsDelete == false))
                {
                    item.IsActive = true;
                    item.DateActive = DateTime.Now.TotalSeconds();
                }
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Phiếu chưa được duyệt.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActiveReceipt(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListReceiptArrId(lstInt);
                foreach (var item in lst.Where(c => c.IsDelete == false))
                {
                    item.IsActive = true;
                    item.DateActive = DateTime.Now.TotalSeconds();
                }
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Message = "Phiếu chưa được duyệt.";

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
                    foreach (var item in lst.Where(c => c.IsActive == false))
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
        public ActionResult DeletePayment(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListPaymentArrId(lstInt);
                    foreach (var item in lst.Where(c => c.IsActive == false))
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
        public ActionResult DeleteReceipt(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetListReceiptArrId(lstInt);
                    foreach (var item in lst.Where(c => c.IsActive == false))
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
