using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class AdvertisingItem : BaseSimple
    {
        public string Name { get; set; }

        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public int? PictureId { get; set; }
        public string PictureUrl { get; set; }
        public int? Sort { get; set; }
        public int? PositionID { get; set; }
        public string PositionName { get; set; }
        public string Link { get; set; }
        public string Content { get; set; }
        public string UrlVideo { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public bool? Show { get; set; }
        public bool? IsDeleted { get; set; }
        public decimal? CreateOnUtc { get; set; }
        public decimal? StartDate { get; set; }
        public decimal? EndDate { get; set; }
        public List<AdvertisingTypeItem> AdvertisingType { get; set; }
        public List<CategoryItem> ListCategoryItem { get; set; }

        public PictureItem GalleryPicture { get; set; }
        public AdvertisingPositionItem AdvertisingPosition { get; set; }
    }
    public class ModelAdvertisingItem : BaseModelSimple
    {
        public IEnumerable<AdvertisingItem> ListItem { get; set; }
        public AdvertisingItem AdvertisingItem { get; set; }
        public int? cateId { get; set; }
        public IEnumerable<AdvertisingPositionItem> ListPositionItem { get; set; }
    }
}
