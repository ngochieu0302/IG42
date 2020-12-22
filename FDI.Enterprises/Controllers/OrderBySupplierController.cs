using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.DA.DA.StorageWarehouse;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Enterprises.Controllers
{
    public class OrderBySupplierController : BaseController
    {
        //
        // GET: /OrderByEnterprises/

        private readonly OrderAPI _da = new OrderAPI();
        private readonly DNAgencyAPI _agencyApi = new DNAgencyAPI();
        private readonly SupplierDA _supplierDa = new SupplierDA();
        private readonly RequestWareDA _requestWareDa = new RequestWareDA();
        readonly StorageWareHouseDA _storageWareHouseDa = new StorageWareHouseDA();
        public ActionResult Index()
        {
            var lst = _supplierDa.GetItems(EnterprisesItem.ID);
            return View(lst);
        }
        public ActionResult ListItems(int id, int year)
        {
            var lst = _requestWareDa.GetItemsBySupplierId(new int[] { 5 });
            var response = new ModelDNRequestWareItem();
            response.ListItems = lst;
            return View(response);

        }
        public async Task<ActionResult> ChangeStatus(int orderId, int status)
        {
            var lst = _supplierDa.GetItems(EnterprisesItem.ID);
            var order = _storageWareHouseDa.GetById(orderId);
            if (!order.DN_RequestWare.Any(m => m.DN_RequestWareSupplier.Any(n => lst.Any(c => n.SupplierId == c.ID))))
            {
                return Json(new JsonMessage(true, "Not authen"));
            }

            order.Status = status;
            _storageWareHouseDa.Save();

            return Json(new JsonMessage(false,"Cập nhật thành công"), JsonRequestBehavior.AllowGet);
        }

    }
}
