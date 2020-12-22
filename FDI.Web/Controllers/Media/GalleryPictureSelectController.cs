using System;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Web.Controllers
{
    public class GalleryPictureSelectController : Controller
    {
        //
        // GET: /Admin/GalleryPictureSelect/
        /// <summary>
        /// Hiển thị select ảnh
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new ModelAlbumItem
            {
                Container = Request["Container"],
                ValuesSelected = Request["ValuesSelected"],
                SelectMutil = Convert.ToBoolean(Request["MutilFile"]),
                Type = Convert.ToInt32(Request["ModuleType"])
            };
            return View(model);
        }
    }
}
