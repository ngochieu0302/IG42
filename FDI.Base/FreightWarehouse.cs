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
    
    public partial class FreightWarehouse
    {
        public int ID { get; set; }
        public Nullable<int> StorageProductID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<decimal> Date { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
    
        public virtual Shop_Product_Detail Shop_Product_Detail { get; set; }
        public virtual StorageFreightWarehouse StorageFreightWarehouse { get; set; }
    }
}
