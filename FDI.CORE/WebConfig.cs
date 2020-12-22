
using System.Configuration;
namespace FDI.CORE
{
    public class WebConfigCore
    {
        public static string UrlUploadsImage = ConfigurationManager.AppSettings["UploadsImage"];
        public static string ImageLogo = ConfigurationManager.AppSettings["ImageLogo"];
        public static string UrlUploadsMailImage = ConfigurationManager.AppSettings["UploadsMailImage"];
        public static string UrlImage = ConfigurationManager.AppSettings["URLImage"];
        public static string Urltail = ConfigurationManager.AppSettings["URLtail"];
        public static string TempImage = ConfigurationManager.AppSettings["TempImage"];

    }
}
