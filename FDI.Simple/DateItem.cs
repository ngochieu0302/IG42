using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    public class DateItem
    {
        public int I { get; set; }
        public decimal S { get; set; }
        public decimal E { get; set; }
    }
    public class DayItem
    {
        public DateItem Item { get; set; }
        public DateTime Date { get; set; }
        public int Thu { get; set; }
    }
    public class MonthItem
    {
        public DateItem Item { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDisCount { get; set; }
        public int Month { get; set; }
    }
    public class MonthEItem
    {
        public int? T { get; set; }
        public decimal? Percent { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalSSC { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalAward { get; set; }
    }
    public class ModelMonthEItem : BaseModelSimple
    {
        public BonusTypeItem BonusTypeItem { get; set; }
        public IEnumerable<MonthEItem> ListItem { get; set; }
    }
    public class ModelMonthItem : BaseModelSimple
    {
        public EnterprisesItem Item { get; set; }
        public BonusTypeItem BonusTypeItem { get; set; }
        public IEnumerable<MonthItem> ListItem { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDiscount { get; set; }
    }
}
