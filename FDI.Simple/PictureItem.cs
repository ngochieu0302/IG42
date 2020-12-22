using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class PictureItem : BaseSimple
    {
        public int? AlbumID { get; set; }
        public string LanguageId { get; set; }
        public int? CategoryID { get; set; }
        public int? ModuleType { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public decimal? DateCreated { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? SourceID { get; set; }
        public int? AgencyId { get; set; }
        public string Folder { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public CategoryItem Category { get; set; }
        public int? Type { get; set; }
    }
    public class ModelPictureItem : BaseModelSimple
    {       
        public int Type { get; set; }
        public int CategoryID { get; set; }       
        public IEnumerable<PictureItem> ListItem { get; set; }
       
    }
}
