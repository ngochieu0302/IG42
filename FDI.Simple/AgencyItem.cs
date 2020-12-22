using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class AgencyItem : BaseSimple
    {
        public int? EnterpriseID { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal? CreateDate { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }
        public decimal? DateLock { get; set; }
        public bool? IsLock { get; set; }
        public bool? IsOut { get; set; }
        public bool? IsShow { get; set; }
        public string Code { get; set; }
        public string IPTimekeep { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public int? Port { get; set; }
        public int? GroupID { get; set; }
        public int? MarketID { get; set; }
        public int? AreaID { get; set; }
        public decimal? PriceReceive { get; set; }
        public decimal? PriceReward { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDisCount { get; set; }
        public decimal? TotalDeposit { get; set; }
        public decimal? Percent { get; set; }
        public string Latitute { get; set; }
        public string Longitude { get; set; }
        public bool? IsFdi { get; set; }
        public decimal? WalletValue { get; set; }
        public decimal? Cashout { get; set; }
        public string Token { get; set; }
        public int? AgencyLevelId { get; set; }
        public IEnumerable<DocumentItem> LstDocumentItems { get; set; }
        public IEnumerable<ImportProductItem> LstImportProductItems { get; set; }
    }

    public class DNAgencyItem
    {
        public string NameAgency { get; set; }
        public string AddressAgency { get; set; }
        public string UserName { get; set; }
    }
    public class DNAgencyAppItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Latitute { get; set; }
        public string Longitude { get; set; }
        public string Mobile { get; set; }
        public double? Km { get; set; }
        public IEnumerable<ProductValueAppItem> ListItem { get; set; }
    }

    public class ProductValueAppItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public decimal? TotalQuantity { get; set; }
        public decimal? Price { get; set; }
    }
    public class ModelAgencyItem : BaseModelSimple
    {
        public AgencyItem Item { get; set; }
        public IEnumerable<AgencyItem> ListItem { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDiscount { get; set; }
    }
}
