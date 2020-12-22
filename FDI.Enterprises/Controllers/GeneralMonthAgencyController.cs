using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Enterprises.Controllers
{
    public class GeneralMonthAgencyController : BaseController
    {
        //
        // GET: /GeneralMonthAgency/

        private readonly ReceiptPaymentAPI _api = new ReceiptPaymentAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        public ActionResult Index()
        {
            return View(_agencyApi.GetAll(EnterprisesItem.ID));
        }
        public ActionResult ListItems(int id, int year)
        {
            var model = _api.GeneralListTotal(year, id);
            return View(model);
        }

    }
}
