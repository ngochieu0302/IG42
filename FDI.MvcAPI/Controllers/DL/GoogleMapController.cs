using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class GoogleMapController : BaseApiController
    {
        readonly GoogleMapDl _dl = new GoogleMapDl();
        public JsonResult GetGoogleMapsItemByDistrictID(int districtID)
        {
            var obj = Request["key"] != Keyapi
                ? new List<GoogleMapItem>() : _dl.GetGoogleMapsItemByDistrictID(districtID);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGoogleMapsItemByCityId(int cityId)
        {
            var obj = Request["key"] != Keyapi
                ? new List<GoogleMapItem>() : _dl.GetGoogleMapsItemByCityId(cityId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllListSimple()
        {
            var obj = Request["key"] != Keyapi
                ? new List<GoogleMapItem>() : _dl.GetAllListSimple();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}