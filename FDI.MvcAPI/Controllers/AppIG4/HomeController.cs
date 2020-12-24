using System.Web.Mvc;
using FDI.DA;

namespace FDI.MvcAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        readonly Shop_ProductDA _da = new Shop_ProductDA("#");
        public ActionResult GetAllList()
        {
            var obj =  _da.GetAllList();
            var jsonResult = Json(obj, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}