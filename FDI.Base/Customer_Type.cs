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
    
    public partial class Customer_Type
    {
        public Customer_Type()
        {
            this.Order_Package = new HashSet<Order_Package>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> Day { get; set; }
        public Nullable<int> PictureId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> Border { get; set; }
        public string Color { get; set; }
        public Nullable<int> Sort { get; set; }
    
        public virtual Customer_TypeGroup Customer_TypeGroup { get; set; }
        public virtual Gallery_Picture Gallery_Picture { get; set; }
        public virtual ICollection<Order_Package> Order_Package { get; set; }
    }
}
