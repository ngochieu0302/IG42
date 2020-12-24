using System;
using System.Collections.Generic;
using FDI.Base;

namespace FDI.Simple
{
    public class CustomerRewardAppIG4DA : BaseSimple
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
        public string Email { get; set; }
        public decimal? TotalReward { get; set; }

        public decimal? TotalReceipt { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalCP { get; set; }
        public decimal? PricePersonal { get; set; }
        public decimal? PriceParent { get; set; }
        public decimal? PriceReward { get; set; }
        public decimal? PriceReceive { get; set; }
        public decimal? PriceReceiver { get; set; }
        public decimal? CashOutWallet { get; set; }
        
        public decimal? PricenoteActive { get; set; }
    }

    public class ModelCustomerRewardAppIG4Item : BaseModelSimple
    {
        public IEnumerable<CustomerRewardAppIG4DA> ListItems { get; set; }
        public string DateMonth { get; set; }
        public string DateYear { get; set; }
        public decimal? TotalReward { get; set; }
        public decimal? TotalReceipt { get; set; }
        public ReceiveHistory ReceiveHistorys { get; set; }
    }
}
