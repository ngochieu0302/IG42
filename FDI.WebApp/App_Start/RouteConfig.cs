using System.Web.Mvc;
using System.Web.Routing;

namespace FDI
{
    public class RouteConfig
    {
        private const string Fm = @"(\d+)";

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("slug-tag", "{tag}/{slug}", new { controller = "Home", action = "Index" }, new { tag = "tag" });
            routes.MapRoute("slug-tag-page", "{tag}/{slug}/{page}", new { controller = "Home", action = "Index" }, new { tag = "tag", page = Fm });
            routes.MapRoute("cate-lang", "{lang}/{slug}-p{pageId}c{cateId}", new { controller = "Home", action = "Index" }, new { pageId = Fm, cateId = Fm });
            routes.MapRoute("cate", "{slug}-p{pageId}c{cateId}", new { controller = "Home", action = "Index" }, new { pageId = Fm, cateId = Fm });
            routes.MapRoute("cate-page-lang", "{lang}/{slug}-p{pageId}c{cateId}p{page}", new { controller = "Home", action = "Index" }, new { pageId = Fm, cateId = Fm, page = Fm });
            routes.MapRoute("cate-page", "{slug}-p{pageId}c{cateId}p{page}", new { controller = "Home", action = "Index" }, new { pageId = Fm, cateId = Fm, page = Fm });
            routes.MapRoute("detail-lang", "{lang}/{slug}-p{pageId}c{cateId}", new { controller = "Home", action = "Index" }, new { pageId = Fm, cateId = Fm });
            routes.MapRoute("detail", "{slug}-p{pageId}c{cateId}", new { controller = "Home", action = "Index" }, new { pageId = Fm, cateId = Fm });
            routes.MapRoute("page-lang", "{lang}/{slug}-p{pageId}", new { controller = "Home", action = "Index" }, new { pageId = Fm });
            routes.MapRoute("slug-page", "{slug}-p{pageId}", new { controller = "Home", action = "Index" }, new { pageId = Fm });
            routes.MapRoute("slug-lang", "{lang}/{slug}", new { controller = "Home", action = "Index" });
            routes.MapRoute("slug", "{slug}", new { controller = "Home", action = "Index" });
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}