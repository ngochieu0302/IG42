using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.DA;

namespace FDI.MvcAPI.Controllers.APPSales
{
    public class PolicyAgencyController : BaseApiAuthAppSaleController

    {
        PolicyAgencyDA _policyAgencyDa = new PolicyAgencyDA();
        [HttpPost]
        public ActionResult Get(int categoryId)
        {
            var lst = _policyAgencyDa.GetAll(categoryId);

            return Json(lst);
        }

    }
}
