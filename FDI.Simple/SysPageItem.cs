using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class SysPageItem : BaseSimple
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int? Type { get; set; }
        public int? Level { get; set; }
        public string Layout { get; set; }
        public int? Sort { get; set; }
        public decimal? CreateDate { get; set; }
        public string SeoPicture { get; set; }
        public string SeoTitle { get; set; }
        public int? Page { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeyword { get; set; }
        public SEOItem SeoItem { get; set; }
        public string FeUrl { get; set; }
        public IEnumerable<ModeItem> LstModeItems { get; set; }
    }

    public class ModelSysPageItem : BaseModelSimple
    {
        public IEnumerable<SysPageItem> ListItem { get; set; }
    }


}

