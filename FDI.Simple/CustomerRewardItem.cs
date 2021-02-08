using System;
using System.Collections.Generic;
using FDI.Base;

namespace FDI.Simple
{
    public class CustomerRewardItem:BaseSimple
    {
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerUser { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string CMND { get; set; }
        public string Phone { get; set; }
        public int? AgencyID { get; set; }
        public int? Count { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public decimal? TotalReward { get; set; }
        public decimal? TotalReceipt { get; set; }
        public decimal? Total { get; set; }
        public decimal? PriceCus { get; set; }
        public decimal? PriceAgency { get; set; }
        public decimal? PriceSouce { get; set; }
        public decimal? PricePacket { get; set; }
        public decimal? PriceReward { get; set; }
        public decimal? PriceReceive { get; set; }
    }

    public class ModelCustomerRewardItem : BaseModelSimple
    {
        public IEnumerable<CustomerRewardItem> ListItems { get; set; }
        public string DateMonth { get; set; }
        public string DateYear { get; set; }
        public decimal? TotalReward { get; set; }
        public decimal? TotalReceipt { get; set; }
        public ReceiveHistory ReceiveHistorys { get; set; }
    }
}
