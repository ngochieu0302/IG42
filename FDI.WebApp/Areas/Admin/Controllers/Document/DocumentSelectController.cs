using System;
using System.Text;
using System.Web.Mvc;
using FDI.Areas.Admin.Controllers;
using FDI.DA;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Areas.Admin.Controllers
{
    public class DocumentSelectController : BaseController
    {
        public ActionResult Index()
        {
            var model = new ModelAlbumItem
            {
                Container = Request["Container"],                
                SelectMutil = Convert.ToBoolean(Request["MutilFile"])
            };
            return View(model);
        }

    }
}
