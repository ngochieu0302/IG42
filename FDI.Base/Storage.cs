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
    
    public partial class Storage
    {
        public Storage()
        {
            this.DN_Import = new HashSet<DN_Import>();
        }
    
        public int ID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<decimal> DateImport { get; set; }
        public Nullable<decimal> Payment { get; set; }
    
        public virtual ICollection<DN_Import> DN_Import { get; set; }
        public virtual DN_Users DN_Users { get; set; }
    }
}
