using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class PartnerItem : BaseSimple
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string PictureUrl { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public string Details { get; set; }
        public string Description { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTitle { get; set; }
        public string SeoKeyword { get; set; }
        public IEnumerable<PictureItem> LstPictures { get; set; }
    }
    public class ModelPartnerItem : BaseModelSimple
    {
        public IEnumerable<PartnerItem> ListItem { get; set; }
        public PartnerItem PartnerItem { get; set; }
    }
}
