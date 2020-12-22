using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;
using FDI.Simple;

namespace FDI.Enterprises.Controllers
{
    public class SupplierController : BaseController

    {
        private readonly SupplierDA _supplierDa = new SupplierDA();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListItems()
        {
            var lst = _supplierDa.GetItems(EnterprisesItem.ID);
            var model = new ModelSupplierItem();
            model.ListItem = lst;
            return View(model);
        }
    }
}
