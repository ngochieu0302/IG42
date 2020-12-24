using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.MvcAPI.Controllers.News
{
    public class NewsController : BaseAppApiController
    {
        // GET: News
        private readonly NewsDA _da = new NewsDA();

        public ActionResult ListItems(int page, int totalpage)
        {
            var data = _da.GetListAll(page, totalpage);
            return Json(new BaseResponse<List<NewsItem>> { Code = 200, Data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemById(int id)
        {
            var obj = _da.GetItemById(id);
            return Json(new BaseResponse<NewsItem> { Code = 200, Data = obj }, JsonRequestBehavior.AllowGet);
        }
    }
}