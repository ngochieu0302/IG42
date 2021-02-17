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
    
    public partial class DN_AttributeDynamic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DN_AttributeDynamic()
        {
            this.AttributeOptions = new HashSet<AttributeOption>();
            this.Categories = new HashSet<Category>();
        }
    
        public int ID { get; set; }
        public Nullable<int> AttributeGroupID { get; set; }
        public Nullable<int> ControlType { get; set; }
        public Nullable<int> CategoryControlID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<bool> Validate { get; set; }
        public Nullable<int> Sort { get; set; }
        public string LanguageId { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> AgencyID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AttributeOption> AttributeOptions { get; set; }
        public virtual CategoryControl CategoryControl { get; set; }
        public virtual DN_AttributeGroup DN_AttributeGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
    }
}
