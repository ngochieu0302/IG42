using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    [Serializable]
    public class SimItem : BaseSimple
    {
        public int? AgencyID { get; set; }
        public string UrlPicture { get; set; }
        public string Name { get; set; }
        public string CodeSku { get; set; }
        public decimal? PriceNew { get; set; }
        public int? Total { get; set; }
        public int? Marks { get; set; }
        public int? Button { get; set; }
        public int? SupplierID { get; set; }
        public int? HomeID { get; set; }
        public decimal? DateUpdate { get; set; }
    }
    public class ModelSimItem : BaseModelSimple
    {
        public IEnumerable<SimItem> ListItem { get; set; }
        //public ProductItem ProductItem { get; set; }
        //public CategoryItem CategoryItem { get; set; }
        //public decimal? Total { get; set; }
        //public decimal? TotalOld { get; set; }
        //public int Quantity { get; set; }
    }
}
