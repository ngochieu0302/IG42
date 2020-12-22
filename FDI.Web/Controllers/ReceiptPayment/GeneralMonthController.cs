using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;
using System;

namespace FDI.Web.Controllers
{
    public class GeneralMonthController : BaseController
    {
        // GET: /Admin/Order/
        private readonly ReceiptPaymentAPI _api = new ReceiptPaymentAPI();
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var year = Request.QueryString["year"];
            var y = string.IsNullOrEmpty(year) ? DateTime.Now.Year : int.Parse(year);
            var model = _api.GeneralListTotal(y, UserItem.AgencyID);
            return View(model);
        }        
    }
}
