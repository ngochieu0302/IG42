using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;
using System;
using FDI.CORE;

namespace FDI.MvcAPI.Controllers
{
    public class CustomerRewardController : BaseApiController
    {
        //
        // GET: /RewardHistory/
        readonly CustomerRewardDA _da = new CustomerRewardDA("#");
        public ActionResult ListItems()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCustomerRewardItem()
                : new ModelCustomerRewardItem { ListItems = _da.GetListSimpleByRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListRewardRequest(int agencyId)
        {
            var obj = Request["key"] != Keyapi
                ? new ModelDNUserItem()
                : new ModelDNUserItem { ListItem = _da.GetListRewardRequest(Request, agencyId), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListCustomerByRequest()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCustomerRewardItem()
                : new ModelCustomerRewardItem { ListItems = _da.GetListCustomerByRequest(Request), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListUserRequest()
        {
            var obj = Request["key"] != Keyapi
                ? new ModelCustomerRewardItem()
                : new ModelCustomerRewardItem { ListItems = _da.GetListUserRequest(Request, Agencyid()), PageHtml = _da.GridHtmlPage };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustomerRewardItem(int agencyId, int cusId, int month, int year,Guid UserId)
        {
            var obj = Request["key"] != Keyapi
                ? new CustomerRewardItem()
                : _da.GetCustomerRewardItem(agencyId, cusId, month, year, UserId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListByCustomer(int page, int id, int agencyId)
        {
            var obj = Request["key"] != Keyapi
               ? new List<CustomerRewardItem>()
               : _da.GetListByCustomer(page, id, agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Add(string key, string json)
        {
            var msg = new JsonMessage(false, "Trả thưởng thành công.");
            var model = new ReceiveHistory();
            try
            {
                if (key != Keyapi) return Json(0, JsonRequestBehavior.AllowGet);
                UpdateModel(model);
                var month = model.Month ?? int.Parse(DateTime.Now.Month.ToString());
                var year = model.Year ?? int.Parse(DateTime.Now.Year.ToString());
                var check = _da.CheckReciveHistory(month, year, model.CustomerID ?? 0, Agencyid(), model.Price ?? 0);
                if (check)
                {
                    model.DateCreate = DateTime.Now.TotalSeconds();
                    model.AgencyId = Agencyid();
                    model.IsDeleted = false;
                    model.Type = 1;
                    _da.Add(model);
                    _da.Save();
                }
                else
                {
                    msg.Erros = true;
                    msg.Message = "Trả thưởng thất bại, vui lòng kiểm tra lại.";
                }
            }
            catch (Exception ex)
            {
                msg.Erros = true;
                msg.Message = "Trả thưởng thất bại, vui lòng kiểm tra lại.";
                Log2File.LogExceptionToFile(ex);
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
    }
}
