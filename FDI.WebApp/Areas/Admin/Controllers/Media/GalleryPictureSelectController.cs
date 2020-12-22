using System;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.Areas.Admin.Controllers
{
    public class GalleryPictureSelectController : Controller
    {
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
