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
    
    public partial class Customer_question
    {
        public Customer_question()
        {
            this.Customer_Review = new HashSet<Customer_Review>();
            this.Customer_Review_Deltails = new HashSet<Customer_Review_Deltails>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> pointID { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<int> AgencyID { get; set; }
    
        public virtual Customer_Point Customer_Point { get; set; }
        public virtual ICollection<Customer_Review> Customer_Review { get; set; }
        public virtual ICollection<Customer_Review_Deltails> Customer_Review_Deltails { get; set; }
    }
}
