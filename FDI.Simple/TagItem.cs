using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class TagItem : BaseSimple
    {
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }
        public string NameAscii { get; set; }
        public string Url { get; set; }
        public bool? IsShow { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyword { get; set; }
        public IEnumerable<NewsItem> ListNewsItem { get; set; }
        public IEnumerable<ProductItem> ListProductItem { get; set; }
    }
    public class ModelTagItem : BaseModelSimple
    {
        public IEnumerable<TagItem> ListItem { get; set; }

    }
}
