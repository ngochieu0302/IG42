using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class AdvertisingTypeItem : BaseSimple
    {
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }


    }
    public class ModelAdvertisingTypeItem : BaseModelSimple
    {
        public IEnumerable<AdvertisingTypeItem> ListItem { get; set; }
    }
}
