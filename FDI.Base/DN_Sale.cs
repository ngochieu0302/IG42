//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FDI.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class DN_Sale
    {
        public DN_Sale()
        {
            this.SaleCodes = new HashSet<SaleCode>();
            this.Shop_Product_Detail = new HashSet<Shop_Product_Detail>();
            this.Categories = new HashSet<Category>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Percent { get; set; }
        public Nullable<decimal> DateStart { get; set; }
        public Nullable<decimal> DateEnd { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<bool> IsAll { get; set; }
        public Nullable<int> QuantityCode { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<bool> IsAgency { get; set; }
        public Nullable<System.Guid> UserCreate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.Guid> UserUpdate { get; set; }
        public Nullable<bool> IsMonth { get; set; }
        public Nullable<bool> IsDay { get; set; }
        public Nullable<decimal> PriceLimit { get; set; }
        public Nullable<decimal> TotalUse { get; set; }
        public Nullable<decimal> TotalOrder { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<bool> IsEnd { get; set; }
    
        public virtual DN_Users DN_Users { get; set; }
        public virtual DN_Users DN_Users1 { get; set; }
        public virtual ICollection<SaleCode> SaleCodes { get; set; }
        public virtual ICollection<Shop_Product_Detail> Shop_Product_Detail { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
