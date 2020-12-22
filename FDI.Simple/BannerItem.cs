using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class BannerItem : BaseSimple
    {
        public string Name { get; set; }
        public string UrlView { get; set; }
        public string Datas { get; set; }
        public string Details { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public int? Sort { get; set; }
        public int PictureID { get; set; }


    }
    public class ModelBannerItem : BaseModelSimple
    {
        public IEnumerable<BannerItem> ListItem { get; set; }
      
        public int Total { get; set; }
    }

   
}
