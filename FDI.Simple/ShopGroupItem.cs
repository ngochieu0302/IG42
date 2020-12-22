using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ShopGroupItem : BaseSimple
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int? Sort { get; set; }
        public int? IsLevel { get; set; }
        public bool? IsShow { get; set; }
        public string LanguageId { get; set; }
        public IEnumerable<ProductItem> ListProductItems { get; set; } 
    }
    public class ModelShopGroupItem : BaseModelSimple
    {
        public IEnumerable<ShopGroupItem> ListItem { get; set; }
    }
}
