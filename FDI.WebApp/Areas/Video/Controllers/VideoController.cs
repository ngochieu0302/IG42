using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Areas.Video.Controllers
{
    public class VideoController : Controller
    {
        readonly VideoBL _bl = new VideoBL();
        private readonly ContactBL _contactBl = new ContactBL();
        public PartialViewResult Index(string slug, int pageId, int ctrId, string url, int cateId = 0, int page = 1)
        {
            var total = 0;
            var pages = string.Format("/{0}-p{1}c{2}p", slug, pageId, cateId);
            var model = new ModelVideoItem
            {
                ListItem = _bl.GetList(cateId, page, ref total),
                PageHtml = Paging.GetPage(pages, 2, page, 18, total),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }

        public PartialViewResult VideoHome(int ctrId, string url)
        {
            var model = new ModelVideoItem
            {
                ListItem = _bl.GetList(),
                CtrId = ctrId,
                CtrUrl = url
            };            
            return PartialView(model);
        }

        public PartialViewResult Detail(int ctrId, string url, int cateId = 0)
        {
            var model = _bl.GetById(cateId);
            return PartialView(model);
        }
        public PartialViewResult VideoOther(int videlId)
        {
            var model = _bl.GetVideoOther(videlId);
            return PartialView(model);
        }
        //public PartialViewResult VideoHot(int ctrId, string url)
        //{
        //    var model = new ModelVideoItem
        //    {
        //        VideoItem = _bl.GetVideoHot()  ,
        //        CtrId = ctrId,
        //        CtrUrl = url
        //    };
        //    return PartialView(model);
        //}
    }
}
