using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class NewsItem : BaseSimple
    {
        public string Title { get; set; }
        public string TitleAscii { get; set; }
        public string CateAscii { get; set; }
        public string CateName { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? StartDateDisplay { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsHot { get; set; }
        public int? PictureId { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsDeleted { get; set; }
        public string Details { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoKeyword { get; set; }
        public string FilePdf { get; set; }
        public string UserCreate;
        public IEnumerable<int> LstInt { get; set; }
        public IEnumerable<TagItem> Tags { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public IEnumerable<CategoryItem> LstCategoryItems { get; set; }
    }
    public class ModelNewsItem : BaseModelSimple
    {
        public IEnumerable<CategoryItem> LstCategoryItems { get; set; }
        public IEnumerable<NewsItem> ListItem { get; set; }
        public NewsItem NewsItem { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public string NewUrl { get; set; }
        public int Total { get; set; }
    }

    public class NewsAppItem : BaseSimple
    {
        public string T { get; set; } // tieu de bai viet
        public DateTime? Dc { get; set; } // ngay dang
        public string D { get; set; } // mo ta ngan
        public string C { get; set; } // noi dung bai viet
        public string Pu { get; set; }
        public string Cname { get; set; }
        public int CId { get; set; }
    }
}
