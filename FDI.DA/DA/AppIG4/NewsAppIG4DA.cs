using System.Linq;
using System.Web;
using System.Collections.Generic;
using FDI.Base;
using FDI.Simple;
using FDI.Utils;
using System;
using FDI.CORE;

namespace FDI.DA
{
    public partial class NewsAppIG4DA : BaseDA
    {
        #region Constructer
        public NewsAppIG4DA()
        {
        }

        public NewsAppIG4DA(string pathPaging)
        {
            PathPaging = pathPaging;
        }

        public NewsAppIG4DA(string pathPaging, string pathPagingExt)
        {
            PathPaging = pathPaging;
            PathPagingext = pathPagingExt;
        }
        #endregion

        public List<NewsAppIG4Item> GetAllListSimple(HttpRequestBase httpRequest, bool isAdmin, Guid userid, string username)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.News_News
                        where c.LanguageId == LanguageId
                        && (isAdmin == true || c.UserName == username
                        || c.Categories.Any(m => m.Steps.Any(s => s.UserID == userid)))
                        && c.IsDeleted == false
                        orderby c.ID descending
                        select new NewsAppIG4Item
                        {
                            ID = c.ID,
                            Title = c.Title,
                            TitleAscii = c.TitleAscii,
                            IsHot = c.IsHot,
                            IsHome = c.IsHome,
                            IsDeleted = c.IsDeleted,
                            IsShow = c.IsShow,
                            IsActive = c.IsActive,
                            Author = c.Author,
                            UserActive = c.UserActive,
                            UserName = c.aspnet_Users.UserName,
                            Step = c.StepID,
                            Description = c.Description,
                            LstInt = c.Categories.Select(p => p.Id)
                        };
            var cateId = httpRequest.QueryString["CateID"];
            var ShowType = httpRequest.QueryString["ShowType"];
            if (!string.IsNullOrEmpty(ShowType))
            {
                if (ShowType.ToLower() == "ishome")
                    query = query.Where(c => c.IsHome == true);
                if (ShowType.ToLower() == "isdeleted")
                    query = query.Where(c => c.IsDeleted == true);
                if (ShowType.ToLower() == "ishide")
                    query = query.Where(c => !c.IsShow.HasValue || !c.IsShow.Value);
                if (ShowType.ToLower() == "isshow")
                    query = query.Where(c => c.IsShow == true);
                if (ShowType.ToLower() == "ishot")
                    query = query.Where(c => c.IsHot == true);
            }
            if (!string.IsNullOrEmpty(cateId))
            {
                var id = int.Parse(cateId);
                query = query.Where(c => c.LstInt.Contains(id));
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }
        public List<NewsAppIG4Item> GetAllListSimpleStruct(HttpRequestBase httpRequest, bool isAdmin, Guid userid, string username)
        {
            Request = new ParramRequest(httpRequest);
            var query = from c in FDIDB.News_News
                        where c.LanguageId == LanguageId
                        && (isAdmin == true || c.UserName == username
                        || c.Categories.Any(m => m.Steps.Any(s => s.UserID == userid))) && c.catid == (int)ModuleType.Struct
                        && c.IsDeleted == false
                        orderby c.ID
                        select new NewsAppIG4Item
                        {
                            ID = c.ID,
                            Title = c.Title,
                            TitleAscii = c.TitleAscii,
                            IsHot = c.IsHot,
                            IsHome = c.IsHome,
                            IsDeleted = c.IsDeleted,
                            IsShow = c.IsShow,
                            IsActive = c.IsActive,
                            Author = c.Author,
                            UserActive = c.UserActive,
                            UserName = c.aspnet_Users.UserName,
                            Step = c.StepID,
                            Description = c.Description,
                            LstInt = c.Categories.Select(p => p.Id)
                        };

            var ShowType = httpRequest.QueryString["ShowType"];
            if (!string.IsNullOrEmpty(ShowType))
            {
                if (ShowType.ToLower() == "ishome")
                    query = query.Where(c => c.IsHome == true);
                if (ShowType.ToLower() == "isdeleted")
                    query = query.Where(c => c.IsDeleted == true);
                if (ShowType.ToLower() == "ishide")
                    query = query.Where(c => !c.IsShow.HasValue || !c.IsShow.Value);
                if (ShowType.ToLower() == "isshow")
                    query = query.Where(c => c.IsShow == true);
                if (ShowType.ToLower() == "ishot")
                    query = query.Where(c => c.IsHot == true);
            }
            query = query.SelectByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<NewsAppIG4Item> StatisticsByNew(HttpRequestBase httpRequest, out decimal? total)
        {
            Request = new ParramRequest(httpRequest);
            var now = DateTime.Now;
            var str = httpRequest["fromdate"];
            var end = httpRequest["todate"];
            var startdate = !string.IsNullOrEmpty(str) ? str.StringToDate() : new DateTime(now.Year, now.Month, 1);
            var endDate = !string.IsNullOrEmpty(end) ? end.StringToDate().AddDays(1) : now;
            var query = from c in FDIDB.News_News
                        where c.LanguageId == LanguageId
                        && c.IsDeleted == false && c.IsActive == true
                        && c.DateCreated >= startdate && c.DateCreated <= endDate
                        orderby c.ID descending
                        select new NewsAppIG4Item
                        {
                            ID = c.ID,
                            DateCreated = c.DateCreated,
                            DateUpdate = c.DateUpdated,
                            Title = c.Title,
                            Author = c.Author,
                            UserName = c.UserName,
                            UserActive = c.UserActive,
                            Price = c.Price
                        };
            query = query.SelectByRequest(Request);
            total = query.Any() ? query.Sum(m => m.Price ?? 0) : 0;
            query = query.SelectPageByRequest(Request, ref TotalRecord);
            return query.ToList();
        }

        public List<NewsAppIG4Item> StatisticsByAuthor(HttpRequestBase httpRequest)
        {
            var now = DateTime.Now;
            var str = httpRequest["fromdate"];
            var end = httpRequest["todate"];
            var startdate = !string.IsNullOrEmpty(str) ? str.StringToDate() : new DateTime(now.Year, now.Month, 1);
            var endDate = !string.IsNullOrEmpty(end) ? end.StringToDate().AddDays(1) : now;
            var query = from c in FDIDB.sp_thongkenguoiviet(startdate, endDate)
                        select new NewsAppIG4Item
                        {
                            UserName = c.UserName,
                            Quantity = c.Quantity
                        };
            return query.ToList();
        }
        public List<NewsAppIG4Item> GetListAll(int page, int totalpage)
        {
            var skip = page < 2 ? 0 : page * totalpage;
            var query = from c in FDIDB.News_News
                        where (!c.IsDeleted.HasValue || !c.IsDeleted.Value) && c.IsShow == true
                        orderby c.ID
                        select new NewsAppIG4Item
                        {
                            ID = c.ID,
                            Title = c.Title,
                            Description = c.Description,
                            DateCreated = c.DateCreated,
                            PictureUrl = c.Gallery_Picture.Folder + c.Gallery_Picture.Url
                        };
            return query.Skip(skip).Take(totalpage).ToList();
        }
        public NewsAppIG4Item GetItemById(int id)
        {
            var query = from c in FDIDB.News_News
                        where (!c.IsDeleted.HasValue || !c.IsDeleted.Value) && c.ID == id
                        select new NewsAppIG4Item
                        {
                            ID = c.ID,
                            Title = c.Title,
                            Description = c.Description,
                            DateCreated = c.DateCreated,
                            Details = c.Details,
                            PictureUrl = c.Gallery_Picture.Folder + c.Gallery_Picture.Url
                        };
            return query.FirstOrDefault();
        }
        public List<NewsAppIG4Item> StatisticsByActive(HttpRequestBase httpRequest)
        {
            var now = DateTime.Now;
            var str = httpRequest["fromdate"];
            var end = httpRequest["todate"];
            var startdate = !string.IsNullOrEmpty(str) ? str.StringToDate() : new DateTime(now.Year, now.Month, 1);
            var endDate = !string.IsNullOrEmpty(end) ? end.StringToDate().AddDays(1) : now;
            var query = from c in FDIDB.sp_thongkenguoiduyet(startdate, endDate)
                        select new NewsAppIG4Item
                        {
                            UserActive = c.UserActive,
                            Quantity = c.Quantity
                        };
            return query.ToList();
        }
        public List<NewsAppIG4Item> GetListSimpleByAutoComplete(string keyword, int showLimit)
        {
            var query = from c in FDIDB.News_News
                        orderby c.Title
                        where c.Title.StartsWith(keyword) && (!c.IsDeleted.HasValue || !c.IsDeleted.Value)
                        select new NewsAppIG4Item
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.Take(showLimit).ToList();
        }

        public List<NewsAppIG4Item> GetListSimpleByAutoComplete(string keyword, int showLimit, bool isShow)
        {
            var query = from c in FDIDB.News_News
                        orderby c.Title
                        where c.IsShow == isShow && (!c.IsDeleted.HasValue || !c.IsDeleted.Value) && c.Title.StartsWith(keyword)
                        select new NewsAppIG4Item
                        {
                            ID = c.ID,
                            Title = c.Title
                        };
            return query.Take(showLimit).ToList();
        }

        #region Check Exits, Add, Update, Delete
        public News_News GetById(int newsID)
        {
            var query = from c in FDIDB.News_News where c.ID == newsID select c;
            return query.FirstOrDefault();
        }

        public List<Category> GetListCategoryByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.Categories where ltsArrID.Contains(c.Id) select c;
            return query.ToList();
        }
        public Step GetStep(Guid UserID, List<int> ltsArrID)
        {
            var query = from c in FDIDB.Steps where c.UserID == UserID && ltsArrID.Contains(c.Category.Id) select c;
            return query.FirstOrDefault();
        }


        public List<News_News> GetListByArrID(List<int> ltsArrID)
        {
            var query = from c in FDIDB.News_News where ltsArrID.Contains(c.ID) select c;
            return query.ToList();
        }

        public bool CheckTitleAsciiExits(string titleascii, int id)
        {
            var query = (from c in FDIDB.News_News
                         where c.TitleAscii == titleascii && c.ID != id && (!c.IsDeleted.HasValue || !c.IsDeleted.Value)
                         select c).Count();
            return query > 0;
        }

        public void Add(News_News newsNews)
        {
            FDIDB.News_News.Add(newsNews);
        }

        public void Save()
        {
            FDIDB.SaveChanges();
        }
        #endregion
    }
}
