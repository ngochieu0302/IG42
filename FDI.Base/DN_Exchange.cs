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
    
    public partial class DN_Exchange
    {
        public int ID { get; set; }
        public Nullable<int> BedDeskID { get; set; }
        public Nullable<int> BedDeskExID { get; set; }
        public Nullable<decimal> StartDate { get; set; }
        public Nullable<decimal> EndDate { get; set; }
        public Nullable<int> AgencyID { get; set; }
    
        public virtual DN_Bed_Desk DN_Bed_Desk { get; set; }
        public virtual DN_Bed_Desk DN_Bed_Desk1 { get; set; }
    }
}