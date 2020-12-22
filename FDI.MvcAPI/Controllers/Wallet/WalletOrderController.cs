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

namespace FDI.MvcAPI.Controllers.Wallet
{
    public class WalletOrderController : BaseApiController
    {
        //
        // GET: /WalletOrder/

        private readonly WallerOrderDA _da = new WallerOrderDA();
        public ActionResult ListItems(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelWalletOrderHistoryItem()
                : new ModelWalletOrderHistoryItem { ListItem = _da.GetListSimpleByRequest(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListById(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelWalletOrderHistoryItem()
                : new ModelWalletOrderHistoryItem { ListItem = _da.GetListSimpleById(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerItem(string key, int id)
        {
            var obj = key != Keyapi ? new WalletOrder_History() : _da.GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json)
        {
            var model = new WalletOrder_History();
            var msg = new JsonMessage(false, "Thêm mới dữ liệu thành công.");
            try
            {
                UpdateModel(model);
                model.DateCreate = ConvertDate.TotalSeconds(DateTime.Now);
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
        public ActionResult Update(string key, string json)
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
