using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers.Sales
{
    public class SaleSimController : BaseController
    {
        //
        // GET: /SaleSim/
        private readonly OrderAPI _ordersApi = new OrderAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        readonly ProductAPI _api = new ProductAPI();
        private readonly CustomerAPI _customerApi = new CustomerAPI();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View();
        }

        public ActionResult Report()
        {
            var model = _agencyApi.GetItemById(UserItem.AgencyID);
            return View(model);
        }

        public ActionResult AjaxForm()
        {
            return View();
        }
        public ActionResult AjaxNoteCate()
        {
            var url = Request.Form.ToString();
            var msg = _customerApi.UpdateCustomerCare(url, UserItem.AgencyID);
            return Json(msg);
        }
        public ActionResult Auto()
        {
            var query = Request["query"];
            var type = Request["type"] ?? "0";
            var ltsResults = _api.GetListAutoSim(query, 10, UserItem.AgencyID, int.Parse(type));
            var resulValues = new AutoCompleteProduct
            {
                query = query,
                suggestions = ltsResults
            };
            return Json(resulValues, JsonRequestBehavior.AllowGet);
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

        public ActionResult Actions()
        {
            var msg = new JsonMessage();
            var url = Request.Form.ToString();
            switch (DoAction)
            {
                case ActionType.Add:
                    msg = _ordersApi.AddSales(UserItem.AgencyID, url, UserItem.UserId, CodeLogin(), "");
                    break;
                default:
                    msg.Message = "Bạn không được phần quyển cho chức năng này.";
                    msg.Erros = true;
                    break;
            }
            return Json(msg);
        }
    }
}
