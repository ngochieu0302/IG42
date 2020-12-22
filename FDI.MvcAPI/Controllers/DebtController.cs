using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class DebtController : BaseApiController
    {
        //
        // GET: /Debt/
        readonly DebtDA _da = new DebtDA();
        public ActionResult GetListByRequest(string key, int agencyId)
        {
            decimal totalprice;
            var obj = key != Keyapi
                ? new ModelDebtItem()
                : new ModelDebtItem { ListItem = _da.GetListByRequest(Request, agencyId, out totalprice), TotalPrice = totalprice, PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemByID(string key, int id)
        {
            var obj = key != Keyapi ? new DebtItem() : _da.GetItemByID(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, Guid userid)
        {
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            var model = new Debt();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                model.Note = HttpUtility.UrlDecode(model.Note);
                model.DateCreated = DateTime.Now.TotalSeconds();
                if (model.Type == 2)
                {
                    model.Price = model.Price * (-1);
                }
                model.IsDeleted = false;
                model.UserCreated = userid;
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
                    if (model.Type == 2)
                    {
                        model.Price = model.Price * (-1);
                    }
                    model.Note = HttpUtility.UrlDecode(model.Note);
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
                        item.IsDeleted = true;
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
