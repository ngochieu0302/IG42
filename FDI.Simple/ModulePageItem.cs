using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ModulePageItem : BaseSimple
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? Type { get; set; }
        public string Layout { get; set; }
        public int? Sort { get; set; }
        public decimal? CreateDate { get; set; }
        public int? Level { get; set; }
        public string SEOTitle { get; set; }
        public string SEODescription { get; set; }
        public string SEOKeyword { get; set; }
        public bool? IsDelete { get; set; }
        public string FeUrl { get; set; }
        
    }

    public class ModelModulePageItem : BaseModelSimple
    {
        public IEnumerable<ModuleItem> ListItem { get; set; }
    }
}
