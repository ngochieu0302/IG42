using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class AdvertisingPositionItem : BaseSimple
    {
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class ModelAdvertisingPositionItem : BaseModelSimple
    {
        public IEnumerable<AdvertisingPositionItem> ListItem { get; set; }
    }
}
