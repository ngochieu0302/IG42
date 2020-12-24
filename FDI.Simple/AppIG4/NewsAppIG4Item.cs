using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class NewsAppIG4Item : BaseSimple
    {
        public string Title { get; set; }
        public string TitleAscii { get; set; }
        public string CateAscii { get; set; }
        public string CateName { get; set; }
        public int? Step { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public string PhotoCaption { get; set; }
        public string ImgFile { get; set; }
        public string Author { get; set; }
        public string UserName { get; set; }
        public string UserActive { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdate { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsHot { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsHome { get; set; }
        public bool? IsDeleted { get; set; }
        public string Details { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoKeyword { get; set; }
        public string UserCreate { get; set; }
        public int? Quantity { get; set; }
        public int? Type { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public IEnumerable<int> LstInt { get; set; }
        public IEnumerable<TagItem> Tags { get; set; }
        public IEnumerable<CustomerTypeAppIG4Item> OrderPackageItems { get; set; }
        public IEnumerable<ListCateName> LstCategoryItems { get; set; }
    }

    public class ListCateName
    {
        public string Name { get; set; }
    }
    public class ModelNewsAppIG4Item : BaseModelSimple
    {
        public IEnumerable<NewsItem> ListItem { get; set; }
        public IEnumerable<CategoryItem> LstCategoryItems { get; set; }
        public CategoryItem CategoryItem { get; set; }
        public NewsItem NewsItem { get; set; }
        public int ortherId { get; set; }
        public decimal? Total { get; set; }
        public int CategoryID { get; set; }
        public string NewUrl { get; set; }
        public string Keyword { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
    }
}
