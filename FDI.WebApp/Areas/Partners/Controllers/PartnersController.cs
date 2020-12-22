using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Areas.Partners.Controllers
{
    public class PartnersController : Controller
    {
        private readonly PartnersBL _bl = new PartnersBL();

        public PartialViewResult Index(string slug, int pageId, int ctrId, string url, int cateId = 0, int page = 1)
        {
            var total = 0;
             var pages = string.Format("{0}-p{1}c{2}p", slug,pageId,cateId);
            var model = new ModelPartnerItem
            {
                ListItem =  _bl.GetList(page, ref total),
                PageHtml = Paging.GetPage(pages, 2, page, 18, total),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }

        public PartialViewResult Detail(int ctrId, string url, int cateId = 0)
        {
            var model = new ModelPartnerItem
            {
                PartnerItem = _bl.GetById(cateId),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }

        public PartialViewResult PartnerOther()
        {
            var model = _bl.GetPartnerListnews();
            return PartialView(model);
        }

        public ActionResult Product(int id)
        {
            var model = _bl.GetListPartner(id);
            return PartialView(model);
        }
    }
}
