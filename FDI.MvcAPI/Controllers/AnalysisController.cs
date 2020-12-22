using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class AnalysisController : BaseApiController
    {
        //
        // GET: /Analysis/
        private readonly AnalysisDA _da = new AnalysisDA();
        public ActionResult GetAllAnalysis(string key, decimal start, decimal end)
        {
            if (Request["key"] != Keyapi) return Json(new AnalysisItem(), JsonRequestBehavior.AllowGet);
            var obj = _da.GetAllAnalysis(Agencyid(), start, end);
            if (obj != null) obj.ListItem = _da.ProductTop(Agencyid(), start, end);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AnalysisByEnterprise(string key, decimal start, decimal end)
        {
            var obj = key != Keyapi ? new List<AnalysisItem>() : _da.AnalysisByEnterprise(EnterprisesId(), start, end);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductTop(string key, decimal start, decimal end)
        {
            var obj = Request["key"] != Keyapi ? new List<ProductExportItem>() : _da.ProductTop(Agencyid(), start, end);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
