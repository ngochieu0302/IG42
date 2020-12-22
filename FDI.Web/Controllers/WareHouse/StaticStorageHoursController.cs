using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI.StorageWarehouse;

namespace FDI.Web.Controllers.WareHouse
{
    public class StaticStorageHoursController : BaseController
    {
        //
        // GET: /StaticStorageHours/
        readonly StorageWarehouseAPI _api = new StorageWarehouseAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.ListItemsStatic(Request.Url.Query, UserItem.AreaID));
        }
    }
}
