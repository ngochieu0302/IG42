using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDI.Simple
{
    [Serializable]
    public class AnalysisItem
    {
        public int? CountC { get; set; }
        public decimal? PriceReward { get; set; }
        public decimal? PriceReceive { get; set; }
        public int? CountO { get; set; }
        public decimal? Pending { get; set; }
        public decimal? Processing { get; set; }
        public decimal? Complete { get; set; }
        public decimal? Cancelled { get; set; }
        public decimal? Refunded { get; set; }
        public decimal? Payment { get; set; }
        public decimal? Receipt { get; set; }
        public decimal? CashAdvance { get; set; }
        public decimal? Repay { get; set; }
        public int? CountU { get; set; }
        public IEnumerable<ProductExportItem> ListItem { get; set; }
        public int? Onl { get; set; }
        public int? Later { get; set; }
    }
    public class LineItem
    {
        public string Line { get; set; }
        public IEnumerable<ValueAnalysisItem> ListLine { get; set; }
    }
    public class ModelLineItem
    {
        public IEnumerable<int> Names { get; set; }
        public IEnumerable<LineItem> ListItem { get; set; }
    }
    public class ValueAnalysisItem
    {
        public decimal? Value { get; set; }
        public decimal? Date { get; set; }
    }
    public class ModelAnalysisItem
    {
        public IEnumerable<AnalysisItem> ListItem { get; set; }
    }
}
