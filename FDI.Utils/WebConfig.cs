using System.Configuration;

namespace FDI.Utils
{
    public class WebConfig
    {
        public static string AdminUrl = ConfigurationManager.AppSettings["AdminUrl"].ToLower();
        public static string ListAdmin = ConfigurationManager.AppSettings["ListAdmin"];
        public static string LstLang = ConfigurationManager.AppSettings["ListLanguage"];
        public static string UrlUploadsImage = ConfigurationManager.AppSettings["UploadsImage"];
        public static string ImageLogo = ConfigurationManager.AppSettings["ImageLogo"];
        public static string UrlUploadsMailImage = ConfigurationManager.AppSettings["UploadsMailImage"];
        public static string UrlImage = ConfigurationManager.AppSettings["URLImage"];
        public static string Urltail = ConfigurationManager.AppSettings["URLtail"];
        public static string UrlJson = "http://" + Utility._d;//+ ConfigurationManager.AppSettings["URLJson"];
        public static string UrlNode = ConfigurationManager.AppSettings["UrlNode"];
        public static string CustomerId = ConfigurationManager.AppSettings["customerID"];
        public static string UserName = ConfigurationManager.AppSettings["UserName"];
        public static string TempImage = ConfigurationManager.AppSettings["TempImage"];
        public static string SessionTimeout = ConfigurationManager.AppSettings["sessionTimeout"];
        public static string ModuleArea = ConfigurationManager.AppSettings["ModuleArea"];
        public static string DocumentDetail = ConfigurationManager.AppSettings["DocumentDetails"]; 

        public static string GoupsMassageId = ConfigurationManager.AppSettings["GoupsMassageId"];
        public static string GoupsTableId = ConfigurationManager.AppSettings["GoupsTableId"];
        public static string MassageServiceId = ConfigurationManager.AppSettings["MassageServiceId"];
        public static string PDFDetail = ConfigurationManager.AppSettings["PDFDetail"];
        public static string NewsId = ConfigurationManager.AppSettings["NewsId"];
        public static string NewDetail = ConfigurationManager.AppSettings["NewsDetail"];
        public static string Partner = ConfigurationManager.AppSettings["Partner"];
        public static string PartnerDetail = ConfigurationManager.AppSettings["PartnerDetail"];
        public static string Product = ConfigurationManager.AppSettings["Product"];
        public static string ProductDetail = ConfigurationManager.AppSettings["ProductDetail"];
        public static string Service = ConfigurationManager.AppSettings["Service"];
        public static string ServiceDetail = ConfigurationManager.AppSettings["ServiceDetail"];
        public static string VideoId = ConfigurationManager.AppSettings["VideoId"];

        public static string ConnectString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
        public static string GetAppSettings(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
