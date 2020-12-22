using System;
using System.Web.Mvc;

namespace FDI.MvcAPI.Controllers
{
    public class BaseApiAuthController : BaseApiController
    {
        public Guid? Userid { get; set; }
        protected bool IsAdmin { get; set; }
        public int IdItem { get; set; }
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
            if (Request.Headers["x-access-isadmin"] != null)
            {
                if (bool.TryParse(Request.Headers["x-access-isadmin"], out var isadmin))
                {
                    IsAdmin = isadmin;
                }
            }
            if (Request.Headers["x-access-ItemID"] != null)
            {
                if (int.TryParse(Request.Headers["x-access-ItemID"], out var ID))
                {
                    IdItem = ID;
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