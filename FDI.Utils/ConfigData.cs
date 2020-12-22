using System.Drawing;
using System.Web;

namespace FDI.Utils
{
    public static class ConfigData
    {
        public static int UserguidePerpage = 30;
        public static int UserguideOther = 10;
        public static int UserguidePageStep;

        public static Size ImageFullHdFile = new Size(1920, 1920);
        public static Size ImageHdFile = new Size(1280, 640);
        public static Size ImageMediumFile = new Size(640, 640);
        public static Size ImageThumbsSize = new Size(257, 257);

        public static string Uploads = "/Uploads/";
        public static string Temp = "/Uploads/Temp/";
        public static string Root = "/Uploads/Root/";
        public static string Thumbs = "/Uploads/Thumbs/";
        public static string Originals = "/Uploads/Originals/";
        public static string Mediums = "/Uploads/Mediums/";
        public static string Images = "/Uploads/Images/";
        public static string Document = "/Uploads/Document/";
        public static string Mail = "/Uploads/Mail/";
        public static string Files = "/Uploads/files/";
        public static string Video = "/Uploads/Video/";

        public static string UploadFolder = HttpContext.Current.Server.MapPath(Uploads);
        public static string TempFolder = HttpContext.Current.Server.MapPath(Temp);
        public static string FolderVideo = HttpContext.Current.Server.MapPath(Video);
        public static string FilesFolder = HttpContext.Current.Server.MapPath(Files);
        public static string TempFolderWeb = HttpContext.Current.Server.MapPath(Temp).Replace("FDI.MvcAPI", "FDI.Web");
        public static string ThumbsFolder = HttpContext.Current.Server.MapPath(Thumbs);
        public static string OriginalFolder = HttpContext.Current.Server.MapPath(Originals);
        public static string ImageUploadMediumFolder = HttpContext.Current.Server.MapPath(Mediums);
        public static string ImageFolder = HttpContext.Current.Server.MapPath(Images);
        public static string DocumentFolder = HttpContext.Current.Server.MapPath(Document);
        public static string DocumentFolderUrl = HttpContext.Current.Server.MapPath(Document).Replace("FDI.MvcAPI", "FDI.Web");
        public static string MailFolder = HttpContext.Current.Server.MapPath(Mail);
        public static string CopyRight = "Copyright © 2016  ";
        public static string WebTitle = "fditech";
        public static string WebVersion = "v1";
        public static string HeaderFormRequired = "Các trường có dấu <span class=\"star\">*</span> bắt buộc phải nhập.";
    }
}