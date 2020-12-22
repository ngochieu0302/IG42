using System.Collections.Generic;

namespace FDI.Simple
{
    public class BonusTypeItem:BaseSimple
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? RootID { get; set; }
        public decimal? Percent { get; set; }
        public decimal? PercentRoot { get; set; }
        public decimal? PercentParent { get; set; }
        public decimal? DateCreate { get; set; }
    }
    public class ModelBonusTypeItem:BaseModelSimple
    {
        public IEnumerable<BonusTypeItem> ListItems { get; set; }
    }
}
