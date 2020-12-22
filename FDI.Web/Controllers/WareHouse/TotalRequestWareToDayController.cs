using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.CORE;
using FDI.DA.DA.StorageWarehouse;
using FDI.GetAPI.StorageWarehouse;
using FDI.GetAPI.Supplier;
using FDI.Simple;
using FDI.Simple.StorageWarehouse;

namespace FDI.Web.Controllers.WareHouse
{
    public class TotalRequestWareToDayController : BaseController
    {
        //
        // GET: /TotalRequestWareRequest/
        private readonly StorageWarehouseAPI _api = new StorageWarehouseAPI();
        private readonly RequestWareAPI _apiRequestWareAPI = new RequestWareAPI();
        private readonly TotalProductToDayAPI _apiTotalProductToDayAPI = new TotalProductToDayAPI();
        private readonly SupplierAmountProductApi _apiSupplierAmountProductApi = new SupplierAmountProductApi();
   

        public ActionResult Index(decimal? todayCode)
        {
            if (todayCode == null)
            {
                return RedirectToAction("Index", new {todayCode = DateTime.Now.AddDays(1).Date.TotalSeconds()});
            }
            return View(todayCode.Value);
        }

        public async Task<ActionResult> ListItems(decimal todayCode)
        {
            var model = await _apiRequestWareAPI.ListItems(todayCode);
            return PartialView(model);
        }

    }
}
