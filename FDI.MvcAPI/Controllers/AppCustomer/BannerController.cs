using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.AppCustomer
{
    public class BannerController : BaseApiController
    {
        //
        // GET: /CateItem/
        readonly BannerDA _da = new BannerDA("#");
        public ActionResult GetAll(string key)
        {
            var data = _da.GetAll();

            return Json(new BaseResponse<List<BannerItem>>()
            {
                Erros = false,
                Data = data
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
