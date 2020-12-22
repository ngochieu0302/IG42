using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;

namespace FDI.MvcAPI.Controllers.APPSales
{
    public class CustomerAppSaleController : BaseApiAuthAppSaleController
    {
        private int rowPerPage = 10;
        CustomerDA _customerDa = new CustomerDA();

         
        [HttpPost]
        public ActionResult GetAll(int pageIndex)
        {
            var total = 0;
            var lst = _customerDa.GetAllByAgencyId(AgencyId, rowPerPage, pageIndex, ref total);
            return Json(lst);
        }


    }
}
