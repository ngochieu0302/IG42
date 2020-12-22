using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class LanguagesItem : BaseSimple
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string FallbackCulture { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Icon { get; set; }
        public bool? IsShow { get; set; }
    }

    public class ModelLanguagesItem : BaseModelSimple
    {
        public IEnumerable<LanguagesItem> ListItem { get; set; }
        public string Name { get; set; }
    }
}
