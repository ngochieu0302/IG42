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
    
    public partial class StorageWarehousing
    {
        public StorageWarehousing()
        {
            this.DN_RequestWare = new HashSet<DN_RequestWare>();
            this.DN_RequestWarehousing = new HashSet<DN_RequestWarehousing>();
            this.StorageWarehousingLogs = new HashSet<StorageWarehousingLog>();
            this.StorageWarehousingUsers = new HashSet<StorageWarehousingUser>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public int Status { get; set; }
        public Nullable<decimal> DateRecive { get; set; }
        public Nullable<decimal> PrizeMoney { get; set; }
        public string UrlConfirm { get; set; }
        public Nullable<int> CustomerId { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual DN_Agency DN_Agency { get; set; }
        public virtual ICollection<DN_RequestWare> DN_RequestWare { get; set; }
        public virtual ICollection<DN_RequestWarehousing> DN_RequestWarehousing { get; set; }
        public virtual ICollection<StorageWarehousingLog> StorageWarehousingLogs { get; set; }
        public virtual ICollection<StorageWarehousingUser> StorageWarehousingUsers { get; set; }
    }
}