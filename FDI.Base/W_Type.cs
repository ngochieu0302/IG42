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
    
    public partial class W_Type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public W_Type()
        {
            this.W_Category = new HashSet<W_Category>();
            this.W_Effect = new HashSet<W_Effect>();
            this.W_Spacer = new HashSet<W_Spacer>();
            this.W_Type1 = new HashSet<W_Type>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentID { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> SupplierID { get; set; }
        public Nullable<int> Sort { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<W_Category> W_Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<W_Effect> W_Effect { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<W_Spacer> W_Spacer { get; set; }
        public virtual W_Supplier W_Supplier { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<W_Type> W_Type1 { get; set; }
        public virtual W_Type W_Type2 { get; set; }
    }
}