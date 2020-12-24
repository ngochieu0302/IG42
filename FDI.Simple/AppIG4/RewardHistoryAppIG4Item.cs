using System;
using System.Collections.Generic;
using FDI.Base;

namespace FDI.Simple
{
    public class RewardHistoryAppIG4Item : BaseSimple
    {
        public int? CustomerID { get; set; }
        public int? CustomerIDR { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string CMND { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceOrder { get; set; }
        public int? OrderID { get; set; }
        public string TransitionCode { get; set; }
        public decimal? Date { get; set; }
        public int? AgencyId { get; set; }
        public string Query { get; set; }
        public int? BonusTypeId { get; set; }
        public int? Type { get; set; }
        public decimal? DateCreate { get; set; }
        public decimal? PriceDeposit { get; set; }
        public int? WalletCusID { get; set; }
        public bool? IsActive { get; set; }
        public decimal? DateActive { get; set; }
        public string TXID { get; set; }
        public string BonustypeName { get; set; }
        public string CustomerBuy { get; set; }
        public string Email { get; set; }
        public decimal? Percent { get; set; }
        public decimal? TotalPrice { get; set; }
        public string CustomerR { get; set; }

		public List<OrderDetail> OrderDetail { get; set; }


    }
    public class ModelRewardHistoryAppIG4Item : BaseModelSimple
    {
        public IEnumerable<RewardHistoryAppIG4Item> ListItems { get; set; }
        public int Count { get; set; }
        public int ID { get; set; }
        public int? TotalCount { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Totalpage { get; set; }
        public int? page { get; set; }
        public int? Totalre { get; set; }
        public decimal? totalall { get; set; }
        public int? Record { get; set; }
    }
}
