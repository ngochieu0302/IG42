//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FDI.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class PolicyAgency
    {
        public int ID { get; set; }
        public int CategoryId { get; set; }
        public int LevelAgency { get; set; }
        public decimal Quantity { get; set; }
        public int Formula { get; set; }
        public decimal Profit { get; set; }
        public System.Guid UserId { get; set; }
        public decimal DateCreate { get; set; }
        public Nullable<System.Guid> UserUpdate { get; set; }
        public Nullable<decimal> DateUpdate { get; set; }
        public bool Isdelete { get; set; }
        public Nullable<decimal> PercentProfit { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
