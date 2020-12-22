using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers.Document
{
    public class StatisticDocumentController : BaseController
    {
        //
        // GET: /StatisticDocument/
        private readonly DNDocumentAPI _api = new DNDocumentAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            var year = Request.QueryString["year"];
            var y = string.IsNullOrEmpty(year) ? DateTime.Now.Year : int.Parse(year);
            var model = _api.GeneralListStatic(y, UserItem.AreaID);
            return View(model);
        }
    }
}
