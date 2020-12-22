using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Web.Controllers.Wallet
{
    public class WalletCustomerHistoryController : BaseController
    {
        //
        // GET: /WalletCustomerHistory/
        WalletCustomerAPI _api = new WalletCustomerAPI();
        public ActionResult Index(string cusId)
        {
            ViewBag.Id = cusId;
            return View();
        }
        public ActionResult ListItems(string Id)
        {
            var query = Request.Url.Query;
            if (string.IsNullOrEmpty(query))
            {
                query = "?cusId=" + Id;
            }
            else
            {
                query += "&cusId=" + Id;
            }
            return View(_api.GetListById(UserItem.AgencyID, query));
        }
    }
}
