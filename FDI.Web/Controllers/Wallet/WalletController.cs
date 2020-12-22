using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using FDI.Web.Models;

namespace FDI.Web.Controllers
{
    public class WalletController : BaseController
    {
        //
        // GET: /Wallet/
        WalletAPI _api = new WalletAPI();
        readonly CustomerRewardAPI _customerRewardApi = new CustomerRewardAPI();
        readonly WalletCustomerAPI _walletCustomerApi = new WalletCustomerAPI();
        readonly ReceiveHistoryAPI _receiveHistoryApi = new ReceiveHistoryAPI();
        private readonly CustomerAPI _customerApi = new CustomerAPI();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.GetListCustomerByRequest(UserItem.AgencyID, Request.Url.Query));
        }

        public ActionResult TranferMoney()
        {
            return PartialView();
        }
        public ActionResult AutoCustomer()
        {
            var query = Request["query"];
            query = query.Replace("%", "");
            query = query.Replace("?", "");
            var ltsResults = _customerApi.GetListAuto(query, 10, UserItem.AgencyID);
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults,
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ActionTranferMoney(TranferMoneyModel model)
        {
            var msg = new JsonMessage { Erros = false };
            var listCus = _customerRewardApi.GetListByCustomer(1, model.CustomerID ?? 0, UserItem.AgencyID);
            var totalrecive = listCus != null ? listCus.Sum(c => c.TotalReceipt) : 0;
            var totalrewar = listCus != null ? listCus.Sum(c => c.TotalReward) : 0;
            var json = Request.Form.ToString();
            var total = totalrewar - totalrecive;
            var tranfer = model.Moneytranfer;
            if (tranfer > total)
            {
                msg = new JsonMessage()
                {
                    Erros = true,
                    Message = "Giao dịch không thành công. Số tiền chuyển lớn hơn số tiền hiện có.!"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            try
            {
                msg.ID = _walletCustomerApi.AddTranfer(model.Moneytranfer ?? 0, UserItem.AgencyID, model.CustomerID ?? 0).ToString();
                msg.ID = _receiveHistoryApi.AddTranfer(model.Moneytranfer ?? 0, UserItem.AgencyID, model.CustomerID ?? 0).ToString();
                if (msg.ID == "1")
                {
                    msg = new JsonMessage()
                    {
                        Erros = false,
                        Message = "Giao dịch thành công.!"
                    };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    msg = new JsonMessage()
                    {
                        Erros = true,
                        Message = "Giao dịch không thành công.!"
                    };
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                msg = new JsonMessage()
                {
                    Erros = true,
                    Message = "Giao dịch Không thành công.!"
                };
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
