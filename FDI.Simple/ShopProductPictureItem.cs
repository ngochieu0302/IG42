using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class ShopProductPictureItem : BaseSimple
    {
        public int? PictureID { get; set; }
        public int? ProductID { get; set; }
        public int? Sort { get; set; }
        public string UrlPicture { get; set; }
    }

    public class ModelShopProductPictureItem : BaseModelSimple
    {
        public List<ShopProductPictureItem> ListShopProductPictureItem { get; set; }
        public ShopProductPictureItem ShopProductPictureItem { get; set; }
    }
}
