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
    
    public partial class Promotion_Product
    {
        public Promotion_Product()
        {
            this.Shop_Order_Details = new HashSet<Shop_Order_Details>();
        }
    
        public int ID { get; set; }
        public Nullable<int> PromotionID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> IsEnd { get; set; }
        public Nullable<decimal> Percent { get; set; }
    
        public virtual DN_Promotion DN_Promotion { get; set; }
        public virtual ICollection<Shop_Order_Details> Shop_Order_Details { get; set; }
    }
}