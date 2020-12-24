using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class PictureAppIG4Item : BaseSimple
    {
        public int? AlbumID { get; set; }
        public int? CategoryID { get; set; }
        public string CateName { get; set; }
        public int? ModuleType { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDeleted { get; set; }
        public int? SourceID { get; set; }
        public string Folder { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public CategoryAppIG4Item Category { get; set; }
    }
    public class ModelPictureAppIG4Item : BaseModelSimple
    {
        public string Container { get; set; }
        public int Type { get; set; }
        public int CategoryID { get; set; }
        public string Action { get; set; }
        public IEnumerable<PictureItem> ListItem { get; set; }

    }
}
