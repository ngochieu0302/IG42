using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.GetAPI;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Web.Controllers
{
    public class BaseNewController : Controller
    {     
        public ActionResult Index(string slug, int modulId = 0, int parent = 0)
        {
            AddCookies("ParentId", parent);
            AddCookies("ModuleId", modulId);
            return Redirect("/" + slug);
        }

        void AddCookies(string name, int val)
        {
            var codeCookie = HttpContext.Request.Cookies[name];
            codeCookie = new HttpCookie(name) { Value = val.ToString(), Expires = DateTime.Now.AddHours(6) };
            Response.Cookies.Add(codeCookie);
        }
    }
}