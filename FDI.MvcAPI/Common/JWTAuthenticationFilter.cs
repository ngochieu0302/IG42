using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using AuthenticationService.Managers;
using AuthenticationService.Models;
using FDI.Utils;
using IAuthorizationFilter = System.Web.Http.Filters.IAuthorizationFilter;

namespace FDI.MvcAPI.Common
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            var token = request.Headers["token"];
            IAuthService authService = new JWTService();
            
            var unauthorizedResult = new JsonResult { Data = new JsonMessage(true, "Unauthorized"), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            if (token == null || !authService.IsTokenValid(token))
            {
                //filterContext.Result =  unauthorizedResult;
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Unauthorized, "Unauthorized");
            }
        }
    }
}