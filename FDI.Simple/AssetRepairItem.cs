using System.Collections.Generic;

namespace FDI.Simple
{
    public class AssetRepairItem:BaseSimple
    {
        public int? AssetID { get; set; }
        public string AssetName { get; set; }
        public string Description { get; set; }
        public decimal? DateRepair { get; set; }
        public decimal? Price { get; set; }
        public string IsDeleted { get; set; }
    }

    public class ModelAssetRepairItem : BaseModelSimple
    {
        public List<AssetRepairItem> ListItems { get; set; }
    }
}
