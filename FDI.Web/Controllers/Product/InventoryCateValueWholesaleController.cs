using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;

namespace FDI.Web.Controllers.Product
{
    public class InventoryCateValueWholesaleController : BaseController
    {
        //
        // GET: /InventoryWholesale/
        readonly StorageProductAPI _api = new StorageProductAPI();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListItems()
        {
            return View(_api.GetListCateValueByRequest(UserItem.AgencyID, Request.Url.Query));
        }
        public ActionResult AjaxView()
        {
            var model = _api.GetCateValueItem(ArrId.FirstOrDefault());
            return View(model);
        }
    }
}
