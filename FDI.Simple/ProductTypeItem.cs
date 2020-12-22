using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class ProductTypeItem : BaseSimple
    {
        public string Name { get; set; }
        public string NameAscii { get; set; }
        public int PictureID { get; set; }
        public bool IsActived { get; set; }
        public bool IsHasSize { get; set; }
        public bool IsHasWeight { get; set; }
        public bool IsHasColor { get; set; }
        public bool IsHasBrand { get; set; }
        public string Description { get; set; }

        public int ProductTypeTotalProducts { get; set; }
    }
    public class ModelProductTypeItem : BaseModelSimple
    {
        public IEnumerable<ProductTypeItem> ListItem { get; set; }
    }
}
