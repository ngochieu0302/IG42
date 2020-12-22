using System.Collections.Generic;

namespace FDI.Simple
{
    public class AssetsItem : BaseSimple
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? DateBuy { get; set; }
        public decimal? DateGuarantee { get; set; }
        public decimal? DateLiquidation { get; set; }
        public int? Status { get; set; }
        public string Serial { get; set; }
        public decimal? Price { get; set; }
        public decimal? Depreciation { get; set; }
        public string Description { get; set; }
        public bool? IsDeleted { get; set; }
    }
    public class ModelAssetsItem : BaseModelSimple
    {
        public List<AssetsItem> ListItems { get; set; }
    }
}
