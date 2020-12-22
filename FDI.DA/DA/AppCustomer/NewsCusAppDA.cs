using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FDI.Simple;
using FDI.Utils;

namespace FDI.DA.DA.AppCustomer
{
     public class NewsCusAppDA:BaseDA
    {
         #region Contruction

        public NewsCusAppDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public NewsCusAppDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }

        #endregion
        public List<NewsAppItem> GetListNews(int rowPerPage, int page, ref int total)
        {
           
            var query = from c in FDIDB.News_News
                        where c.IsShow == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        orderby c.DateCreated descending
                        select new NewsAppItem
                        { 
                            T = c.Title,
                            ID = c.ID,
                            Dc = c.DateCreated,
                            D = c.Description,
                            Pu = c.Gallery_Picture.Url,
                            Cname = c.Categories.FirstOrDefault().Name,
                            CId = c.Categories.FirstOrDefault().Id
                        };
            return query.Paging(page, rowPerPage, ref total).ToList();
        }
        public List<NewsAppItem> GetListNewsHot()
        {
            var query = from c in FDIDB.News_News
                where c.IsShow == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                      && c.IsHot == true
                orderby c.DateCreated descending
                select new NewsAppItem
                {
                    Cname = c.Categories.FirstOrDefault().Name,
                    T = c.Title,
                    ID = c.ID,
                    Dc = c.DateCreated,
                    D = c.Description,
                    Pu = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                    CId = c.Categories.FirstOrDefault().Id
                };
            return query.ToList();
        }
        public NewsAppItem GetNewsItem(int id)
        {
            var query = from c in FDIDB.News_News
                        where c.IsShow == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                        && c.ID == id
                        orderby c.DateCreated descending
                        select new NewsAppItem
                        {
                            T = c.Title,
                            ID = c.ID,
                            Dc = c.DateCreated,
                            C = c.Details,
                            CId = c.Categories.FirstOrDefault().Id
                        };
            return query.FirstOrDefault();
        }

        public List<NewsAppItem> GetListNewsbyCateIdApp(int id)
        {
            var query = from c in FDIDB.News_News
                where c.IsShow == true && (!c.IsDeleted.HasValue || c.IsDeleted == false)
                                       && c.Categories.Any(a=>a.Id == id)
                orderby c.DateCreated descending
                select new NewsAppItem
                {
                    T = c.Title,
                    ID = c.ID,
                    Dc = c.DateCreated,
                    C = c.Details,
                    CId = c.Categories.FirstOrDefault().Id
                };
            return query.Take(10).ToList();
        }
    }
}
