using FDI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace FDI.MvcAPI.Common
{
    public class CustomerAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SkipAuthorization(filterContext)) return;

            var token = filterContext.RequestContext.HttpContext.Request.Headers["token"];
            if (token == null || !JWTService.Instance.IsTokenValid(token))
            {
                filterContext.Result = new JsonResult { Data = new JsonMessage(401, "Unauthorized"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        private static bool SkipAuthorization(AuthorizationContext filterContext)
        {
            return filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                   || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
        }
    }
}