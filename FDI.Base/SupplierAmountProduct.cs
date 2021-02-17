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
    
    public partial class SupplierAmountProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SupplierAmountProduct()
        {
            this.Shop_Product_Comingsoon = new HashSet<Shop_Product_Comingsoon>();
        }
    
        public int ID { get; set; }
        public int SupplierId { get; set; }
        public Nullable<int> ProductID { get; set; }
        public decimal PublicationDate { get; set; }
        public Nullable<decimal> ExpireDate { get; set; }
        public bool IsAlwayExist { get; set; }
        public int AmountEstimate { get; set; }
        public int AmountPayed { get; set; }
        public Nullable<decimal> CallDate { get; set; }
        public Nullable<System.Guid> UserActiveId { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> CreatedDate { get; set; }
        public bool IsDelete { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual DN_Supplier DN_Supplier { get; set; }
        public virtual DN_Users DN_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shop_Product_Comingsoon> Shop_Product_Comingsoon { get; set; }
    }
}
