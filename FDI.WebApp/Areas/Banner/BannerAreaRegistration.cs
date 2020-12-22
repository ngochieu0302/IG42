using System.Web.Mvc;

namespace FDI.Web.Areas.Banner
{
    public class BannerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Banner";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Banner_default",
                "Banner/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
