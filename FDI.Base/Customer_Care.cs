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
    
    public partial class Customer_Care
    {
        public int ID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public string Note { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
