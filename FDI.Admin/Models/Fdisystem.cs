using System.Web;

namespace FDI.Admin.Models
{
    public class Fdisystem
    {
        public static string LanguageId
        {
            get
            {
                var cookie = HttpContext.Current.Request.Cookies["LanguageId"];
                return cookie != null ? cookie.Value : "vi";
            }
        }
    }
}