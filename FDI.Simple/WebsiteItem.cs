using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class WebsiteItem : BaseSimple
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsTaget { get; set; }
        public string LanguageId { get; set; }
    }
    public class ModelLinkWebsiteItem : BaseModelSimple
    {

        public IEnumerable<WebsiteItem> ListItem { get; set; }
    }
}
