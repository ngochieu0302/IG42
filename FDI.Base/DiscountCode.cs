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
    
    public partial class DiscountCode
    {
        public int ID { get; set; }
        public Nullable<int> PromotionId { get; set; }
        public string Code { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<bool> IsComplete { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<bool> IsSMS { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    
        public virtual Promotion Promotion { get; set; }
    }
}