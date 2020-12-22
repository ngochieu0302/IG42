using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class DayOffItem : BaseSimple
    {
        public string Name { get; set; }
        public decimal? Date { get; set; }
        public decimal? DateEnd { get; set; }
        public bool? IsYear { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsShow { get; set; }
        public int? AgencyID { get; set; }
        public int? Quantity { get; set; }
    }
    public class DateOffItem
    {
        public decimal? Date { get; set; }
        public int? Quantity { get; set; }
    }
    public class ModelDayOffItem : BaseModelSimple
    {
        public DayOffItem Item { get; set; }
        public IEnumerable<DayOffItem> ListItem { get; set; }
    }
}
