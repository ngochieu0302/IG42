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
    
    public partial class OrderDetail
    {
        public int ID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<decimal> DateEnd { get; set; }
        public Nullable<decimal> DateCreate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsTime { get; set; }
        public Nullable<byte> StatusPayment { get; set; }
        public int CustomerId { get; set; }
        public Nullable<decimal> DateUpdateStatus { get; set; }
        public Nullable<int> Check { get; set; }
        public Nullable<bool> IsPrestige { get; set; }
    
        public virtual Order Order { get; set; }
    }
}
