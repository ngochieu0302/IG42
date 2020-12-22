using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class TMNewsController : BaseApiController
    {
        readonly NewsDL _dl = new NewsDL();
        public JsonResult GetList(int cateId, int page)
        {
            int total = 0;
            var obj = Request["key"] != Keyapi
                ? new ModelNewsItem()
                : new ModelNewsItem {ListItem = _dl.GetList(cateId, page, ref total), Total = total};
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewKeyword(string keyword, int page)
        {
            var total = 0;
            var obj = Request["key"] != Keyapi
                ? new ModelNewsItem()
                : new ModelNewsItem { ListItem = _dl.GetNewKeyword(keyword, page, ref total), Total = total };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewByTag(int cateId, int page)
        {
            var total = 0;
            var obj = Request["key"] != Keyapi
                ? new ModelNewsItem()
                : new ModelNewsItem { ListItem = _dl.GetNewByTag(cateId, page, ref total), Total = total };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListByCateId(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new List<NewsItem>()
                :_dl.GetListByCateId(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListCateId(int id)
        {
            var obj = Request["key"] != Keyapi
                ? new List<CategoryItem>()
                : _dl.GetListCateId(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListOther(int id, int ortherId)
        {
            var obj = Request["key"] != Keyapi
                ? new List<NewsItem>()
                : _dl.GetListOther(id,ortherId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNewsId(int id)
        {
            var obj = Request["key"] != Keyapi
                 ? new NewsItem()
                 : _dl.GetNewsId(id,"");
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNewsItem(string name)
        {
            var obj = Request["key"] != Keyapi
                  ? new NewsItem()
                  : _dl.GetNewsItem(name);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }


    }
}