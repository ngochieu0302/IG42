using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Base;
using FDI.GetAPI;

namespace FDI.Web.Controllers.Statistical
{
    public class StaticAreaProductController : BaseController
    {
        //
        // GET: /StaticAreaProduct/
        readonly AreaAPI _api = new AreaAPI();
        readonly CityAPI _cityApi = new CityAPI();
        private readonly CategoryAPI _categoryApi = new CategoryAPI();
        
        public ActionResult Index()
        {
            ViewBag.listCity = _cityApi.GetAll();
            ViewBag.lstcate = _categoryApi.GetAll();
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItemsStatic(Request.Url.Query));
        }

    }
}
