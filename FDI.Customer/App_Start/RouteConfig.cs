using System.Web.Mvc;
using System.Web.Routing;

namespace FDI.Customer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("cay-he-thong", "cay-he-thong", new { controller = "Customer", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("don-hang", "don-hang", new { controller = "Order", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("thong-tin-tai-khoan", "thong-tin-tai-khoan", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("dang-ky", "dang-ky", new { controller = "Account", action = "Register", id = UrlParameter.Optional });
            routes.MapRoute("danh-sach-the", "danh-sach-the", new { controller = "Card", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("hop-thu", "hop-thu", new { controller = "Email", action = "Index" , id = UrlParameter.Optional} );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}