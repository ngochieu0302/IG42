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
    
    public partial class DN_Export
    {
        public DN_Export()
        {
            this.Export_Product_Value = new HashSet<Export_Product_Value>();
        }
    
        public int ID { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<System.Guid> UserGet { get; set; }
        public string Code { get; set; }
        public Nullable<decimal> DateCreated { get; set; }
        public string Note { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> DateExport { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<bool> IsOrder { get; set; }
    
        public virtual DN_Users DN_Users { get; set; }
        public virtual DN_Users DN_Users1 { get; set; }
        public virtual ICollection<Export_Product_Value> Export_Product_Value { get; set; }
    }
}