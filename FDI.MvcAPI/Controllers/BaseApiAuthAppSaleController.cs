using System;
using System.Web.Mvc;
using FDI.MvcAPI.Common;

namespace FDI.MvcAPI.Controllers
{
    public class BaseApiAuthAppSaleController : BaseApiController
    {
        //public int AgencyId { get; set; }
        public Guid? Userid { get; set; }
        protected bool IsAdmin { get; set; }
        public int IdItem { get; set; }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNotNullResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Request.Headers["x-access-key"] == null)
            {
                throw new ArgumentException("Missing x-access-key.");
            }

            if (Request.Headers["x-access-userId"] != null)
            {
                if (Guid.TryParse(Request.Headers["x-access-userId"], out var tmpUserid))
                {
                    Userid = tmpUserid;
                }
            }
           
            //if (Request.Headers["x-access-agencyId"] == null)
            //{
            //    throw new ArgumentException("Missing x-access-agencyId.");
            //}

            //if (!int.TryParse(Request.Headers["x-access-agencyId"], out var agencyId))
            //{
            //    throw new ArgumentException("Invalid x-access-key.");
            //}

            //AgencyId = agencyId;

        }
    }
}