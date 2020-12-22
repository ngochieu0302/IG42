using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FDI.Simple;
using FDI.Utils;

namespace FDI.Admin.Controllers
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
            if (codeCookie == null)
            {
                codeCookie = new HttpCookie(name) { Value = val.ToString(), Expires = DateTime.Now.AddHours(6) };
                Response.Cookies.Add(codeCookie);
            }
            else
            {
                codeCookie.Value = val.ToString();
                codeCookie.Expires = DateTime.Now.AddHours(6);
                Response.Cookies.Add(codeCookie);
            }
        }
    }
}