using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.CORE;
using FDI.DA;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.Wallet
{
    public class WalletCustomerController : BaseApiController
    {
        //
        // GET: /WalletCustomer/
        private readonly WalletCustomerDA _da = new WalletCustomerDA();
        public ActionResult ListItems(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelWalletCustomerItem()
                : new ModelWalletCustomerItem { ListItem = _da.GetListSimpleByRequest(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListById(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelWalletCustomerItem()
                : new ModelWalletCustomerItem { ListItem = _da.GetListSimpleById(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerItem(string key, int id)
        {
            var obj = key != Keyapi ? new WalletCustomer() : _da.GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add(string key, string json)
        {
            var model = new WalletCustomer();
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
        public ActionResult AddTranfer(string key, string moneyTranfer, string agencyId, string customerId)
        {
            // var model = new WalletCustomer();
            try
            {
                var wallet = new WalletCustomer
                {
                    AgencyId = int.Parse(agencyId),
                    CustomerID = int.Parse(customerId),
                    DateCreate = DateTime.Now.TotalSeconds(),
                    IsDelete = false,
                    TotalPrice = decimal.Parse(moneyTranfer)
                };
                _da.Add(wallet);
                _da.Save();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
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
