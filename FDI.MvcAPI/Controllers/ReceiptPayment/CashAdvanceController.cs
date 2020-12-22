using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;
using System.Linq;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class CashAdvanceController : BaseApiController
    {
        //
        // GET: /CostType/
        readonly CashAdvanceDA _da = new CashAdvanceDA();

        public ActionResult GetListByRequest(string key, int agencyId)
        {
            decimal totalprice;
            decimal totalactive;
            decimal totaldelete;
            var obj = key != Keyapi
                ? new ModelCashAdvanceItem()
                : new ModelCashAdvanceItem { ListItem = _da.GetListByRequest(Request, agencyId, out totalprice, out totalactive, out totaldelete), TotalPrice = totalprice, TotalActive = totalactive, TotalDelete = totaldelete, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCashAdvanceItem(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new CashAdvanceItem() : _da.GetCashAdvanceItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                var model = new CashAdvance();
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                model.IsDelete = false;
                model.IsActive = false;
                model.Code = DateTime.Now.ToString("yyMMddHHmm");
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

        public ActionResult Update(string key, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công !");
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
                msg.Message = "Dữ liệu chưa được chỉnh sửa.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Active(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Duyệt dữ liệu thành công !");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var lstInt = FDIUtils.StringToListInt(lstArrId);
                var lst = _da.GetListArrId(lstInt);
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

        public ActionResult Delete(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Đã xóa dữ liệu thành công !");
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

        //Trả ứng
        public ActionResult GetListRepayByRequest(string key, int agencyId)
        {
            decimal totalprice;
            decimal totalactive;
            decimal totaldelete;
            var obj = key != Keyapi
                ? new ModelCashAdvanceItem()
                : new ModelCashAdvanceItem { ListItem = _da.GetListRepayByRequest(Request, agencyId, out totalprice, out totalactive, out totaldelete), TotalPrice = totalprice, TotalActive = totalactive, TotalDelete = totaldelete, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRepayItem(string key, int id)
        {
            var obj = Request["key"] != Keyapi ? new CashAdvanceItem() : _da.GetRepayItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddRepay(string key)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công !");
            try
            {
                var model = new Repay();
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                model.IsDelete = false;
                model.IsActive = true;
                model.Code = DateTime.Now.ToString("yyMMddHHmm");
                var startDate = Request["DateReturn_"];
                model.DateReturn = startDate.StringToDecimal();
                model.DateCreated = DateTime.Now.TotalSeconds();
                _da.AddRepay(model);
                _da.Save();
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được thêm mới.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateRepay(string key, string json)
        {
            var msg = new JsonMessage(false, "Cập nhật dữ liệu thành công !");
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                var model = _da.GetRepayById(ItemId);
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
                    msg.Message = "Không được phép chỉnh sửa.";
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Dữ liệu chưa được cập nhật.";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteRepay(string key, string lstArrId)
        {
            var msg = new JsonMessage(false, "Xóa dữ liệu thành công !");
            try
            {
                if (key == Keyapi)
                {
                    var lstInt = FDIUtils.StringToListInt(lstArrId);
                    var lst = _da.GetRepayListArrId(lstInt);
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

        //Tổng quát
        public ActionResult GetListGeneralCash(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<CashAdvanceItem>() : _da.GetListGeneralCash(Request, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
