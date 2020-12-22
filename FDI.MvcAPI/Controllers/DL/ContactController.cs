using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class ContactController : BaseApiController
    {
        readonly ContactDL _dl = new ContactDL();

        public JsonResult SysConfig()
        {
            var obj = Request["key"] != Keyapi
                ? new SystemConfigItem() : _dl.SysConfig();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCity()
        {
            var obj = Request["key"] != Keyapi
                ? new List<CityItem>() : _dl.GetCity();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGoogleMap()
        {
            var obj = Request["key"] != Keyapi
                ? new List<GoogleMapItem>() : _dl.GetGoogleMap();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckEmail(string txt)
        {
            var obj = Request["key"] == Keyapi && _dl.CheckEmail(txt);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}