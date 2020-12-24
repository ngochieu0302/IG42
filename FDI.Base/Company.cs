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
    
    public partial class Company
    {
        public Company()
        {
            this.P_Workshop = new HashSet<P_Workshop>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public string NameRepresent { get; set; }
        public string Bank { get; set; }
        public string NumberBank { get; set; }
        public Nullable<decimal> DateCreate { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public Nullable<decimal> DateUpdate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Phone { get; set; }
    
        public virtual DN_Users DN_Users { get; set; }
        public virtual ICollection<P_Workshop> P_Workshop { get; set; }
    }
}