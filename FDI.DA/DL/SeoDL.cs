using System.Linq;
using FDI.Simple;

namespace FDI.DA
{
    public class SeoDL : BaseDA
    {
        public SEOItem GetSeoTag(string url)
        {
            var query = from n in FDIDB.System_Tag
                        where n.NameAscii == url
                        select new SEOItem
                        {
                            SeoTitle = n.SEOTitle,
                            SeoDescription = n.SEODescription,
                            SeoKeyword = n.SEOKeyword
                        };
            return query.FirstOrDefault();
        }

        
        public SEOItem GetSeoNews(string url)
        {
            var query = from n in FDIDB.News_News
                        where n.TitleAscii.ToLower() == url.ToLower() && (!n.IsDeleted.HasValue || !n.IsDeleted.Value)
                        select new SEOItem
                        {
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            SeoTitle = n.SEOTitle,
                            SeoDescription = n.SEODescription,
                            SeoKeyword = n.SEOKeyword
                        };
            return query.FirstOrDefault();
        }

        public SEOItem GetSeoProduct(int id)
        {
            var query = from n in FDIDB.Shop_Product_Detail
                        where n.ID == id
                        select new SEOItem
                        {
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            SeoTitle = n.SEOTitle,
                            SeoDescription = n.SEODescription,
                            SeoKeyword = n.SEOKeyword
                        };
            return query.FirstOrDefault();
        }
        public SEOItem GetSeoPartner(int id)
        {
            var query = from n in FDIDB.Partners
                        where n.ID == id
                        select new SEOItem
                        {
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            SeoTitle = n.SEOTitle,
                            SeoDescription = n.SEODescription,
                            SeoKeyword = n.SEOKeyword
                        };
            return query.FirstOrDefault();
        }

        public SEOItem GetSeoCategory(string url, int type)
        {
            var query = from n in FDIDB.Categories
                        where n.Slug == url && n.Type == type
                        select new SEOItem
                        {
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            SeoTitle = n.SEOTitle,
                            SeoDescription = n.SEODescription,
                            SeoKeyword = n.SEOKeyword
                        };
            return query.FirstOrDefault();
        }

        public SEOItem GetSeoPage(int url)
        {

            var query = from n in FDIDB.ModulePages
                        where n.Id == url
                        select new SEOItem
                        {
                            PictureUrl = n.FeUrl,
                            SeoTitle = n.SEOTitle,
                            SeoDescription = n.SEODescription,
                            SeoKeyword = n.SEOKeyword
                        };
            if (url == 0)
            {
                query = from n in FDIDB.ModulePages
                        where n.Key == "home"
                        select new SEOItem
                        {
                            PictureUrl = n.FeUrl,
                            SeoTitle = n.SEOTitle,
                            SeoDescription = n.SEODescription,
                            SeoKeyword = n.SEOKeyword
                        };
            }
            return query.FirstOrDefault();
        }
    }
}