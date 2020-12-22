using System;
using System.Collections.Generic;

namespace FDI.Simple
{
    public class EnterprisesItem : BaseSimple
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsTest { get; set; }
        public string Url { get; set; }
        public string DomainDN { get; set; }
        public DateTime? DateCreate { get; set; }
        public decimal? DateStart { get; set; }
        public decimal? DateEnd { get; set; }
        public string CMTND { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDisCount { get; set; }
        public decimal? Percent { get; set; }
        public decimal? PercentOrder { get; set; }
        
        public string PictureUrl { get; set; }
        public string Content { get; set; }
    }
    
    public class ModelEnterprisesItem : BaseModelSimple
    {
        public EnterprisesItem Item { get; set; }
        public IEnumerable<EnterprisesItem> ListItem { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalPay { get; set; }
        public decimal? TotalDiscount { get; set; }
    }
    public class ModelEnterprisesStaticItem : BaseModelSimple
    {
        public decimal? TotalPrice { get; set; }
        public int? TotalOrder { get; set; }
        public int? TotalAgent { get; set; }
        public int? TotalCustomer { get; set; }
    }
    public class ModelEnterprisesTotalItem : BaseModelSimple
    {
        public decimal? TotalReceipt { get; set; }
        public decimal? TotalPayment { get; set; }
        public decimal? TotalCash { get; set; }
        public decimal? TotalRepay { get; set; }
    }
    public class ModelTotalItem : BaseSimple
    {
        public ModelEnterprisesStaticItem Items { get; set; }
        public ModelEnterprisesTotalItem Item2 { get; set; }
    }
}
