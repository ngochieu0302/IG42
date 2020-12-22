using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class HtmlItem : BaseSimple
    {
        public int CtrId { get; set; }
        public int PageId { get; set; }
        public int Sort { get; set; }
        public string Name { get; set; }
        public string Section { get; set; }
        public string Value { get; set; }
        
    }
    public class ModelHtmlItem : BaseModelSimple
    {
        public IEnumerable<HtmlItem> ListItem { get; set; }

    }
}
