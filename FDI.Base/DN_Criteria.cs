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
    
    public partial class DN_Criteria
    {
        public DN_Criteria()
        {
            this.Criteria_Total = new HashSet<Criteria_Total>();
            this.DN_Salary = new HashSet<DN_Salary>();
            this.DN_Roles = new HashSet<DN_Roles>();
            this.DN_Users = new HashSet<DN_Users>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> AgencyID { get; set; }
        public Nullable<bool> IsAll { get; set; }
        public Nullable<bool> IsSchedule { get; set; }
        public Nullable<int> TypeID { get; set; }
    
        public virtual ICollection<Criteria_Total> Criteria_Total { get; set; }
        public virtual DN_TypeCriteria DN_TypeCriteria { get; set; }
        public virtual ICollection<DN_Salary> DN_Salary { get; set; }
        public virtual ICollection<DN_Roles> DN_Roles { get; set; }
        public virtual ICollection<DN_Users> DN_Users { get; set; }
    }
}
