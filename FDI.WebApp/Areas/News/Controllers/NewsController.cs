using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Areas.News.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsBL _bl = new NewsBL();
        private readonly ProductBL _productBl = new ProductBL();
        private readonly MenuBL _menuBl = new MenuBL();
        private readonly ModuleSettingBL _sysbl = new ModuleSettingBL();
        private readonly GalleryBL _gallery = new GalleryBL();
        private readonly NewsDA _newsDa = new NewsDA("#");
        readonly CategoryBL _categoryBl = new CategoryBL();
        public PartialViewResult Index(string slug, string url, string tag = "", int pageId = 0, int ctrId = 0, int cateId = 0, int page = 1)
        {
            var total = 0;
            const int rowPage = 5;
            var keyword = "";
            var text = "";
            var search = slug.Contains("search");
            var isTag = tag.Contains("tag");
            if (search)
            {
                text = Request["keyword"];
                keyword = FomatString.Slug(text);
            }
            var pages = string.Format("/{0}-p{1}c{2}p", slug, pageId, cateId);
            var model = new ModelNewsItem
            {
                ListItem = isTag ? _bl.GetNewByTag(slug, cateId, page, rowPage, ref total)
                    : search ? _bl.GetNewKeyword(keyword, page, rowPage, ref total) : _bl.GetList(cateId, page, rowPage, ref total),
                PageHtml = isTag ? Paging.GetPage("/tag/" + slug + "/", 2, page, rowPage, total) :
                            !search ? Paging.GetPage(pages, 2, page, rowPage, total) :
                            Paging.GetPage(pages, 2, page, rowPage, total, "keyword", text),
                CategoryItem = _productBl.CategoryItem(cateId),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }

        public PartialViewResult NewsHot(int ctrId, string url)
        {
            var model = new ModelCategoryItem { CtrId = ctrId, CtrUrl = url };
            if (Request["doAction"] == "setting")
            {
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                model.ListItem = _menuBl.GetChildCategories((int)ModuleType.News);
                return PartialView(model);
            }
            model.ListItem = _categoryBl.GetlistCateShowhome((int)ModuleType.News);
            return PartialView(model);
        }

        public PartialViewResult CategoryHot(int ctrId, string url)
        {
            var model = new ModelCategoryItem() { CtrId = ctrId, CtrUrl = url };
            if (Request["doAction"] == "setting")
            {
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                model.ListItem = _categoryBl.GetlistCate();
                return PartialView(model);
            }
            else
            {
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 5;
                model.CategoryItem = _categoryBl.GetByid(val);
                return PartialView(model);
            }
        }
        public PartialViewResult CategoryHotGroup(int ctrId, string url)
        {
            var model = new ModelCategoryItem() { CtrId = ctrId, CtrUrl = url };
            if (Request["doAction"] == "setting")
            {
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                model.ListItem = _categoryBl.GetlistCate();
                return PartialView(model);
            }
            else
            {
                model.CtrUrl = url;
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 17;
                model.CategoryItem = _categoryBl.GetByid(val);
                return PartialView(model);
            }
        }

        public PartialViewResult NewsHome(int ctrId, string url)
        {
            var model = new ModelNewsItem { CtrId = ctrId, CtrUrl = url };
            model.ListItem = _bl.GetListHot();
            return PartialView(model);

        }

        public ViewResult ListNews(int homeId = 0, int cateId = 0)
        {
            var model = _bl.GetList(homeId, cateId);
            return View(model);
        }

        public PartialViewResult LisNewOther(string slug, int ctrId, string url)
        {
            var model = new ModelNewsItem { CtrId = ctrId, CtrUrl = url };
            if (Request["doAction"] == "setting")
            {
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                model.LstCategoryItems = _menuBl.GetChildCategories((int)ModuleType.News);
                return PartialView(model);
            }
            else
            {
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 3;
                model.NewUrl = _bl.GetName(val);
                model.ListItem = _bl.GetListByCateId(val);
                return PartialView(model);
            }
        }

        public PartialViewResult Detail(int ctrId, string url, string slug, int cateId = 0)
        {
            var model = new ModelNewsItem
            {
                NewsItem = _bl.GetNewsId(slug, cateId),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }

        public ActionResult NewsOther(int cateId, int ortherId)
        {
            var model = _bl.GetListOther(cateId, ortherId);
            return PartialView(model);
        }

        public PartialViewResult NewsHomeLeft(int ctrId, string url)
        {
            var model = new ModelNewsItem { CtrId = ctrId, CtrUrl = url };
            if (Request["doAction"] == "setting")
            {
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                model.LstCategoryItems = _menuBl.GetChildCategories((int)ModuleType.News);
                return PartialView(model);
            }
            else
            {
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 3;
                model.NewUrl = _bl.GetName(val);
                model.ListItem = _bl.GetListByCateId(val);
                return PartialView(model);
            }
        }

        public PartialViewResult NewsLeft(int ctrId, string url)
        {
            var model = new ModelCategoryItem { CtrId = ctrId, CtrUrl = url };
            if (Request["doAction"] == "setting")
            {
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 5;
                model.ListItem = _menuBl.GetChildCategories((int)ModuleType.News);
                return PartialView(model);
            }
            else
            {
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 79;
                model.ListItem = _categoryBl.GetlistCatebyParent(val);
                return PartialView(model);
            }
        }
        public PartialViewResult NewsLeftDetail(int ctrId, string url)
        {
            var model = new ModelNewsItem { CtrId = ctrId, CtrUrl = url };
            if (Request["doAction"] == "setting")
            {
                var key = _sysbl.GetKey(ctrId);
                model.PageId = key != null ? int.Parse(key.Value) : 0;
                model.LstCategoryItems = _menuBl.GetChildCategories((int)ModuleType.News);
                return PartialView(model);
            }
            else
            {
                var key = _sysbl.GetKey(ctrId);
                var val = key != null ? int.Parse(key.Value) : 9;
                model.NewUrl = _bl.GetName(val);
                model.CategoryItem = _categoryBl.GetByid(val);
                return PartialView(model);
            }
        }
        public PartialViewResult BannerDetails()
        {
            var model = _gallery.GetAdvertisingItem((int)Position.Detailnews);
            return PartialView(model);
        }
        public PartialViewResult ProjectOther(int ctrId, string url)
        {
            var model = new ModelNewsItem { CtrId = ctrId, CtrUrl = url };
            var item = _sysbl.GetKey(ctrId);
            var id = item != null ? int.Parse(item.Value) : 1033;
            if (Request["doAction"] == "setting")
            {
                model.PageId = id;
                model.LstCategoryItems = _menuBl.GetChildCategories((int)ModuleType.News);
                return PartialView(model);
            }
            model.ListItem = _bl.GetListByCateId(id);
            return PartialView(model);
        }

        public ActionResult NewsRecent()
        {
            var model = _bl.GetList();
            return PartialView(model);
        }
        public PartialViewResult ListPDF(string slug, string url, string tag = "", int pageId = 0, int ctrId = 0, int cateId = 0, int page = 1)
        {
            var total = 0;
            const int rowPage = 5;
            var text = "";
            
            var pages = string.Format("/{0}-p{1}c{2}p", slug, pageId, cateId);
            var model = new ModelNewsItem
            {
                ListItem = _bl.GetListPDF(15, page, rowPage, ref total),
                PageHtml = Paging.GetPage(pages, 2, page, rowPage, total, "keyword", text),
                CategoryItem = _productBl.CategoryItem(cateId),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }
        public PartialViewResult ViewPDF(int ctrId, string url, string slug, int cateId = 0)
        {
            var model = new ModelNewsItem
            {
                NewsItem = _bl.GetNewsId(slug, cateId),
                CtrId = ctrId,
                CtrUrl = url
            };
            return PartialView(model);
        }
    }
}
