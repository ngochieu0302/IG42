using System.Linq;
using System.Web;
using System.Collections.Generic;
using FDI.Base;
using FDI.Simple;

namespace FDI.DA
{
    public partial class NewsDA : BaseDA
    {
        public NewsDA(){
        }
        public NewsDA(string pathPaging)
        {
            PathPaging = pathPaging;
        }
        public NewsDA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
       

        /// <summary>
        /// Lấy về tất cả kiểu đơn giản
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        public List<NewsItem> GetAllListSimple(HttpRequestBase httpRequest)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.News_News
                        where c.IsDeleted == false && c.LanguageId == LanguageId
                        orderby c.ID descending 
                        select new NewsItem
                        {
                            ID = c.ID,
                            Title = c.Title,
                            TitleAscii = c.TitleAscii,
                            IsHot = c.IsHot,
                            IsShow = c.IsShow,
                            Description = c.Description,
                            LstInt = c.Categories.Select(p=> p.Id)
                        };
            var cateId = httpRequest.QueryString["CateID"];
            if (!string.IsNullOrEmpty(cateId))
            {
                var id = int.Parse(cateId);
                query = query.Where(c => c.LstInt.Contains(id));
            }
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

        public NewsItem GetItemById(int id)
        {
            var query = from c in FDIDB.News_News
                        where c.ID == id
                        orderby c.Title
                        select new NewsItem
                        {
                            ID = c.ID,
                            Title = c.Title,
                            Description = c.Description,
                            PictureUrl = c.Gallery_Picture.Folder + c.Gallery_Picture.Url,
                            SeoTitle = c.SEOTitle,
                            PictureId = c.PictureID,
                            SeoDescription = c.SEODescription,
                            SeoKeyword = c.SEOKeyword,
                            DateCreated = c.DateCreated,
                            StartDateDisplay = c.StartDateDisplay,
                            IsShow = c.IsShow,
                            IsDeleted = c.IsDeleted,
                            IsHot = c.IsHot
                        };
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <returns></returns>
        public List<SuggestionsTMNews> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in FDIDB.News_News
                        orderby c.Title
                        where c.Title.StartsWith(keyword) && !c.IsDeleted.Value
                        select new SuggestionsTMNews
                        {
                            ID = c.ID,
                            value = c.Title
                        };
            return query.Take(showLimit).ToList();
        }

        /// <summary>
        /// Lấy về dưới dạng Autocomplete
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="showLimit"></param>
        /// <param name="isShow"> </param>
        /// <returns></returns>
        public List<NewsItem> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.News_News
                        orderby c.Title
                        where c.IsShow == isShow && c.IsDeleted == false
                        && c.Title.StartsWith(keyword) && !c.IsDeleted.Value
                        select new NewsItem
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.Take(showLimit).ToList();
        }

        public List<NewsItem>  SearchNewsByConditions(HttpRequestBase httpRequest, List<int> caInts)
        {
            Request = new ParramRequest(httpRequest);

            var query = from c in FDIDB.News_News
                        where c.Categories.Any(m => caInts.Contains(m.Id))  && c.IsDeleted == false
                        orderby c.ID
                        select new NewsItem
                        {
                            ID = c.ID,
                            Title = c.Title,
                            TitleAscii = c.TitleAscii,
                            IsShow = c.IsShow,
                             IsHot = c.IsHot
                        };
           
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
    
        /// <summary>
        /// Lấy về kiểu đơn giản, phân trang
        /// </summary>
        /// <param name="httpRequest"> </param>
        /// <returns>Danh sách bản ghi</returns>
        public List<NewsItem> GetListSimpleByRequest(HttpRequestBase httpRequest, int agencyId)
        { 
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.News_News
                        where !c.IsDeleted.Value && c.LanguageId.Equals(LanguageId)  //&& c.Categories.Any(m => cate.Contains(m.Id))
                        orderby c.ID descending
                        select new NewsItem
                        {
                            ID = c.ID,
                            Title = c.Title,
                            TitleAscii = c.TitleAscii,
                            Details = c.Details,
                            Description = c.Description,
                            SeoDescription = c.SEODescription,
                            LstCategoryItems = c.Categories.Where(m=> m.IsShow == true && m.IsDeleted == false).Select(m=> new CategoryItem
                            {
                                ID = m.Id,
                                Name = m.Name,
                            }),
                            SeoKeyword = c.SEOKeyword,
                            SeoTitle = c.SEOTitle,
                            IsHot = c.IsHot,
                            IsShow = c.IsShow,
                            DateCreated = c.DateCreated,
                            StartDateDisplay = c.StartDateDisplay,
                            IsDeleted = c.IsDeleted,
                            CateAscii = c.Categories.FirstOrDefault().Slug,
                            CateName = c.Categories.FirstOrDefault().Name,
                        };

            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<Category> GetListCategory(List<int> ltsArrId)
        {
            var query = from c in FDIDB.Categories where ltsArrId.Contains(c.Id) select c;
            return query.ToList();
        }
        public List<NewsItem> GetListSimpleByArrID(List<int> ltsArrID)
        {
            var query = from o in FDIDB.News_News
                        where ltsArrID.Contains(o.ID) && !o.IsDeleted.Value
                        orderby o.ID descending
                        select new NewsItem
                        {
                            ID = o.ID,
                            Title = o.Title,
                            TitleAscii = o.TitleAscii,
                            IsShow = o.IsShow,
                            Details = o.Details,
                            Description = o.Description,
                            IsHot = o.IsHot,
                            SeoDescription = o.SEODescription,
                            SeoKeyword = o.SEOKeyword,
                            SeoTitle = o.SEOTitle,
                            PictureUrl = (o.PictureID.HasValue) ? o.Gallery_Picture.Folder + o.Gallery_Picture.Url : string.Empty,
                            Tags = o.System_Tag.Select(c => new TagItem
                            {
                                ID = c.ID,
                                Name = c.Name,
                                NameAscii = c.NameAscii
                            }).ToList()
                        };
            TotalRecord = query.Count();
            return query.ToList();
        }

        #region Check Exits, Add, Update, Delete
        /// <summary>
        /// Lấy về bản ghi qua khóa chính
        /// </summary>
        /// <param name="newsID">ID bản ghi</param>
        /// <returns>Bản ghi</returns>
        public News_News GetById(int newsID)
        {
            var query = from c in FDIDB.News_News where c.ID == newsID select c;
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<System_Tag> GetListIntTagByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.System_Tag where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }
        public List<System_Tag> GetListTagByArrID(List<string> ltsArrID)
        {
            var query = from c in FDIDB.System_Tag where ltsArrID.Contains(c.Name) select c;
            return query.ToList();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<Shop_Product> GetListIntProductByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Shop_Product where ltsArrID.Contains(c.ID) && c.IsDelete == false && c.IsShow == false select c;
            return query.ToList();
        }
        public List<Shop_Product> GetListProductByArrID(List<string> ltsArrID)
        {
           // var query = from c in FDIDB.Shop_Product where ltsArrID.Contains(c.Name) && c.IsDelete == false && c.IsShow == false select c;
            var query = from c in FDIDB.Shop_Product where  c.IsDelete == false && c.IsShow == false select c;
            return query.ToList();
        }

        /// <summary>
        /// Lấy về danh sách qua mảng id
        /// </summary>
        /// <param name="ltsArrID">Mảng ID</param>
        /// <returns>Danh sách bản ghi</returns>
        public List<News_News> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.News_News where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        /// <summary>
        /// Kiểm tra bản ghi đã tồn tại hay chưa
        /// </summary>
        /// <param name="title">Tên bản ghi hiện tại</param>
        /// <param name="id">id của bạn ghi hiện tại</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool CheckExits(string title, int id)
        {
            var query = (from c in FDIDB.News_News
                         where c.Title == title && c.ID != id && c.IsDeleted == false
                         select c).Count();
            return query > 0;
        }

        /// <summary>
        /// Kiểm tra link ascii đã tồn tại hay chưa
        /// </summary>
        /// <param name="titleascii">Tên bản ghi hiện tại</param>
        /// <param name="id">id của bạn ghi hiện tại</param>
        /// <returns>Trạng thái tồn tại</returns>
        public bool CheckTitleAsciiExits(string titleascii, int id)
        {
            var query = (from c in FDIDB.News_News
                         where c.TitleAscii == titleascii && c.ID != id && c.IsDeleted == false
                         select c).Count();
            return query > 0;
        }

        /// <summary>
        /// Lấy về bản ghi qua tên
        /// </summary>
        /// <param name="title"> </param>
        /// <returns>Bản ghi</returns>
        public News_News GetByName(string title)
        {
            var query = from c in FDIDB.News_News where ((c.Title == title)) select c;
            return query.FirstOrDefault();
        }



        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="newsNews">bản ghi cần thêm</param>
        public void Add(News_News newsNews)
        {
            FDIDB.News_News.Add(newsNews);
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
