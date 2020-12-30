//using FDI.DA.DA.AppIG4;
//using FDI.Simple;
//using FDI.Simple.AppIG4;
//using FDI.Utils;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Mvc;

//namespace FDI.MvcAPI.Controllers.AppIG4.Customer
//{
//    public class CustomerAppSMSController : BaseAppApiController
//    {
//        private readonly CustomerAppSMSDA _da = new CustomerAppSMSDA();
        
//        [HttpPost]
//        public ActionResult AddSMS(SMSAppIG4Item data)
//        {

//            _da.Add();
//            return Json(new { Code = 200, Erros = false }, JsonRequestBehavior.AllowGet);
//        }
//    }
//}
