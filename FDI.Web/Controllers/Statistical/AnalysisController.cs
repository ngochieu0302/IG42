using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.GetAPI;
using FDI.Utils;

namespace FDI.Web.Controllers.Statistical
{
    public class AnalysisController : BaseController
    {
        //
        // GET: /Analysis/
        private readonly AnalysisAPI _api = new AnalysisAPI();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.GetAllAnalysis(UserItem.AgencyID, ConvertDate.TotalSeconds(DateTime.Today), ConvertDate.TotalSeconds(DateTime.Today.AddDays(1))));
        }
        public ActionResult ProductTop()
        {
            return Json(_api.ProductTop(UserItem.AgencyID, ConvertDate.TotalSeconds(DateTime.Today), ConvertDate.TotalSeconds(DateTime.Today.AddDays(1))), JsonRequestBehavior.AllowGet);
        }
    }
}
