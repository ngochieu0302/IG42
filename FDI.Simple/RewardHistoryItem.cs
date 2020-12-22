using System.Collections.Generic;
using FDI.Base;

namespace FDI.Simple
{
    public class RewardHistoryItem : BaseSimple
    {
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string CMND { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceOrder { get; set; }
        public int? OrderID { get; set; }
        public decimal? Date { get; set; }
        public int? AgencyId { get; set; }
        public int? BonusTypeId { get; set; }
        public int? Type { get; set; }

		public List<Shop_Order_Details> OrderDetail { get; set; }

    }
    public class ModelRewardHistoryItem : BaseModelSimple
    {
        public IEnumerable<RewardHistoryItem> ListItems { get; set; }
        public int Count { get; set; }
    }
}
