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
    
    public partial class DN_ImportProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DN_ImportProduct()
        {
            this.FreightWareHouse_Active = new HashSet<FreightWareHouse_Active>();
            this.RewardHistories = new HashSet<RewardHistory>();
            this.Shop_Order_Details = new HashSet<Shop_Order_Details>();
        }
    
        public System.Guid GID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> QuantityOut { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<decimal> Date { get; set; }
        public Nullable<decimal> DateEnd { get; set; }
        public string BarCode { get; set; }
        public Nullable<decimal> PriceNew { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> ProductValueID { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<bool> Ischeck { get; set; }
        public Nullable<System.Guid> UserCreated { get; set; }
        public Nullable<System.Guid> UserUpdate { get; set; }
        public Nullable<decimal> CreateDate { get; set; }
        public Nullable<decimal> UpdateDate { get; set; }
    
        public virtual DN_Agency DN_Agency { get; set; }
        public virtual DN_Users DN_Users { get; set; }
        public virtual DN_Users DN_Users1 { get; set; }
        public virtual Product_Value Product_Value { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FreightWareHouse_Active> FreightWareHouse_Active { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RewardHistory> RewardHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shop_Order_Details> Shop_Order_Details { get; set; }
    }
}
