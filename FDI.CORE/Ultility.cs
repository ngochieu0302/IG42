namespace FDI.CORE
{
    public static class Ultility
    {
        public static string ImageLogo(this string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName)) return "";
            return WebConfigCore.ImageLogo + pictureName;
        }
        public static string UploadPicture(this string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName)) return "";
            return WebConfigCore.UrlUploadsImage + pictureName;
        }
        public static string UploadMailPicture(this string pictureName)
        {
            return (string.IsNullOrEmpty(pictureName)) ? "" : WebConfigCore.UrlUploadsMailImage + pictureName;
        }
        public static string Picture(this string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName)) return "";
            var list = pictureName.Split('/');
            pictureName = pictureName.Replace(list[0], "");
            return WebConfigCore.UrlImage + pictureName;
        }
        public static string TempPicture(this string pictureName)
        {
            return WebConfigCore.TempImage + pictureName;
        }
    }
}
