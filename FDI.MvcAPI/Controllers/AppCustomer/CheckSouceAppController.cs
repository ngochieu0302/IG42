using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers
{
    public class CheckSouceAppController : BaseApiController
    {
        //
        // GET: /CheckSouce/
        readonly DNImportDA _dnImportDa = new DNImportDA("#");
        public ActionResult CustomerCheckSouce(string key, string barcode, int cusId)
        {
            var model = new BarcodeSouceItem { Status = 0 };
            if (key == Keyapi)
            {
                try
                {
                    if (!string.IsNullOrEmpty(barcode)) return Json(_dnImportDa.GetByBarcode(barcode, cusId), JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    model = new BarcodeSouceItem { Status = 0 };
                }
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetListAll(string key, int id, int type)
        {
            var obj = key != Keyapi ? new List<SourceItem>() : _dnImportDa.GetListSourcesByStorageID(id, type);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSourceByID(string key, int id)
        {
            var obj = key != Keyapi ? new SourceItem() : _dnImportDa.GetSourceByID(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
