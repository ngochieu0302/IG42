using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class GalleryController : BaseApiController
    {
        readonly GalleryDL _dl = new GalleryDL();
        public JsonResult GetListAdvertising(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new List<AdvertisingItem>() : _dl.GetListAdvertising(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAdvertising()
        {
            var obj = Request["key"] != Keyapi
                ? new List<AdvertisingPositionItem>() : _dl.GetAdvertising();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}