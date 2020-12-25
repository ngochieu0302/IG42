using FDI.DA;
using FDI.MvcAPI.Common;
using FDI.Simple;
using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FDI.MvcAPI.Controllers
{
    [CustomerAuthorize]
    public class AdvertisingIGController : BaseAppApiController
    {
        AdvertisingAppIG4DA advertisingDA = new AdvertisingAppIG4DA();

        public ActionResult GetByPosition(int id)
        {
            var lst = advertisingDA.GetByPosition(id);
            return Json(new BaseResponse<List<AdvertisingItem>> { Code = 200, Erros = false, Data = lst }, JsonRequestBehavior.AllowGet);
        }

    }
}
