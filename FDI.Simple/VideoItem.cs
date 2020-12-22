using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class VideoItem:BaseSimple
    {
        public int? AlbumId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public int? PictureId { get; set; }
        public string Url { get; set; }
        public string UrlYoutube { get; set; }
        public string Description { get; set; }
        public decimal? DateCreated { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public bool IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public GalleryPictureItem GalleryPicture { get; set; }
        public string UrlPicture { get; set; }
        public string CategoryName { get; set; }
        public string CategorySlug { get; set; }

    }
    public class ModelVideoItem : BaseModelSimple
    {
        public IEnumerable<VideoItem> ListItem { get; set; }
        public IEnumerable<CategoryItem> ListCategoryItem { get; set; }
        public SystemConfigItem ConfigItem { get; set; }
        public int Total { get; set; }
    }
}
