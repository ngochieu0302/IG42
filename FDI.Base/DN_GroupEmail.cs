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
    
    public partial class DN_GroupEmail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DN_GroupEmail()
        {
            this.DN_Users = new HashSet<DN_Users>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<bool> IsShow { get; set; }
        public Nullable<int> AgencyID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DN_Users> DN_Users { get; set; }
    }
}
