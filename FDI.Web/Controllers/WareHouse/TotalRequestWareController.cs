using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI.StorageWarehouse;

namespace FDI.Web.Controllers.WareHouse
{
    public class TotalRequestWareController : BaseController
    {
        //
        // GET: /TotalRequestWareRequest/
        private readonly StorageWarehouseAPI _api = new StorageWarehouseAPI();
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ListItems()
        {
            var lst= await _api.GetRequestWareSummary(Request.Url.Query);
            return PartialView(lst);
        }

    }
}
