using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA.DA.AppSales;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers.APPSales
{
    public class DocumentsAppController : BaseApiController
    {
        //
        // GET: /DocumentsApp/
        readonly DocumentsAppDA _da = new DocumentsAppDA("#");
        public ActionResult GetListDocByAgencyApp(string key, int agencyId)
        {
            var obj = key != Keyapi ? new List<DocumentItem>() : _da.GetListDocByAgencyApp(agencyId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDocFileByIDApp(string key, int id)
        {
            var obj = key != Keyapi ? new DocumentItem() : _da.GetDocFileByIDApp(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}
