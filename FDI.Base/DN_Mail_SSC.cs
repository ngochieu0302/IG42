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
    
    public partial class DN_Mail_SSC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DN_Mail_SSC()
        {
            this.DN_StatusEmail = new HashSet<DN_StatusEmail>();
            this.DN_File_Mail = new HashSet<DN_File_Mail>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<decimal> CreateDate { get; set; }
        public Nullable<decimal> UpdateDate { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<bool> StatusEmail { get; set; }
        public Nullable<System.Guid> UserSendId { get; set; }
        public Nullable<System.Guid> UserReceiveId { get; set; }
        public Nullable<int> CustomerSendId { get; set; }
        public Nullable<int> CustomerReceiveId { get; set; }
        public Nullable<int> AgencyID { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<bool> IsImportant { get; set; }
        public Nullable<bool> IsRecycleBin { get; set; }
        public Nullable<bool> IsSpam { get; set; }
        public Nullable<bool> IsDraft { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Customer Customer1 { get; set; }
        public virtual DN_Users DN_Users { get; set; }
        public virtual DN_Users DN_Users1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DN_StatusEmail> DN_StatusEmail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DN_File_Mail> DN_File_Mail { get; set; }
    }
}
