namespace FDI.Utils
{
    public class UrlWrite
    {
        // News
        public static string NewsPage(string lang, string slug, int pageid, int page)
        {
            return string.Format("/{0}{1}-p{2}c{3}", lang, slug, pageid, page);
        }

        public static string Detail(string slug, int? pageId, int newsId)
        {
            return string.Format("/{0}-p{1}n{2}.html", slug, pageId, newsId);
        }

        //Product

        public static string Products(string slug, int id)
        {
            return string.Format("/{0}-p{1}", slug, id);
        }

        public static string ProductsPage(string slug, int id, int page)
        {
            return string.Format("/{0}-p{1}/{2}", slug, id, page);
        }

        public static string ProductsDetail(string slug, int id)
        {
            return string.Format("/{0}-p{1}.html", slug, id);
        }

        public static string Cate(string slug, int? pageId, int id)
        {
            return string.Format("/{0}-p{1}c{2}", slug, pageId, id);
        }

        public static string PDetail(string slug, int? pageId, int newsId)
        {
            return string.Format("/{0}-p{1}n{2}.html", slug, pageId, newsId);
        }

        public static string Tag(string slug, int? pageId, int id)
        {
            return string.Format("/tag-{0}-p{1}c{2}", slug, pageId, id);
        }
        //video
        public static string Video(string slug)
        {
            return string.Format("/{0}", slug);
        }

        public static string VideoPage(string slug, int pageId, int cateId)
        {
            return string.Format("/{0}-p{1}c{2}p", slug, pageId, cateId);
        }
        public static string VideoSearch(string slug, int pageId, int cateId)
        {
            return string.Format("/{0}-p{1}c{2}", slug, pageId, cateId);
        }
    }
}
