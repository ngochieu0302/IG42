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
    
    public partial class TotalStorageWare
    {
        public TotalStorageWare()
        {
            this.StorageProducts = new HashSet<StorageProduct>();
        }
    
        public int ID { get; set; }
        public Nullable<int> CateID { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> QuantityOut { get; set; }
        public Nullable<decimal> Today { get; set; }
        public Nullable<int> Hour { get; set; }
        public Nullable<int> AreaID { get; set; }
        public Nullable<int> MarketID { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual ICollection<StorageProduct> StorageProducts { get; set; }
    }
}