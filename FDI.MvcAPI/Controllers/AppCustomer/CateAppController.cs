using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.AppCustomer
{
    public class CateAppController : BaseApiController
    {
        //
        // GET: /CateItem/
        readonly CateAppDA _da = new CateAppDA("#");
        public ActionResult ListAll(string key)
        {
            var obj = key != Keyapi ? new List<CateAppItem>() : _da.GetListApp((int)ModuleType.Product);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetCategoryChild(int categoryId)
        {
            var categorys = _da.GetCategorysParentId(categoryId);
            return Json(categorys);
        }
    }
}
