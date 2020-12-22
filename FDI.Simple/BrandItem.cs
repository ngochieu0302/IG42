using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class BrandItem : BaseSimple
    {
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public bool? IsShow { get; set; }
        public int? PictureId { get; set; }
        public int? LogoPictureId { get; set; }
        public bool? IsDeleted { get; set; }
        public int? ParentId { get; set; }
    }
    public class ModelBrandItem : BaseModelSimple
    {
        public string LstInt { get; set; }
        public IEnumerable<BrandItem> ListItem { get; set; }
        
    }
}
