using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Areas.Banner.Controllers
{
    public class BannerController : Controller
    {
        readonly GalleryBL _bl = new GalleryBL();
        private readonly ModuleSettingBL _sysbl = new ModuleSettingBL();
        readonly CategoryBL _categoryBl = new CategoryBL();
        readonly NewsBL _newsBl = new NewsBL();
        public PartialViewResult Slide(int ctrId, string url)
        {
            var model = new ModelAdvertisingItem { CtrId = ctrId };
            if (Request["doAction"] == "setting")
            {
                model.ListPositionItem = _bl.GetAdvertising();
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 14;
                model.ListItem = _bl.GetListAdvertising(val);
                return PartialView(model);
            }
        }

        public PartialViewResult Banner(int ctrId, string url, int cateId = 0)
        {
            var model = new ModelAdvertisingItem { CtrId = ctrId };
            if (Request["doAction"] == "setting")
            {
                model.ListPositionItem = _bl.GetAdvertising();
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 16;
                model.ListItem = _bl.GetListAdvertising(val);
                return PartialView(model);
            }
        }
        public PartialViewResult BannerDetails(int ctrId, string url, string slug, int cateId = 0)
        {
            var model = new ModelAdvertisingItem { CtrId = ctrId, CtrUrl = url };
            if (Request["doAction"] == "setting")
            {
                model.ListPositionItem = _bl.GetAdvertising();
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                model.cateId = cateId;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 15;
                model.AdvertisingItem = _bl.GetAdvertisingItem(val);
                return PartialView(model);
            }
        }
        public ActionResult CategoryChild(int cateId)
        {
            var model = new ModelCategoryItem();
            model.CategoryItem = _categoryBl.GetByid(cateId);
            return View(model);
        }
        public ActionResult CategoryChildDetails(int cateId)
        {
            var model = new ModelNewsItem();
            model.NewsItem = _newsBl.GetNewsId("", cateId);
            return View(model);
        }
        public PartialViewResult BannerMain(int ctrId, string url)
        {
            var model = new ModelAdvertisingItem { CtrId = ctrId };
            if (Request["doAction"] == "setting")
            {
                model.ListPositionItem = _bl.GetAdvertising();
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 10;
                model.AdvertisingItem = _bl.GetAdvertisingItem(val);
                return PartialView(model);
            }
        }
        public PartialViewResult Partners(int ctrId, string url)
        {
            var model = new ModelAdvertisingItem { CtrId = ctrId };
            if (Request["doAction"] == "setting")
            {
                model.ListPositionItem = _bl.GetAdvertising();
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : (int)SlideType.MainSlide;
                model.ListItem = _bl.GetListAdvertising(val);
                return PartialView(model);
            }
        }
        public PartialViewResult BannerCateHot(int ctrId, string url)
        {
            var model = new ModelAdvertisingItem { CtrId = ctrId };
            if (Request["doAction"] == "setting")
            {
                model.ListPositionItem = _bl.GetAdvertising();
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                return PartialView(model);
            }
            else
            {
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 17;
                model.AdvertisingItem = _bl.GetAdvertisingItem(val);
                return PartialView(model);
            }
        }
        public ActionResult BannerLeft()
        {
            var model = _bl.GetAdvertisingItem(18);
                return PartialView(model);
        }
    }
}
