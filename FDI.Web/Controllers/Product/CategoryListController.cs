using System;
using System.Linq;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class CategoryListController : BaseController
    {
        readonly CategoryAPI _categoryApi;

        public CategoryListController()
        {
            _categoryApi = new CategoryAPI();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_categoryApi.ListItems(Request.Url.Query, (int)ModuleType.Product));
        }
       
    }
}
