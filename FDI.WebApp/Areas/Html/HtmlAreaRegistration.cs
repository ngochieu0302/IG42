using System.Web.Mvc;

namespace FDI.Web.Areas.Html
{
    public class HtmlAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Html";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Html_default",
                "Html/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
