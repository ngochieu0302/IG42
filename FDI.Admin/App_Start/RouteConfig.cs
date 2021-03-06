﻿using System.Web.Mvc;
using System.Web.Routing;

namespace FDI
{
    public class RouteConfig
    {
        private const string Fm = @"(\d+)";
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Dieu hương", "{slug}/p{modulId}c{parent}", new { controller = "BaseNew", action = "Index" }, new { modulId = Fm, parent = Fm });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Admin", action = "Index", id = UrlParameter.Optional });
        }
    }
}