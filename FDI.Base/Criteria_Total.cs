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
    
    public partial class Criteria_Total
    {
        public int ID { get; set; }
        public Nullable<int> TotalID { get; set; }
        public Nullable<int> CriteriaID { get; set; }
        public Nullable<decimal> Value { get; set; }
    
        public virtual DN_Criteria DN_Criteria { get; set; }
        public virtual DN_Total_SalaryMonth DN_Total_SalaryMonth { get; set; }
    }
}