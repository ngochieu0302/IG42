using System;
using System.Collections.Generic;
using FDI.Base;

namespace FDI.Simple
{
    public class CustomerRewardAppIG4Item : BaseSimple
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

    public class StaticWalletsTotal
    {
        public decimal? Total { get; set; }
        public decimal? Percent { get; set; }
        public decimal? TotalCustomer { get; set; }
        public decimal? TotalAgency { get; set; }
        public decimal? TotalSouce { get; set; }

        public decimal? DateCreate { get; set; }

    }

    public class ListRewardAgencyApp
    {
        public string Avatar { get; set; }
        public string Fullname { get; set; }
        public decimal? Total { get; set; }
        public string Des { get; set; }
        public  decimal? Date { get; set; }

    }

    public class TotalRefAppItem
    {
        public int customer { get; set; }
        public int agency { get; set; }

        public int souce { get; set; }

        public int ctv { get; set; }

    }
}
