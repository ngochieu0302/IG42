using System.Linq;
using System.Web;
using System.Collections.Generic;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class BannerDA : BaseDA
    {
        public BannerDA()
        {
        }
        public BannerDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public BannerDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }


        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<BannerItem> GetAllListSimple(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.Banners
                        where c.IsDelete == false
                        orderby c.ID descending
                        select new BannerItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            //PictureUrl = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Description = c.Description,
                            UrlView = c.UrlView,
                            //Datas = c.Datas
                        };

            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <param name="isShow">Kiểm tra hiển thị</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<NewsItem> GetListSimpleAll(bool isShow)
        {
            var query = from c in FDIDB.News_News
                        where (c.IsShow == isShow) && !c.IsDeleted.Value
                        orderby c.Title
                        select new NewsItem
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.ToList();
        }

        public BannerItem GetItemById(int id)
        {
            var query = from c in FDIDB.Banners
                        where c.ID == id
                        orderby c.Sort descending
                        select new BannerItem
                        {
                            ID = c.ID,
                            Description = c.Description,
                            //PictureUrl = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            //Datas = c.Datas,
                            UrlView = c.UrlView,
                            Name = c.Name,
                            Details = c.Details,
                            Sort = c.Sort,
                            PictureID = c.PictureID ?? 0
                        };
            return query.FirstOrDefault();
        }


        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="newsID">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public Banner GetById(int newsID)
        {
            var query = from c in FDIDB.Banners where c.ID == newsID select c;
            return query.FirstOrDefault();
        }

        public List<BannerItem> GetAll()
        {
            var query = from c in FDIDB.Banners
                        where c.IsDelete == false
                        orderby c.ID descending
                        select new BannerItem
                        {
                            ID = c.ID,
                            Name = c.Name,
                            //PictureUrl = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            Description = c.Description,
                            //Datas = c.Datas
                        };

            return query.ToList();
        }


        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="newsNews">bản ghi cần thêm</param>
        public void Add(Banner obj)
        {
            FDIDB.Banners.Add(obj);
        }

        /// <summary>
        /// Xóa bản ghi
        /// </summary>
        /// <param name="newsNews">Xóa bản ghi</param>
        public void Delete(News_News newsNews)
        {
            FDIDB.News_News.Remove(newsNews);
        }

        /// <summary>
        /// save bản ghi vào DB
        /// </summary>
        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion
    }
}
