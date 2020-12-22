using System.Collections.Generic;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.MvcAPI.Controllers
{
    public class VideoController : BaseApiController
    {
        readonly VideoDL _dl = new VideoDL();
        public JsonResult GetList(int page)
        {
            int total = 0;
            var obj = Request["key"] != Keyapi
                ? new ModelVideoItem()
                : new ModelVideoItem {ListItem = _dl.GetList(page, ref total), Total = total};
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetById(int id)
        {
            var obj = Request["key"] != Keyapi
                 ? new VideoItem() : _dl.GetById(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetList()
        {
            var obj = Request["key"] != Keyapi
                 ? new List<VideoItem>() : _dl.GetList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVideoOther(int id)
        {
            var obj = Request["key"] != Keyapi
                  ? new List<VideoItem>() : _dl.GetVideoOther(id);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVideoHot()
        {
            var obj = Request["key"] != Keyapi
                  ? new List<VideoItem>() : _dl.GetVideoHot();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}