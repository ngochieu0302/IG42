using System.Collections.Generic;
using System.Linq;
using FDI.Simple;

namespace FDI.DA
{
    public class NewsDL : BaseDA
    {
        public List<NewsItem> GetList(int homeId, int cateId)
        {
            var query = from n in FDIDB.News_News
                        where n.IsDeleted == false && n.IsShow == true
                        orderby n.ID descending
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            DateCreated = n.DateCreated,
                        };
            return query.Take(12).ToList();
        }
        public List<NewsItem> GetListPDF(int slug, int page, int rowPage, ref int total)
        {
            var query = from n in FDIDB.News_News
                        where n.IsDeleted == false && n.IsShow == true && n.LanguageId == LanguageId
                        && (n.Categories.Any(c => c.Id == slug) || n.Categories.Any(c => c.ParentId == slug))
                        && n.LinkTinTuc != null
                        orderby n.DateCreated descending
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                            //UserCreate = n.aspnet_Users.UserName,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            Description = n.Description,
                            DateCreated = n.DateCreated,
                        };
            query = query.Paging(page, rowPage, ref total);
            return query.ToList();
        }
        public List<NewsItem> GetListHot()
        {
            var query = from n in FDIDB.News_News
                        where n.IsDeleted == false && n.IsShow == true && n.IsHot == true
                        orderby n.ID descending
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                            DateCreated = n.DateCreated,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                        };
            return query.Take(12).ToList();
        }
        public List<NewsItem> GetList(int slug, int page, int rowPage, ref int total)
        {
            var query = from n in FDIDB.News_News
                        where n.IsDeleted == false && n.IsShow == true && n.LanguageId == LanguageId
                        && (n.Categories.Any(c => c.Id == slug) || n.Categories.Any(c => c.ParentId == slug))
                        orderby n.DateCreated descending
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                            //UserCreate = n.aspnet_Users.UserName,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            Description = n.Description,
                            DateCreated = n.DateCreated,
                            CategoryItem = (from u in n.Categories
                                            orderby u.IsLevel descending
                                            select new CategoryItem
                                            {
                                                ID = u.Id,
                                                Name = u.Name,
                                                Slug = u.Slug
                                            }).FirstOrDefault()
                        };
            query = query.Paging(page, rowPage, ref total);
            return query.ToList();
        }

        public List<NewsItem> GetNewKeyword(string key, int page, int rowPage, ref int total)
        {
            var query = from n in FDIDB.News_News
                        where n.IsDeleted == false && n.IsShow == true && n.LanguageId == LanguageId
                        && n.TitleAscii.Contains(key)
                        orderby n.ID descending
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                            //UserCreate = n.aspnet_Users.UserName,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            Description = n.Description,
                            DateCreated = n.DateCreated,
                            CategoryItem = (from u in n.Categories
                                            orderby u.IsLevel descending
                                            select new CategoryItem
                                            {
                                                ID = u.Id,
                                                Name = u.Name,
                                                Slug = u.Slug
                                            }).FirstOrDefault()
                        };
            query = query.Paging(page, rowPage, ref total);
            return query.ToList();
        }

        public List<NewsItem> GetNewByTag(string slug, int cateId, int page, int rowPage, ref int total)
        {
            var query = from n in FDIDB.News_News
                        where n.IsDeleted == false && n.IsShow == true && n.LanguageId == LanguageId
                        && (n.System_Tag.Any(c => c.ID == cateId) || n.System_Tag.Any(c => c.NameAscii.Equals(slug)))
                        orderby n.DateCreated descending
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                            //UserCreate = n.aspnet_Users.UserName,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            Description = n.Description,
                            DateCreated = n.DateCreated,
                            CategoryItem = (from u in n.Categories
                                            orderby u.IsLevel descending
                                            select new CategoryItem
                                            {
                                                ID = u.Id,
                                                Name = u.Name,
                                                Slug = u.Slug
                                            }).FirstOrDefault()
                        };
            query = query.Paging(page, rowPage, ref total);
            return query.ToList();
        }
        public string GetName(int id)
        {
            return FDIDB.Categories.Where(c => c.Id == id).Select(c => c.Name).FirstOrDefault();
        }
        public List<NewsItem> GetListByCateId(int id, int take)
        {
            var query = from n in FDIDB.News_News
                        where n.IsDeleted == false && n.LanguageId == LanguageId
                        && n.IsShow == true && n.Categories.Any(c => c.Id == id)
                        orderby n.DateCreated descending
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            IsHot = n.IsHot,
                            Title = n.Title,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            Description = n.Description,
                            DateCreated = n.DateCreated
                        };
            if (take != 0)
                query = query.Take(take);
            return query.ToList();
        }

        public List<NewsItem> GetListByDate(int take)
        {
            var query = from n in FDIDB.News_News
                        where n.IsDeleted == false && n.LanguageId == LanguageId && n.IsShow == true 
                        orderby n.DateCreated descending
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            IsHot = n.IsHot,
                            Title = n.Title,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            Description = n.Description,
                            DateCreated = n.DateCreated
                        };
            if (take != 0)
                query = query.Take(take);
            return query.ToList();
        }
        
        public List<CategoryItem> GetListCateId(int id)
        {
            var query = from n in FDIDB.Categories
                        where n.IsDeleted == false && n.LanguageId == LanguageId
                              && n.IsMenu == true && n.ParentId == id
                        select new CategoryItem
                        {
                            ID = n.Id,
                            Name = n.Name,
                            Slug = n.Slug,
                            UrlPicture = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            ListNewsItem = (from c in n.News_News
                                            where c.IsDeleted == false && c.IsHot == true && c.IsShow == true
                                            orderby c.DateCreated descending
                                            select new NewsItem
                                            {
                                                ID = c.ID,
                                                TitleAscii = c.TitleAscii,
                                                Title = c.Title

                                            })
                        };

            return query.ToList();
        }
        public List<NewsItem> GetListOther(int id, int ortherId)
        {
            var query = from n in FDIDB.News_News
                        where n.IsDeleted == false && n.IsShow == true
                        && n.ID != ortherId && n.Categories.Any(c => c.Id == id)
                        orderby n.DateCreated descending
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : ""
                        };
            return query.Take(16).ToList();
        }
        public NewsItem GetNewsId(int id, string slug)
        {
            var query = from n in FDIDB.News_News
                        where (n.ID == id || n.TitleAscii == slug) && n.IsDeleted == false
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                            //UserCreate = n.aspnet_Users.UserName,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            DateCreated = n.DateCreated,
                            Details = n.Details,

                            CateName = n.Categories.FirstOrDefault().Name,
                            LstInt = n.Categories.Where(c=>c.IsDeleted == false && c.IsShow == true).Select(a=>a.Id),
                            FilePdf = n.LinkTinTuc,
                            CategoryItem = (from u in n.Categories
                                            orderby u.IsLevel descending
                                            select new CategoryItem
                                            {
                                                ID = u.Id,
                                                Name = u.Name,
                                                Slug = u.Slug
                                            }).FirstOrDefault(),
                            Tags = n.System_Tag.Where(u => u.IsDelete == false && u.IsShow == true).Select(v => new TagItem
                            {
                                ID = v.ID,
                                Name = v.Name,
                                Url = v.Url,
                                NameAscii = v.NameAscii
                            })
                        };
            return query.FirstOrDefault();
        }
        public NewsItem GetNewsTitleAssci(string slug)
        {
            var query = from n in FDIDB.News_News
                        where  n.TitleAscii == slug
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                        };
            return query.FirstOrDefault();
        }
        public NewsItem GetNewsItem(string name)
        {
            var query = from n in FDIDB.News_News
                        where n.TitleAscii.Contains(name)
                        select new NewsItem
                        {
                            ID = n.ID,
                            TitleAscii = n.TitleAscii,
                            Title = n.Title,
                            Description = n.Description,
                            PictureUrl = n.Gallery_Picture.IsDeleted != true ? n.Gallery_Picture.Folder + n.Gallery_Picture.Url : "",
                            DateCreated = n.DateCreated
                        };
            return query.FirstOrDefault();
        }
    }
}