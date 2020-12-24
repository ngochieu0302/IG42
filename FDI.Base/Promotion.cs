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
    
    public partial class Promotion
    {
        public Promotion()
        {
            this.DiscountCodes = new HashSet<DiscountCode>();
            this.Promotion_Procuct = new HashSet<Promotion_Procuct>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string UserCreated { get; set; }
        public string Content { get; set; }
        public string ShopId { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<decimal> FromPrice { get; set; }
        public Nullable<decimal> ToPrice { get; set; }
        public string Mode { get; set; }
        public Nullable<decimal> Sale { get; set; }
        public Nullable<int> Remain { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsSMS { get; set; }
    
        public virtual ICollection<DiscountCode> DiscountCodes { get; set; }
        public virtual ICollection<Promotion_Procuct> Promotion_Procuct { get; set; }
    }
}