using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class SeoController : BaseApiController
    {
        readonly SeoDL _dl = new SeoDL();
        public JsonResult GetSeoNews(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new SEOItem() : _dl.GetSeoNews(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetSeoProduct(int id)
        {
            var obj = Request["key"] != Keyapi
                 ? new SEOItem() : _dl.GetSeoProduct(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSeoPartner(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new SEOItem() : _dl.GetSeoPartner(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetSeoCategory(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new SEOItem() : _dl.GetSeoCategory(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}