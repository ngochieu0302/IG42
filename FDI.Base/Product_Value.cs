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
    
    public partial class Product_Value
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product_Value()
        {
            this.DN_ImportProduct = new HashSet<DN_ImportProduct>();
            this.Shop_Order_Details = new HashSet<Shop_Order_Details>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string Barcode { get; set; }
        public Nullable<decimal> PriceNew { get; set; }
        public Nullable<decimal> PriceCost { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<decimal> DateImport { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<int> CountSP { get; set; }
        public Nullable<int> QuantityOut { get; set; }
        public Nullable<int> CateValueID { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<int> UnitID { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public string IdLog { get; set; }
        public Nullable<int> ProduceId { get; set; }
    
        public virtual Cate_Value Cate_Value { get; set; }
        public virtual DN_Agency DN_Agency { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DN_ImportProduct> DN_ImportProduct { get; set; }
        public virtual DN_Unit DN_Unit { get; set; }
        public virtual Produce Produce { get; set; }
        public virtual Shop_Product_Detail Shop_Product_Detail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shop_Order_Details> Shop_Order_Details { get; set; }
    }
}
