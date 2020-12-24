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
    
    public partial class StorageFreightWarehouse
    {
        public StorageFreightWarehouse()
        {
            this.FreightWarehouses = new HashSet<FreightWarehouse>();
            this.FreightWareHouse_Active = new HashSet<FreightWareHouse_Active>();
        }
    
        public int ID { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string Note { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<decimal> DateImport { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<System.Guid> UserActive { get; set; }
        public Nullable<decimal> DateActive { get; set; }
        public Nullable<int> AgencyReceiveID { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.Guid> keyreq { get; set; }
        public Nullable<bool> IsOrder { get; set; }
    
        public virtual DN_Users DN_Users { get; set; }
        public virtual DN_Users DN_Users1 { get; set; }
        public virtual ICollection<FreightWarehouse> FreightWarehouses { get; set; }
        public virtual ICollection<FreightWareHouse_Active> FreightWareHouse_Active { get; set; }
    }
}