using System.Web.Mvc;
using FDI.DA;

namespace FDI.Admin.Controllers
{
    public class CustomerTreeController : BaseController
    {
        private readonly CustomerDA _da = new CustomerDA();

        public ActionResult Index()
        {
            return View();
        }
        //public JsonResult JsonTreeCategorySelect()
        //{
        //    var ltsCategory = _da.GetListTree(1,5);
        //    return Json(ltsCategory, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult AjaxTreeSelect()
        {
            return View();
        }

    }
}
