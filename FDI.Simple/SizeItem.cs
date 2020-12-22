using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class SizeItem : BaseSimple
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool? IsShow { get; set; }
        public int? Sort { get; set; }
        public string LanguageId { get; set; }
    }
    public class ModelSizeItem : BaseModelSimple
    {

        public IEnumerable<SizeItem> ListItem { get; set; }
    }
}
