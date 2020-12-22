using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.DA.DA;
using FDI.DA.DA.AppCustomer;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers.AppCustomer
{
    public class NewsCusAppController : BaseApiAuthController
    {
        //
        // GET: /NewsCusApp/
        private readonly NewsCusAppDA _da = new NewsCusAppDA("#");
        [HttpPost]
        public ActionResult GetListNewsApp(int pageIndex, int pageSize)
        {
            int total = 0;
            var obj = _da.GetListNews(pageSize, pageIndex, ref total);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetListNewsHotApp()
        {
            var obj = _da.GetListNewsHot();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult GetNewsDetailApp(int id)
        {
            var obj = _da.GetNewsItem(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetListNewsbyCateIdApp(int id)
        {
            var obj = _da.GetListNewsbyCateIdApp(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}
