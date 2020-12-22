using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA.DA;
using FDI.GetAPI;
using FDI.Simple;

namespace FDI.Web.Controllers.Document
{
    public class DocumentWarningController : BaseController
    {
        //
        // GET: /DocumentWarning/
        private readonly DNDrawerAPI _dnDrawerApi = new DNDrawerAPI();
        private readonly DNDocumentAPI _documentApi = new DNDocumentAPI();
        public ActionResult Index()
        {
            var model = new ModelDocumentItem
            {
                ListDrawerItems = _dnDrawerApi.GetListSimple(),
            };
            return View(model);
        }
        public ActionResult ListItems()
        {
            ViewBag.isadmin = IsAdmin;
            return View(_documentApi.ListItemsWarning(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var model = _documentApi.GetItemsByID(ArrId.FirstOrDefault());
            return View(model);
        }
    }
}
